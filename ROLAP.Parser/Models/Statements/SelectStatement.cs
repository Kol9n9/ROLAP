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

using ROLAP.Common.Model.Models;
using ROLAP.Parser.Models.Expressions;

namespace ROLAP.Parser.Models.Statements;

internal class SelectStatement : IStatement
{
    private List<IExpression> _expressions;
    private string _cubeName;

    public SelectStatement(string cubeName, List<IExpression> expressions)
    {
        _cubeName = cubeName;
        _expressions = expressions;
    }
    public CubeQuery Execute()
    {
        List<CubeQueryAxis> axes = new List<CubeQueryAxis>();
        foreach (var expression in _expressions)
        {
            axes.Add((CubeQueryAxis)expression.Eval());
        }
        return new CubeQuery
        {
            Axes = axes,
            CubeName = _cubeName,
            Type = CubeQueryType.Select
        };
    }
}