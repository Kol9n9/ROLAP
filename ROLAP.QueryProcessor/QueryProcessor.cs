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

        IEnumerable<IContainer> items = new List<IContainer>();
        
        var set = MappingHelper.ToSet(axisQuery.Member);
        
        
        foreach (var tuple in set.Members)
        {
           ProcessQueryTuple(MappingHelper.ToTuple(tuple), configurationCube, ref items);
        }

        return new CubeItemTuple(items);
    }

    private void ProcessQueryTuple(TupleItem? tupleQuery, CubeConfiguration configurationCube, ref IEnumerable<IContainer> containers)
    {
        if (tupleQuery is null) throw new ArgumentNullException(nameof(tupleQuery));
        foreach (var member in tupleQuery.Items)
        {
            var cubeItem = ProcessMember(member as MemberItem, configurationCube);
            if (cubeItem is not null)
            {
                containers = cubeItem.Merge(containers);
            }
        }
    }

    private IContainer? ProcessMember(MemberItem? memberQuery, CubeConfiguration configurationCube)
    {
        if (memberQuery is null) throw new ArgumentNullException(nameof(memberQuery));

        IContainer container = IsMeasure(memberQuery.Hierarchy[0])
            ? configurationCube.Measures
            : configurationCube.Dimensions;

        return container.FindByHierarchy(memberQuery.Hierarchy);
    }

    private bool IsMeasure(string name) => name.ToLower() == "measure";

    #endregion
}