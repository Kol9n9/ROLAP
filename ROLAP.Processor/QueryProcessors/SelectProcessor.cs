using ROLAP.Common.Enums;
using ROLAP.Common.Helpers;
using ROLAP.Common.Model;
using ROLAP.Common.Model.CubeResult;
using ROLAP.Common.Model.Query;
using ROLAP.Configuration.Models.Interfaces;

namespace ROLAP.Processor.QueryProcessors;

public class SelectProcessor
{
    private readonly ICubeConfigurationStore _store;
    public SelectProcessor(ICubeConfigurationStore store)
    {
        _store = store;
    }
    public CubeResult ExecuteQuery(CubeQuery query)
    {
        var cube = _store.GetByName(query.CubeName);
        IEnumerable<CubeResultTuple> tupleItems = ProcessAxes(query.Axes, cube);
        var values = LoadValues(tupleItems);

        var sets = ConvertTupleItems(tupleItems);
        var aggregatedValues = FillSetsValues(values, sets);
        CubeResult res = new CubeResult(sets,aggregatedValues);
        return res;
    }
    
    
    
    #region ProcessCubeQuery
    
    private IEnumerable<CubeResultTuple> ProcessAxes(IEnumerable<CubeAxisQuery> axesQuery,Cube cube)
    {
        List<CubeResultTuple> tuples = new List<CubeResultTuple>();

        foreach (var axis in axesQuery)
        {
            tuples.Add(ProcessAxisQuery(axis.Execute(cube) as CubeAxisQuery,cube));
        }


        return tuples;
    }


    private CubeResultTuple ProcessAxisQuery(CubeAxisQuery? axisQuery, Cube cube)
    {
        if (axisQuery is null) throw new ArgumentNullException(nameof(axisQuery));

        List<CubeItem> items = new List<CubeItem>();
        
        var set = MappingHelper.ToSet(axisQuery.Member);
        
        
        foreach (var tuple in set.Members)
        {
            ProcessQueryTuple(MappingHelper.ToTuple(tuple), cube, ref items);
        }

        return new CubeResultTuple(items);
    }

    private void ProcessQueryTuple(CubeTupleQuery? tupleQuery, Cube cube, ref List<CubeItem> cubeItems)
    {
        if (tupleQuery is null) throw new ArgumentNullException(nameof(tupleQuery));

        foreach (var member in tupleQuery.Items)
        {
            var cubeItem = ProcessMember(member as CubeMemberQuery, cube);
            if (cubeItem is not null)
            {
                MergeCubeItem(cubeItem, cubeItems);
            }
        }
    }

    private CubeItem? ProcessMember(CubeMemberQuery? memberQuery, Cube cube)
    {
        if (memberQuery is null) throw new ArgumentNullException(nameof(memberQuery));
        if (memberQuery.Hierarchy[0].ToLower() == "measure")
        {
            return FindMeasure(cube, memberQuery.Hierarchy[1]);
        }
        else
        {
            return FindDimension(cube, memberQuery.Hierarchy);
        }
    }

    private CubeItem? FindMeasure(Cube cube, string name)
    {
        var measure = cube.Measures.FirstOrDefault(x => x.Name == name);
        if (measure is null) return null;
        var cubeItem = new MeasureCubeItem("Показатель", string.Empty,null);
        cubeItem.Values.Add(new MeasureCubeItem(measure.Name, measure.Key, measure.ValuesLoader));
        return cubeItem;
    }

    private CubeItem? FindDimension(Cube cube, string[] hierarchy)
    {
        int i = 0;
        
        CubeItem? result = null;
        CubeItem? temp = null;
        List<Dimension>? dimensions = cube.Dimensions;
        Dimension? current = null;

        do
        {
            if (!dimensions.Any()) return null;
            current = dimensions.FirstOrDefault(x => x.Name == hierarchy[i]);
            if (current is null) return null;
            if (result is null)
            {
                result = temp = new CubeItem(current.Name, current.Key, CubeItemType.Dimension);
            }
            else
            {
                temp.Values.Add(new CubeItem(current.Name, current.Key, CubeItemType.Dimension));
                temp = temp.Values[0];
            }
            dimensions = current.Values;
            
        } while (++i < hierarchy.Length);


        return result;
    }

    private void MergeCubeItem(CubeItem cubeItem, List<CubeItem> cubeItems)
    {
        if (!cubeItems.Any())
        {
            cubeItems.Add(cubeItem);
            return;
        }

        CubeItem? prevFind = null;
        List<CubeItem>? temp = cubeItems;
        CubeItem? currentCubeItem = cubeItem;
        do
        {
            var findedCubeItem = temp.FirstOrDefault(x => x.Name == currentCubeItem.Name);
            if(findedCubeItem is null) break;
            prevFind = findedCubeItem;
            currentCubeItem = cubeItem.Values[0];
            temp = findedCubeItem.Values;
        } while (cubeItem.Values.Any());

        if (prevFind is null)
        {
            cubeItems.Add(cubeItem);
        }
        else
        {
            if (currentCubeItem.Name != prevFind.Name)
            {
                prevFind.Values.Add(currentCubeItem);
            }
        }
    }
    
  
    
    #endregion

    #region LoadValues

