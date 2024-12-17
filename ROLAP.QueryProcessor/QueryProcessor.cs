using ROLAP.Configuration.Interfaces;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Model.Query;
using ROLAP.QueryProcessor.Helpers;
using ROLAP.QueryProcessor.Models;
using ROLAP.QueryProcessor.Models.Items;
using ROLAP.QueryProcessor.Parser;

namespace ROLAP.QueryProcessor;

public class QueryProcessor
{
    private readonly IConfigurationStore _store;

    public QueryProcessor(IConfigurationStore store)
    {
        _store = store;
    }
    
    public CubeQuery ProcessQuery(string query)
    {
        var queryModel = QueryParser.Parse(query);
        return PrepareQueryModel(queryModel);
    }

    private CubeQuery PrepareQueryModel(QueryModel queryModel)
    {
        var cubeName = queryModel.CubeName;
        var cubeConf = _store.GetConfiguration(cubeName);
        var sets = ProcessAxes(queryModel.Axes, cubeConf);
        var whereSet = ProcessAxes(queryModel.Where, cubeConf).FirstOrDefault();
        // if (whereTuples.Any(x => x.Members.Any(y => y.Values.Count() > 1)))
        //     throw new Exception("В Where для измерений / мер можно указывать только одно значение");
        return new CubeQuery(queryModel.QueryType, sets, whereSet);
    }
    
    
    
    #region ProcessQueryModel
    
    private IEnumerable<CubeItemSet> ProcessAxes(IEnumerable<AxisItem> axesQuery, ICubeConfiguration configurationCube)
    {
        List<CubeItemSet> sets = new List<CubeItemSet>();

        foreach (var axis in axesQuery)
        {
            sets.Add(ProcessAxisQuery(axis.Execute(configurationCube) as AxisItem,configurationCube));
        }

        return sets;
    }


    private CubeItemSet ProcessAxisQuery(AxisItem? axisQuery, ICubeConfiguration configurationCube)
    {
        if (axisQuery is null) throw new ArgumentNullException(nameof(axisQuery));

        List<CubeItemTuple> items = new List<CubeItemTuple>();
        
        var set = MappingHelper.ToSet(axisQuery.Member);
        
        
        foreach (var tuple in set.Members)
        {
           ProcessQueryTuple(MappingHelper.ToTuple(tuple), configurationCube, ref items);
        }

        return new CubeItemSet(items);
    }

    private void ProcessQueryTuple(TupleItem? tupleQuery, ICubeConfiguration configurationCube, ref List<CubeItemTuple> tuples)
    {
        if (tupleQuery is null) throw new ArgumentNullException(nameof(tupleQuery));
        foreach (var member in tupleQuery.Items)
        {
            var tuple = ProcessMember(member as MemberItem, configurationCube);
            if (tuple is not null)
            {
                MergeTuples(tuples, tuple);
            }
        }
    }

    private CubeItemTuple? ProcessMember(MemberItem? memberQuery, ICubeConfiguration configurationCube)
    {
        if (memberQuery is null) throw new ArgumentNullException(nameof(memberQuery));

        var cubeItem = IsMeasure(memberQuery.Hierarchy[0])
            ? FindInEnumerable(configurationCube.GetMeasures(), memberQuery.Hierarchy)
            : FindInEnumerable(configurationCube.GetDimensions(), memberQuery.Hierarchy);
        if (cubeItem is null) return null;
        return new CubeItemTuple(new List<ICubeItem> { cubeItem });
    }

    private ICubeItem? FindInEnumerable(IEnumerable<ICubeItem> cubeItems, string[] hierarchy)
    {
        ICubeItem? find = null;

        foreach (var cubeItem in cubeItems)
        {
            if (cubeItem is IMeasureCubeItem measureCubeItem)
            {
                find = FindMeasureByHierarchy(measureCubeItem, hierarchy);
            }
            else if (cubeItem is IDimensionCubeItem dimensionCubeItem)
            {
                find = FindDimensionByHierarchy(dimensionCubeItem, hierarchy);
            }
            else
            {
                throw new Exception("Неожиданный тип");
            }
            if(find is not null) break;
        }
        
        return find;
    }

    private void MergeTuples(List<CubeItemTuple> tuples, CubeItemTuple tuple)
    {
        if (!tuples.Any())
        {
            tuples.Add(tuple);
            return;
        }

        if (tuple.Members.First() is IMeasureCubeItem)
        {
            MergeMeasureTuple(tuples, tuple);
        }
        else
        {
            MergeDimensionTuple(tuples, tuple);
        }
    }

