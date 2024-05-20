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
    public void ExecuteQuery(CubeQuery query)
    {
        var cube = _store.GetByName(query.CubeName);
        CubeResult res = new CubeResult(ProcessAxes(query.Axes,cube));
        LoadValues(res.Axes.SelectMany(x => x.Tuples).ToList());
    }

    #region ProcessCubeQuery
    
    private IEnumerable<CubeResultSet> ProcessAxes(IEnumerable<CubeAxisQuery> axesQuery,Cube cube)
    {
        List<CubeResultSet> sets = new List<CubeResultSet>();

        foreach (var axis in axesQuery)
        {
            sets.Add(ProcessAxisQuery(axis.Execute(cube) as CubeAxisQuery,cube));
        }


        return sets;
    }


    private CubeResultSet ProcessAxisQuery(CubeAxisQuery? axisQuery, Cube cube)
    {
        if (axisQuery is null) throw new ArgumentNullException(nameof(axisQuery));

        List<CubeResultTuple> tuples = new List<CubeResultTuple>();
        
        var set = MappingHelper.ToSet(axisQuery.Member);
        
        foreach (var tuple in set.Members)
        {
            tuples.Add(ProcessQueryTuple(MappingHelper.ToTuple(tuple),cube));
        }
        
        return new CubeResultSet(tuples);
    }

    private CubeResultTuple ProcessQueryTuple(CubeTupleQuery? tupleQuery, Cube cube)
    {
        if (tupleQuery is null) throw new ArgumentNullException(nameof(tupleQuery));

        List<ICubeItem> cubeItems = new List<ICubeItem>();

        foreach (var member in tupleQuery.Items)
        {
            var cubeItem = ProcessMember(member as CubeMemberQuery, cube);
            if (cubeItem is not null)
            {
                cubeItems.Add(cubeItem);
            }
        }

        return new CubeResultTuple(cubeItems);
    }

    private ICubeItem? ProcessMember(CubeMemberQuery? memberQuery, Cube cube)
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

    private Measure? FindMeasure(Cube cube, string name)
    {
        var measure = cube.Measures.FirstOrDefault(x => x.Name == name);
        return measure;
    }

    private Dimension? FindDimension(Cube cube, string[] hierarchy)
    {
        int i = 0;

        Dimension? result = null;
        Dimension? res = null;
        Dimension? temp = null;
        IEnumerable<Dimension> dimensions = cube.Dimensions;
        do
        {
            if (!dimensions.Any()) return null;
            temp = dimensions.FirstOrDefault(x => x.Name == hierarchy[i]);
            if (temp is null) return null;

            if (res is null)
            {
                result = res = temp;
            }
            else
            {
                res.Values = new List<Dimension> { temp };
                res = temp;
            }

            dimensions = temp.Values;
            i++;

        } while (i < hierarchy.Length);

        return result;
    }
    
    #endregion

    #region LoadValues

    private IEnumerable<MeasureValue> LoadValues(List<CubeResultTuple> cubeTuples)
    {
        List<MeasureValue> values = new List<MeasureValue>();
        foreach (var cubeTuple in cubeTuples)
        {
            var measure = (Measure)cubeTuple.Members.FirstOrDefault(x => x is Measure)!;
            var dimensions = cubeTuple.Members.Where(x => x is Dimension).Cast<Dimension>();
            values.AddRange(measure.ValuesLoader.Load(dimensions));
        }

        return values;
    }

    #endregion
}