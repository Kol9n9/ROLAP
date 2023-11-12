namespace ROLAP.CubeConfiguration.Models.Measure;

internal class StaticMeasureConfiguration : IMeasureConfiguration
{
    /// <summary>
    /// Идентификатор меры
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Наименование меры
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Ключ
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Связь со значениями
    /// </summary>
    public MeasureValueConnection ValueConnection { get; set; }
}