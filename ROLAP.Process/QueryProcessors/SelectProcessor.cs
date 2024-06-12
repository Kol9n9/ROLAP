using ROLAP.Common.Enums;
using ROLAP.Common.Helpers;
using ROLAP.Common.Model;
using ROLAP.Process.Models.Result;

namespace ROLAP.Process.QueryProcessors;

internal class SelectProcessor
{
    internal CubeResult ExecuteQuery(CubeQuery query)
    {
        IEnumerable<CubeItemTuple> setItems = query.Sets;
        var values = LoadValues(setItems, query.Where);

        var sets = ConvertTupleItems(setItems);
        var aggregatedValues = FillSetsValues(values, sets);
        CubeResult res = new CubeResult(sets,aggregatedValues);
        return res;
    }
    
    #region LoadValues

    private IEnumerable<MeasureValue> LoadValues(IEnumerable<CubeItemTuple> tupleItems, IEnumerable<CubeItemTuple> whereTupleItems)
    {
        List<MeasureValue> values = new List<MeasureValue>();

        var measures =
            tupleItems.Select(x => x.Members.Where(x => x.Type == CubeItemType.Measure)).SelectMany(x => x.SelectMany(y => y.Values)).Cast<MeasureCubeItem>().ToList();

        var dimensions =
            tupleItems.Select(x => x.Members.Where(x => x.Type == CubeItemType.Dimension)).SelectMany(x => x).ToList();

        measures.AddRange(whereTupleItems.Select(x => x.Members.Where(x => x.Type == CubeItemType.Measure)).SelectMany(x => x.SelectMany(y => y.Values)).Cast<MeasureCubeItem>().ToList());
        dimensions.AddRange(whereTupleItems.Select(x => x.Members.Where(x => x.Type == CubeItemType.Dimension)).SelectMany(x => x).ToList());
        
        foreach (var measure in measures)
        {
            values.AddRange(measure.LoadValues(dimensions));
        }
       
        return values;
    }
    
    #endregion

    #region Aggregate

    private List<CubeResultSet> ConvertTupleItems(IEnumerable<CubeItemTuple> tupleItems, bool isAggregate = true)
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
        var measure = members.FirstOrDefault(x => x.Type == CubeItemType.Measure)?.Values
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
    
    #endregion
}