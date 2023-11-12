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
using ROLAP.Common.Model.Models.Meta;

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
    public ICubeQueryNode Eval(List<ICubeMeta> cubeMeta)
    {
        var set = Helpers.MapToSet(_expression.Eval(cubeMeta));
        if (set == null) set = new CubeQuerySet();
        return new CubeQueryAxis()
        {
            AxisNumber = _axisNumber,
            Set = set
        };
    }
}