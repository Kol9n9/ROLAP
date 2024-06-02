using ROLAP.QueryProcessor.Interfaces;
using ROLAP.QueryProcessor.Models.Items;

namespace ROLAP.QueryProcessor.Helpers;

internal static class MappingHelper
{
    public static SetItem ToSet(IQueryItem item)
    {
        if (item is SetItem set) return set;
        if (item is TupleItem tuple) return new SetItem(new List<TupleItem> { tuple });
        if(item is MemberItem member) return new SetItem(new List<TupleItem> { new TupleItem(new List<MemberItem>{ member})});
        throw new InvalidCastException("Неивестный тип");
    }

    public static TupleItem ToTuple(IQueryItem item)
    {
        if (item is SetItem) throw new InvalidCastException("Невозможно смапить Set в Tuple");
        if (item is TupleItem tuple) return tuple;
        if(item is MemberItem member) return new TupleItem(new List<MemberItem>{ member});
        throw new InvalidCastException("Неивестный тип");
    }
}