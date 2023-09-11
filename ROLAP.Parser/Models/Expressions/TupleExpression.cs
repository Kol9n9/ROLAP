// #region Copyright
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

using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.ExpressionValues;

namespace ROLAP.Parser.Models.Expressions;

public class TupleExpression : IExpression
{
    private List<IExpression> _expressions;

    public TupleExpression(List<IExpression> expressions)
    {
        _expressions = expressions;
    }
    public IExpressionValue Eval()
    {
        List<CubeRequestAxisMember> values = new List<CubeRequestAxisMember>();
        foreach (var expression in _expressions)
        {
            values.Add((CubeRequestAxisMember)expression.Eval());
        }
        return new CubeRequestAxisTuple(values);
    }
}