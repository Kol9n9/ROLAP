// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: SelectStatement.cs
// *  Автор: *(slezenko)
// *  Дата создания: 11.09.2023
// *  Назначение: Определение класса SelectStatement.cs
// ****************************************************************************/
// #endregion Copyright

using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.Expressions;

namespace ROLAP.Parser.Models.Statements;

public class SelectStatement : IStatement
{
    private List<IExpression> _expressions;
    private string _cubeName;

    public SelectStatement(string cubeName, List<IExpression> expressions)
    {
        _cubeName = cubeName;
        _expressions = expressions;
    }
    public void Execute()
    {
        List<CubeRequestAxis> axes = new List<CubeRequestAxis>();
        foreach (var expression in _expressions)
        {
            axes.Add((CubeRequestAxis)expression.Eval());
        }
    }
}