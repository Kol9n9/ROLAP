// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: AxesExpression.cs
// *  Автор: *(slezenko)
// *  Дата создания: 11.09.2023
// *  Назначение: Определение класса AxesExpression.cs
// ****************************************************************************/
// #endregion Copyright

using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.ExpressionValues;

namespace ROLAP.Parser.Models.Expressions;

public class AxisExpression : IExpression
{
    private IExpression _expression;
    private int _axisNumber;

    public AxisExpression(int axisNumber, IExpression expression)
    {
        _axisNumber = axisNumber;
        _expression = expression;
    }
    public IExpressionValue Eval()
    {
        return new CubeRequestAxis(_axisNumber, Helpers.MapToSet(_expression.Eval()));
    }
}