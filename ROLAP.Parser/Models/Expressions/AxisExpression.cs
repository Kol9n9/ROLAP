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

using ROLAP.Common.Model.Models;

namespace ROLAP.Parser.Models.Expressions;

internal class AxisExpression : IExpression
{
    private IExpression _expression;
    private int _axisNumber;

    public AxisExpression(int axisNumber, IExpression expression)
    {
        _axisNumber = axisNumber;
        _expression = expression;
    }
    public ICubeQueryNode Eval()
    {
        return new CubeQueryAxis()
        {
            AxisNumber = _axisNumber,
            Set = Helpers.MapToSet(_expression.Eval())
        };
    }
}