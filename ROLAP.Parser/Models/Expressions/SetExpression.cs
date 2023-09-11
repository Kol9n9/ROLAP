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

using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.ExpressionValues;

namespace ROLAP.Parser.Models.Expressions;

public class SetExpression : IExpression
{
    private List<IExpression> _expressions;

    public SetExpression(List<IExpression> expressions)
    {
        _expressions = expressions;
    }
    public IExpressionValue Eval()
    {
        List<CubeRequestAxisTuple> values = new List<CubeRequestAxisTuple>();
        foreach (var expression in _expressions)
        {
            values.Add(Helpers.MapToTuple(expression.Eval()));
        }
        return new CubeRequestAxisSet(values);
    }
}