    private void MergeMeasureTuple(List<CubeItemTuple> tuples, CubeItemTuple tuple)
    {
        var measureTuple = tuples.FirstOrDefault(x => x.Members.FirstOrDefault() is IMeasureCubeItem);
        if (measureTuple is null)
        {
            tuples.Add(tuple);
            return;
        }

        ICubeItem insertItem = tuple.Members.First();
        
        var measureMember = measureTuple.Members.FirstOrDefault(x => x.Equals(insertItem));
        if (measureMember is null)
        {
            measureTuple.AddMember(insertItem);
        }
    }

    private void MergeDimensionTuple(List<CubeItemTuple> tuples, CubeItemTuple tuple)
    {
        IDimensionCubeItem? insertItem = tuple.Members.First() as IDimensionCubeItem;
        if (insertItem is null) throw new Exception("Неожиданный тип");
        
        var dimensionTuples = tuples.Where(x => x.Members.FirstOrDefault() is IDimensionCubeItem);
        var dimensionTuple = dimensionTuples.FirstOrDefault(x => x.Members.First().Equals(insertItem));
        if (dimensionTuple is null)
        {
            tuples.Add(tuple);
            return;
        }


        IDimensionCubeItem? prev = null;
        IEnumerable<IDimensionCubeItem> currentValues = dimensionTuple.Members.Cast<IDimensionCubeItem>();
        
        do
        {
            IDimensionCubeItem? find = null;
            find = currentValues.FirstOrDefault(x => x.Equals(insertItem));
            if (find is null)
            {
                break;
            }
            prev = find;
            insertItem = insertItem.GetDimensions().FirstOrDefault();
            if(insertItem is null) return;
            currentValues = find.GetDimensions();

        } while (currentValues.Any());

        if (prev is null)
        {
            dimensionTuple.AddMember(insertItem);
        }
        else
        {
            prev.AddDimension(insertItem);
        }
    }

    private bool IsMeasure(string name) => name.ToLower() == "measure";

    private ICubeItem? FindMeasureByHierarchy(IMeasureCubeItem measureCubeItem, string[] hierarchy)
    {
        return measureCubeItem.GetName() == hierarchy[1] ? measureCubeItem.Clone(false) : null;
    }
    
    // private ICubeItem? FindDimensionByHierarchy(IDimensionCubeItem dimensionCubeItem, string[] hierarchy)
    // {
    //     if (dimensionCubeItem.GetName() != hierarchy[0]) return null;
    //     IDimensionCubeItem clone = (IDimensionCubeItem)dimensionCubeItem.Clone(false);
    //     IDimensionCubeItem current = clone;
    //
    //     hierarchy = hierarchy.Skip(1).ToArray();
    //
    //     IEnumerable<IDimensionCubeItem> currentValues = dimensionCubeItem.GetDimensions();
    //
    //     while (true)
    //     {
    //         IDimensionCubeItem? find = null;
    //
    //         foreach (var value in currentValues)
    //         {
    //             find = FindDimensionByHierarchy(value,hierarchy) as IDimensionCubeItem;
    //             if(find is not null) break;
    //         }
    //
    //         if (find is null) return null;
    //
    //         current.AddDimension((IDimensionCubeItem)find.Clone(true));
    //         current = current.GetDimensions().First();
    //
    //         if (current is null) return null;
    //         
    //         currentValues = current.GetDimensions();
    //         hierarchy = hierarchy.Skip(1).ToArray();
    //     }
    //
    //     return clone;
    // }

    private ICubeItem? FindDimensionByHierarchy(IDimensionCubeItem dimensionCubeItem, string[] hierarchy)
    {
        if (dimensionCubeItem.GetName() != hierarchy[0]) return null;
        hierarchy = hierarchy.Skip(1).ToArray();
        IDimensionCubeItem clone = (IDimensionCubeItem)dimensionCubeItem.Clone(false);
        IDimensionCubeItem current = clone;
        IEnumerable<IDimensionCubeItem> currentValues = dimensionCubeItem.GetDimensions();
        while (hierarchy.Any())
        {
            var find = currentValues.FirstOrDefault(value => value.GetName() == hierarchy[0]);
            if (find == null) return null;

            currentValues = find.GetDimensions();
            current.AddDimension((IDimensionCubeItem)find.Clone(false));
            current = current.GetDimensions().First();
            hierarchy = hierarchy.Skip(1).ToArray();
        }

        return clone;
    }

    #endregion
}