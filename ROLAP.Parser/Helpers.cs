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

using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.ExpressionValues;

namespace ROLAP.Parser;

public static class Helpers
{
    public static CubeRequestAxisSet MapToSet(IExpressionValue value)
    {
        if (value is CubeRequestAxisMember member)
        {
            return
                new CubeRequestAxisSet(
                    new List<CubeRequestAxisTuple>()
                    {
                        new CubeRequestAxisTuple(new List<CubeRequestAxisMember>()
                        {
                            {
                                member
                            }
                        })
                    });
        }
        if (value is CubeRequestAxisTuple tuple)
        {
            return
                new CubeRequestAxisSet(
                    new List<CubeRequestAxisTuple>()
                    {
                        tuple
                    });
        }
        if (value is CubeRequestAxisSet set)
        {
            return set;
        }
        return null;
    }
    public static CubeRequestAxisTuple MapToTuple(IExpressionValue value)
    {
        if (value is CubeRequestAxisMember member)
        {
            return new CubeRequestAxisTuple(new List<CubeRequestAxisMember>()
            {
                {
                    member
                }
            });
        }
        if (value is CubeRequestAxisTuple tuple)
        {
            return tuple;
        }
        if (value is CubeRequestAxisSet set)
        {
            throw new Exception("Unable cast Set to Tuple");
        }
        return null;
    }
}