﻿// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: TupleExpression.cs
// *  Автор: *(slezenko)
// *  Дата создания: 11.09.2023
// *  Назначение: Определение класса TupleExpression.cs
// ****************************************************************************/
// #endregion Copyright

using ROLAP.Common.Model.Models;

namespace ROLAP.Parser.Models.Expressions;

internal class TupleExpression : IExpression
{
    private List<IExpression> _expressions;

    public TupleExpression(List<IExpression> expressions)
    {
        _expressions = expressions;
    }
    public ICubeQueryNode Eval()
    {
        List<CubeQueryMember> values = new List<CubeQueryMember>();
        foreach (var expression in _expressions)
        {
            values.Add((CubeQueryMember)expression.Eval());
        }
        return new CubeQueryTuple()
        {
            Members = values
        };
    }
}