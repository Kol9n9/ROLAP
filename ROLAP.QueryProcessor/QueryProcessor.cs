using ROLAP.Common.Enums;
using ROLAP.Common.Interfaces;
using ROLAP.Common.Model;
using ROLAP.Configuration.Models.Interfaces;
using ROLAP.QueryProcessor.Helpers;
using ROLAP.QueryProcessor.Models;
using ROLAP.QueryProcessor.Models.Items;
using ROLAP.QueryProcessor.Parser;

namespace ROLAP.QueryProcessor;

public class QueryProcessor : IQueryProcessor
{
    private readonly ICubeConfigurationStore _store;

    public QueryProcessor(ICubeConfigurationStore store)
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
        var cubeConf = _store.GetByName(cubeName);
        var tuples = ProcessAxes(queryModel.Axes, cubeConf);
        return new CubeQuery(queryModel.QueryType, tuples);
    }
    
    
    
    #region ProcessQueryModel
    
    private IEnumerable<CubeItemTuple> ProcessAxes(IEnumerable<AxisItem> axesQuery,ConfigurationCube configurationCube)
    {
        List<CubeItemTuple> tuples = new List<CubeItemTuple>();

        foreach (var axis in axesQuery)
        {
            tuples.Add(ProcessAxisQuery(axis.Execute(configurationCube) as AxisItem,configurationCube));
        }

        return tuples;
    }


    private CubeItemTuple ProcessAxisQuery(AxisItem? axisQuery, ConfigurationCube configurationCube)
    {
        if (axisQuery is null) throw new ArgumentNullException(nameof(axisQuery));

        List<CubeItem> items = new List<CubeItem>();
        
        var set = MappingHelper.ToSet(axisQuery.Member);
        
        
        foreach (var tuple in set.Members)
        {
            ProcessQueryTuple(MappingHelper.ToTuple(tuple), configurationCube, ref items);
        }

        return new CubeItemTuple(items);
    }

    private void ProcessQueryTuple(TupleItem? tupleQuery, ConfigurationCube configurationCube, ref List<CubeItem> cubeItems)
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

    private CubeItem? ProcessMember(MemberItem? memberQuery, ConfigurationCube configurationCube)
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

    private CubeItem? FindMeasure(ConfigurationCube configurationCube, string name)
    {
        var measure = configurationCube.Measures.FirstOrDefault(x => x.Name == name);
        if (measure is null) return null;
        var cubeItem = new MeasureCubeItem("Показатель", string.Empty,null);
        cubeItem.Values.Add(measure.Clone());
        return cubeItem;
    }

    private CubeItem? FindDimension(ConfigurationCube configurationCube, string[] hierarchy)
    {
        int i = 0;
        
        CubeItem? result = null;
        CubeItem? temp = null;
        List<CubeItem>? dimensions = configurationCube.Dimensions.ToList();
        CubeItem? current = null;

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
}