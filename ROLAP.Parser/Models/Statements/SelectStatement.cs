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

using ROLAP.Common.Model.Interfaces;
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
    public CubeQuery Execute(IMappingCubeConfiguration mappingCubeConfiguration)
    {
        var cubeMeta = mappingCubeConfiguration.GetCubeMeta(_cubeName);
        List<CubeQueryAxis> axes = new List<CubeQueryAxis>();
        foreach (var expression in _expressions)
        {
            axes.Add((CubeQueryAxis)expression.Eval(cubeMeta));
        }
        DeleteExtraDimensions(axes, cubeMeta);
        return new CubeQuery
        {
            Axes = axes,
            CubeName = _cubeName,
            Type = CubeQueryType.Select
        };
    }
    private void DeleteExtraDimensions(List<CubeQueryAxis> axes, CubeMeta cubeMeta)
    {
        var measures = FindMeasures(axes, cubeMeta);
        var commonDimensions = GetCommonDimensions(measures, cubeMeta);
        DeleteExtraDimensions(axes,commonDimensions);
    }
    private List<CubeMetaItem> FindMeasures(List<CubeQueryAxis> axes, CubeMeta cubeMeta)
    {
        List<CubeMetaItem> measures = new List<CubeMetaItem>();

        foreach (var axis in axes)
        {
            foreach(var tuple in axis.Set.Tuples)
            {
                foreach(var member in tuple.Members)
                {
                    if(member.Type == CubeMemberType.Measure)
                    {
                        var cubeMetaMeasure = cubeMeta.Measures.FirstOrDefault(x => x.Measure.Name == member.Names[1] || x.Measure.Key == member.Names[1]);
                        if(cubeMetaMeasure != null)
                            measures.Add(cubeMetaMeasure.Measure);
                    }
                }
            }
        }
        return measures;
    }
    private List<string> GetCommonDimensions(List<CubeMetaItem> measures, CubeMeta cubeMeta)
    {
        List<string> commonDimensions = new List<string>();
        var allDimensions = measures.Select(x => x.Dimensions).ToList();
        if(allDimensions.Count > 0)
        {
            commonDimensions.AddRange(allDimensions[0]);
        }
        for(int i = 1; i < allDimensions.Count; i++)
        {
            var dim = allDimensions[i].Intersect(allDimensions[i - 1]);
            commonDimensions = commonDimensions.Intersect(dim).ToList();   
        }
        List<string> commonDimensionNames = new List<string>();
        foreach(var dim in commonDimensions)
        {
            var metaDimension = cubeMeta.Dimensions.FirstOrDefault(x => x.Dimension.Key == dim);
            if(metaDimension != null)
                commonDimensionNames.Add(metaDimension.Dimension.Name);
        }
        return commonDimensionNames;
    }
    private void DeleteExtraDimensions(List<CubeQueryAxis> axes, List<string> dimensions)
    {
        foreach (var axis in axes)
        {
            foreach (var tuple in axis.Set.Tuples.ToList())
            {
                foreach (var member in tuple.Members.ToList())
                {
                    if (member.Type == CubeMemberType.Dimension && !dimensions.Contains(member.Names[0]))
                    {
                        tuple.Members.Remove(member);
                    }
                }
                if(tuple.Members.Count == 0)
                {
                    axis.Set.Tuples.Remove(tuple);
                }
            }
        }
    }
}