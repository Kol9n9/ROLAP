// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: CubeMeta.cs
// *  Автор: *(slezenko)
// *  Дата создания: 18.09.2023
// *  Назначение: Определение класса CubeMeta.cs
// ****************************************************************************/
// #endregion Copyright

namespace ROLAP.Common.Model.Models;

public class CubeMeta
{
    public List<CubeMetaMeasure> Measures { get; set; }
}

public class CubeMetaMeasure
{
    public CubeMetaItem Measure { get; set; }
    public List<CubeMetaDimension> Dimensions { get; set; }
}

public class CubeMetaDimension
{
    public CubeMetaItem Dimension { get; set; }
    public List<CubeMetaItem> Values { get; set; }
}

public class CubeMetaItem
{
    public string Name { get; set; }
    public string Key { get; set; }
    public string Schema { get; set; }
    public string Table { get; set; }
    public string ConnectionField { get; set; }
    public string ValueField { get; set; }
}