using System.Text;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Core.Models.Model.Query;
using ROLAP.Formatter.Models;

namespace ROLAP.Formatter.Helpers;

internal static class MappingHelper
{
    public static SetResult Map(CubeItemSet set)
    {
        List<TupleResult> tuples = new List<TupleResult>();
        foreach (var tuple in set.Tuples)    
        {
            tuples.Add(Map(tuple));
        }

        return new SetResult(tuples);
    }

    public static TupleResult Map(CubeItemTuple tuple)
    {
        List<MemberResult> memberResults = new List<MemberResult>();
        foreach (var member in tuple.Members)
        {
            if(member is IDimensionContainer dimensionContainer) memberResults.AddRange(Map(dimensionContainer));
            if(member is IMeasureContainer measureContainer) memberResults.AddRange(Map(measureContainer));
        }

        return new TupleResult(memberResults);
    }

    public static IEnumerable<MemberResult> Map(IDimensionContainer container, List<MemberResult> prevMembers = null)
    {
        List<MemberResult> res = new List<MemberResult>();
        var item = Map((container.GetItem() as IMemberCubeItem)!);

        if (prevMembers is null) prevMembers = new List<MemberResult>();
        else prevMembers = new List<MemberResult>(prevMembers);
        
        prevMembers.Add(item);

        var values = container.GetValues<IDimensionContainer>();
        if (values.Any())
        {
            foreach (var value in values)
            {
                 res.AddRange(Map(value, prevMembers));
            }
        }
        else
        {
            StringBuilder nameBuilder = new StringBuilder();
            StringBuilder keyBuilder = new StringBuilder();
            for (int i = 0; i < prevMembers.Count; i++)
            {
                nameBuilder.Append("[" + prevMembers[i].Name + "]");
                keyBuilder.Append("[" + prevMembers[i].Key + "]");
                if (i != prevMembers.Count - 1)
                {
                    nameBuilder.Append('.');
                    keyBuilder.Append('.');
                }
            }

            if (prevMembers.Count == 1)
            {
                nameBuilder.Append(".[All]");
                keyBuilder.Append(".[All]");
            }
            res.Add(new MemberResult(nameBuilder.ToString(),keyBuilder.ToString()));
        }

        return res;
    }
    public static IEnumerable<MemberResult> Map(IMeasureContainer container)
    {
        List<MemberResult> res = new List<MemberResult>();
        foreach (var value in container.GetValues<IMemberCubeItem>())
        {
            var mapItem = Map(value);
            res.Add(new MemberResult("[Индикатор].["+mapItem.Name+"]","[Индикатор].["+mapItem.Key+"]"));
        }

        if (!res.Any())
        {
            res.Add(new MemberResult("[Индикатор].[All]","[Индикатор].[All]"));
        }

        return res;
    }

    public static MemberResult Map(IMemberCubeItem memberCubeItem)
    {
        return new MemberResult(memberCubeItem.GetName(),memberCubeItem.GetKey());
    }
    public static ValueResult Map(IValueCubeItem valueCubeItem)
    {
        return new ValueResult(valueCubeItem.GetValue(), valueCubeItem.GetFormattedValue());
    }
}