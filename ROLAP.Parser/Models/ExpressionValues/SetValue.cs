// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: SetValue.cs
// *  Автор: *(slezenko)
// *  Дата создания: 11.09.2023
// *  Назначение: Определение класса SetValue.cs
// ****************************************************************************/
// #endregion Copyright

using ROLAP.Parser.Models.CubeRequest;

namespace ROLAP.Parser.Models.ExpressionValues;

public class SetValue : IExpressionValue
{
    private CubeRequestAxisSet _value;
    public SetValue(CubeRequestAxisSet value)
    {
        _value = value;
    }
}