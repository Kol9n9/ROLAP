using ROLAP.Common.Interfaces;
using ROLAP.Common.Model.Query;

namespace ROLAP.Common.Helpers;

public static class MappingHelper
{
    public static CubeSetQuery ToSet(ICubeQueryItem item)
    {
        if (item is CubeSetQuery set) return set;
        if (item is CubeTupleQuery tuple) return new CubeSetQuery(new List<CubeTupleQuery> { tuple });
        if(item is CubeMemberQuery member) return new CubeSetQuery(new List<CubeTupleQuery> { new CubeTupleQuery(new List<CubeMemberQuery>{ member})});
        throw new InvalidCastException("Неивестный тип");
    }

    public static CubeTupleQuery ToTuple(ICubeQueryItem item)
    {
        if (item is CubeSetQuery) throw new InvalidCastException("Невозможно смапить Set в Tuple");
        if (item is CubeTupleQuery tuple) return tuple;
        if(item is CubeMemberQuery member) return new CubeTupleQuery(new List<CubeMemberQuery>{ member});
        throw new InvalidCastException("Неивестный тип");
    }
}