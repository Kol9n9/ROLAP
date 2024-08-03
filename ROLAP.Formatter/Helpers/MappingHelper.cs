using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Model.Query;
using ROLAP.Formatter.Models;

namespace ROLAP.Formatter.Helpers;

internal static class MappingHelper
{
    public static SetResult Map(CubeItemSet set)
    {
        List<TupleResult> results = new List<TupleResult>();
        foreach (var tuple in set.Tuples)
        {
            results.Add(Map(tuple));
        }

        return new SetResult(results);
    }

    public static TupleResult Map(CubeItemTuple tuple)
    {
        List<MemberResult> members = new List<MemberResult>();
        foreach (var member in tuple.Members)
        {
            members.Add(member is IDimensionCubeItem dimension ? Map(dimension) :
                member is IMeasureCubeItem measure ? Map(measure) : throw new Exception("Неожиданный тип"));
        }
        return new TupleResult(members);
    }

    public static MemberResult Map(IDimensionCubeItem dimension, IEnumerable<IDimensionCubeItem>? prevDimensions = null)
    {
        List<IDimensionCubeItem> items = new List<IDimensionCubeItem>();
        if(prevDimensions is not null) items.AddRange(prevDimensions);
        items.Add(dimension);
        
        var values = dimension.GetDimensions();
        if (values.Any())
        {
            return Map(values.First(), items);
        }
        string name = string.Join(".", items.Select(x => $"[{x.GetName()}]"));
        string key = string.Join(".", items.Select(x => $"[{x.GetKey()}]"));

        if (items.Count == 1)
        {
            name += ".[All]";
            key += ".[All]";
        }
        
        return new MemberResult(name, key);
    }

    public static MemberResult Map(IMeasureCubeItem measure)
    {
        if (measure.IsTotal())
        {
            return new MemberResult("[Индикаторы].[All]", "[Индикаторы].[All]");
        }
        return new MemberResult($"[Индикаторы].[{measure.GetName()}]",$"[Индикаторы].[{measure.GetKey()}]");
    }
    
    public static ValueResult Map(IValueCubeItem value)
    {
        return new ValueResult(value.GetValue(), value.GetFormattedValue());
    }
}