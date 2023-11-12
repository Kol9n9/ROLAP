namespace ROLAP.CubeConfiguration.Models.Dimension;

internal class StaticDimensionConfiguration : IDimensionConfiguration
{
    /// <summary>
    /// Идентификатор группы измерений
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Наименование группы измерений
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Ключ
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Значения измерения
    /// </summary>
    public List<IDimensionValueConfiguration> Values { get; set; }
}