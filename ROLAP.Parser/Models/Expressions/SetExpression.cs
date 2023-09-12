// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: SetExpression.cs
// *  Автор: *(slezenko)
// *  Дата создания: 11.09.2023
// *  Назначение: Определение класса SetExpression.cs
// ****************************************************************************/
// #endregion Copyright

using ROLAP.Common.Model.Models;

namespace ROLAP.Parser.Models.Expressions;

internal class SetExpression : IExpression
{
    private List<IExpression> _expressions;

    public SetExpression(List<IExpression> expressions)
    {
        _expressions = expressions;
    }
    public ICubeQueryNode Eval()
    {
        List<CubeQueryTuple> values = new List<CubeQueryTuple>();
        foreach (var expression in _expressions)
        {
            values.Add(Helpers.MapToTuple(expression.Eval()));
        }
        return new CubeQuerySet()
        {
            Tuples = values
        };
    }
}