using ROLAP.Configuration.Interfaces;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;
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
        var tuples = ProcessAxes(queryModel.Axes, cubeConf);
        var whereTuples = ProcessAxes(queryModel.Where, cubeConf);
        // if (whereTuples.Any(x => x.Members.Any(y => y.Values.Count() > 1)))
        //     throw new Exception("В Where для измерений / мер можно указывать только одно значение");
        return new CubeQuery(queryModel.QueryType, tuples, whereTuples);
    }
    
    
    
    #region ProcessQueryModel
    
    private IEnumerable<CubeItemTuple> ProcessAxes(IEnumerable<AxisItem> axesQuery, CubeConfiguration configurationCube)
    {
        List<CubeItemTuple> tuples = new List<CubeItemTuple>();

        foreach (var axis in axesQuery)
        {
            tuples.Add(ProcessAxisQuery(axis.Execute(configurationCube) as AxisItem,configurationCube));
        }

        return tuples;
    }


    private CubeItemTuple ProcessAxisQuery(AxisItem? axisQuery, CubeConfiguration configurationCube)
    {
        if (axisQuery is null) throw new ArgumentNullException(nameof(axisQuery));

        List<ICubeItem> items = new List<ICubeItem>();
        
        var set = MappingHelper.ToSet(axisQuery.Member);
        
        
        foreach (var tuple in set.Members)
        {
            ProcessQueryTuple(MappingHelper.ToTuple(tuple), configurationCube, ref items);
        }

        return new CubeItemTuple(items);
    }

    private void ProcessQueryTuple(TupleItem? tupleQuery, CubeConfiguration configurationCube, ref List<ICubeItem> cubeItems)
    {
        if (tupleQuery is null) throw new ArgumentNullException(nameof(tupleQuery));

        foreach (var member in tupleQuery.Items)
        {
            var cubeItem = ProcessMember(member as MemberItem, configurationCube);
            if (cubeItem is not null)
            {
                MergeCubeItem(cubeItem, cubeItems);
            }
        }
    }

    private ICubeItem? ProcessMember(MemberItem? memberQuery, CubeConfiguration configurationCube)
    {
        if (memberQuery is null) throw new ArgumentNullException(nameof(memberQuery));
        if (memberQuery.Hierarchy[0].ToLower() == "measure")
        {
            return FindMeasure(configurationCube, memberQuery.Hierarchy[1]);
        }
        else
        {
            return FindDimension(configurationCube, memberQuery.Hierarchy);
        }
    }

    private ICubeItem? FindMeasure(CubeConfiguration configurationCube, string name)
    {
        var measure = configurationCube.Measures.FirstOrDefault(x => x.NameEqual(name));
        if (measure is null) return null;
        List<MeasureCubeItem> measureCubeItems = new List<MeasureCubeItem> { measure.Clone<MeasureCubeItem>() };
        return new MeasureGroupCubeItem("Показатель",measureCubeItems);
    }

    private ICubeItem? FindDimension(CubeConfiguration configurationCube, string[] hierarchy)
    {
        int i = 0;
        
        ICubeItem? result = null;
        ICubeItem? temp = null;
        IEnumerable<DimensionCubeItem> dimensions = configurationCube.Dimensions;
        DimensionCubeItem? current = null;
        
        do
        {
            if (!dimensions.Any()) return null;
            current = (DimensionCubeItem)dimensions.FirstOrDefault(x => x.NameEqual(hierarchy[i]));
            if (current is null) return null;
            if (result is null)
            {
                result = temp = new DimensionCubeItem(current.Key, current.Name, new List<DimensionCubeItem>());
            }
            else
            {
                var newVal = new DimensionCubeItem(current.Key, current.Name, new List<DimensionCubeItem>());
                temp.AddValue(newVal);
                temp = newVal;
            }
            dimensions = current.Values;
            
        } while (++i < hierarchy.Length);
        
        
        return result;
    }

    private void MergeCubeItem(ICubeItem cubeItem, List<ICubeItem> cubeItems)
    {
        if (!cubeItems.Any())
        {
            cubeItems.Add(cubeItem);
            return;
        }

        ICubeItem? prevFind = null;
        IEnumerable<ICubeItem>? temp = cubeItems;
        ICubeItem? currentCubeItem = cubeItem;
        do
        {
            var findedCubeItem = temp.FirstOrDefault(x => x.NameEqual(currentCubeItem.GetName()));
            if(findedCubeItem is null) break;
            prevFind = findedCubeItem;
            
            currentCubeItem = currentCubeItem.GetValues().FirstOrDefault();
            if(currentCubeItem is null || !currentCubeItem.IsContainer()) break;
            
            if(!findedCubeItem.IsContainer()) break;
            temp = findedCubeItem.GetValues();
        } while (currentCubeItem.GetValues().Any());

        if (prevFind is null)
        {
            cubeItems.Add(cubeItem);
        }
        else
        {
            if (!prevFind.GetValues().Any(x => x.NameEqual(currentCubeItem.GetName())))
            {
                prevFind.AddValue(currentCubeItem);
            }
        }
    }
    
    #endregion
}