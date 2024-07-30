using ROLAP.Configuration.Interfaces;
using ROLAP.Core.Models.Interfaces.Container;
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
        var tuples = ProcessAxes(queryModel.Axes, cubeConf);
        var whereTuples = ProcessAxes(queryModel.Where, cubeConf);
        // if (whereTuples.Any(x => x.Members.Any(y => y.Values.Count() > 1)))
        //     throw new Exception("В Where для измерений / мер можно указывать только одно значение");
        return new CubeQuery(queryModel.QueryType, tuples, whereTuples);
    }
    
    
    
    #region ProcessQueryModel
    
    private IEnumerable<CubeItemTuple> ProcessAxes(IEnumerable<AxisItem> axesQuery, ICubeConfiguration configurationCube)
    {
        List<CubeItemTuple> tuples = new List<CubeItemTuple>();

        foreach (var axis in axesQuery)
        {
            tuples.Add(ProcessAxisQuery(axis.Execute(configurationCube) as AxisItem,configurationCube));
        }

        return tuples;
    }


    private CubeItemTuple ProcessAxisQuery(AxisItem? axisQuery, ICubeConfiguration configurationCube)
    {
        if (axisQuery is null) throw new ArgumentNullException(nameof(axisQuery));

        List<CubeItemTuple> items = new List<CubeItemTuple>();
        
        var set = MappingHelper.ToSet(axisQuery.Member);
        
        
        foreach (var tuple in set.Members)
        {
           ProcessQueryTuple(MappingHelper.ToTuple(tuple), configurationCube, ref items);
        }

        throw new Exception();
        //return new CubeItemTuple(items);
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
                //containers = cubeItem.Merge(containers);
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
            find = cubeItem.FindByHierarchy(hierarchy);
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

        bool isMeasure = (tuple.Members.First()! as IMeasureCubeItem) != null;

        if (isMeasure)
        {
            var measure = tuples.FirstOrDefault(x => x.Members.OfType<IMeasureCubeItem>() != null);
            if (measure is null)
            {
                tuples.Add(tuple);
                return;
            }
            var find = FindByName(measure, tuple.Members.First(), out var _);
            if (find is null)
            {
                measure.AddMember(tuple.Members.First());
            }
            return;
        }

        foreach (var VARIABLE in tuples.Where(x => x.Members.OfType<IDimensionCubeItem>() != null))
        {
            
        }
    }

    private ICubeItem? FindByName(CubeItemTuple tuple, ICubeItem cubeItem, out ICubeItem prev)
    {
        prev = null;
        CubeItemTuple? find = null;

        foreach (var member in tuple.Members)
        {
            member.FindByHierarchy();
        }
    }
    
    private bool IsMeasure(string name) => name.ToLower() == "measure";
    

    #endregion
}