namespace ROLAP.CubeConfiguration.Models.Dimension;

internal class StaticDimensionValueConfiguration : IDimensionValueConfiguration
{
    /// <summary>
    /// Идентификатор измерения
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Название измерения
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Ключ
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Поле связи между значением и измерением
    /// </summary>
    public string ValueConnectionField { get; set; }
}