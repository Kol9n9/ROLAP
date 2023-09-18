// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: Helpers.cs
// *  Автор: *(slezenko)
// *  Дата создания: 11.09.2023
// *  Назначение: Определение класса Helpers.cs
// ****************************************************************************/
// #endregion Copyright

using ROLAP.Common.Model.Models;

namespace ROLAP.Parser;

internal static class Helpers
{
    public static CubeQuerySet MapToSet(ICubeQueryNode value)
    {
        if (value is CubeQueryMember member)
        {
            return new CubeQuerySet()
            {
                Tuples = new List<CubeQueryTuple>()
                {
                    new CubeQueryTuple()
                    {
                        Members= new List<CubeQueryMember>()
                        {
                            member
                        }
                    }
                }
            };
        }
        if (value is CubeQueryTuple tuple)
        {
            return new CubeQuerySet()
            {
                Tuples = new List<CubeQueryTuple>()
                {
                    tuple
                }
            };
        }
        if (value is CubeQuerySet set)
        {
            return set;
        }
        return null;
    }
    public static CubeQueryTuple MapToTuple(ICubeQueryNode value)
    {
        if (value is CubeQueryMember member)
        {
            return new CubeQueryTuple()
            {
                Members = new List<CubeQueryMember>()
                {
                    member
                }
            };
        }
        if (value is CubeQueryTuple tuple)
        {
            return tuple;
        }
        if (value is CubeQuerySet set)
        {
            throw new Exception("Unable cast Set to Tuple");
        }
        return null;
    }
}