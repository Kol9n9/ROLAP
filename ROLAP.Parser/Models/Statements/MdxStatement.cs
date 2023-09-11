// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: MdxStatement.cs
// *  Автор: *(slezenko)
// *  Дата создания: 11.09.2023
// *  Назначение: Определение класса MdxStatement.cs
// ****************************************************************************/
// #endregion Copyright

using ROLAP.Parser.Models.Expressions;

namespace ROLAP.Parser.Models.Statements;

public class MdxStatement : IStatement
{
    private IExpression _expression;

    public MdxStatement(IExpression expression)
    {
        _expression = expression;
    }
    public void Execute()
    {
        _expression.Eval();
    }
}