    private IEnumerable<MeasureValue> LoadValues(IEnumerable<CubeResultTuple> tupleItems)
    {
        List<MeasureValue> values = new List<MeasureValue>();

        var measures =
            tupleItems.Select(x => x.Members.Where(x => x.Type == CubeItemType.Measure)).SelectMany(x => x.SelectMany(y => y.Values)).Cast<MeasureCubeItem>();

        var dimensions =
            tupleItems.Select(x => x.Members.Where(x => x.Type == CubeItemType.Dimension)).SelectMany(x => x);

        foreach (var measure in measures)
        {
            values.AddRange(measure.LoadValues(dimensions));
        }
       
        return values;
    }
    
    #endregion

    #region Aggregate

    private List<CubeResultSet> ConvertTupleItems(IEnumerable<CubeResultTuple> tupleItems, bool isAggregate = true)
    {
        List<CubeResultSet> sets = new List<CubeResultSet>();

        foreach (var tuple in tupleItems)
        {
            List<CubeResultTuple> resultTuples = new List<CubeResultTuple>();
            var members = tuple.Members.ToList();
            if (members.Any())
            {
                if (isAggregate)
                {
                    resultTuples = GetTuples(members[^1]);
                    for (int i = members.Count - 2; i >= 0; i--)
                    {
                        resultTuples = Merge(GetTuples(members[i]), resultTuples);
                    }
                }
                else
                {
                    foreach (var member in members)
                    {
                        resultTuples.AddRange(GetTuples(member,isAggregate));
                    }
                }
            }
            sets.Add(new CubeResultSet(resultTuples));
        }

        return sets;
    }
    
    
    private List<CubeResultTuple> Merge(IEnumerable<CubeResultTuple> tuples1, IEnumerable<CubeResultTuple> tuples2)
    {
        List<CubeResultTuple> res = new List<CubeResultTuple>();

        foreach (var tuple1 in tuples1)
        {
            foreach (var tuple2 in tuples2)
            {
                List<CubeItem> items = new List<CubeItem>();
                foreach (var tupleMember in tuple1.Members)
                {
                    items.Add(tupleMember.Clone());
                }

                foreach (var tupleMember in tuple2.Members)
                {
                    items.Add(tupleMember.Clone());
                }
                var newTuple = new CubeResultTuple(items);
                res.Add(newTuple);
            }
        }
    
        return res;
    }
    
    private List<CubeResultTuple> GetTuples(CubeItem cubeItem, bool isAggregate = true)
    {
        List<CubeResultTuple> res = new List<CubeResultTuple>();

        if (isAggregate)
        {
            res.Add(new CubeResultTuple(new List<CubeItem>{cubeItem.Clone(false)}));
        }

        foreach (var value in cubeItem.Values)
        {
            var copy = cubeItem.Clone(false);
            copy.Values.Add(value.Clone(false));
            res.Add(new CubeResultTuple(new List<CubeItem>{copy}));
        }
        return res;
    }

    #endregion


    private IEnumerable<MeasureValue> FillSetsValues(IEnumerable<MeasureValue> values, IEnumerable<CubeResultSet> sets, IEnumerable<CubeResultTuple> prevTuples = null)
    {
        List<MeasureValue> resValues = new List<MeasureValue>();

        if (sets.Count() == 1)
        {
            List<CubeResultTuple> tuples = Merge(prevTuples,sets.FirstOrDefault().Tuples);
            resValues = FillTupleValues(values, tuples).ToList();
        }
        else
        {
            var lastSet = sets.LastOrDefault();
            List<CubeResultTuple> tuples = new List<CubeResultTuple>();
            foreach (var tuple in lastSet.Tuples)
            {
                var copyTuples = new List<CubeResultTuple> { tuple };
                if (prevTuples is not null)
                {
                    copyTuples = Merge(prevTuples, copyTuples);
                }
                resValues.AddRange(FillSetsValues(values,sets.Take(sets.Count()-1),copyTuples));
            }
        }

        return resValues;
    }

    private IEnumerable<MeasureValue> FillTupleValues(IEnumerable<MeasureValue> values,
        IEnumerable<CubeResultTuple> tuples)
    {
        List<MeasureValue> resValues = new List<MeasureValue>();
        
        foreach (var tuple in tuples)
        {
            List<MeasureValue> tupleValues = new List<MeasureValue>();
            foreach (var value in values)
            {
                if(IsValueAggregatated(value,tuple.Members)) tupleValues.Add(value);
            }
            resValues.Add(AggregatedValues(tupleValues));
        }

        return resValues;
    }

    private bool IsValueAggregatated(MeasureValue value, IEnumerable<CubeItem> members)
    {
        var measure = members.Where(x => x.Type == CubeItemType.Measure).FirstOrDefault().Values
            .FirstOrDefault();
        if (measure is not null && value.MeasureKey != measure.Key) return false;
        var dimensions = members.Where(x => x.Type == CubeItemType.Dimension);
        return CubeItemHelper.IsValueInDimensions(value, dimensions);
    }

    private MeasureValue AggregatedValues(IEnumerable<MeasureValue> values)
    {
        var first = values.FirstOrDefault();
        return first ?? new MeasureValue()
        {
            Value = "-"
        };
    }
}