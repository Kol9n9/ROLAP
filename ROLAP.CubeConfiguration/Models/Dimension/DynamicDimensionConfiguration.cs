namespace ROLAP.CubeConfiguration.Models.Dimension;

internal class DynamicDimensionConfiguration : IDimensionConfiguration
{
    /// <summary>
    /// Название схемы
    /// </summary>
    public string SchemaName { get; set; }
    /// <summary>
    /// Название таблицы
    /// </summary>
    public string TableName { get; set; }
    /// <summary>
    /// Поле с идентификатором
    /// </summary>
    public string IdField { get; set; }
    /// <summary>
    /// Поле с наименованием
    /// </summary>
    public string NameField { get; set; }
    /// <summary>
    /// Поле с ключом
    /// </summary>
    public string KeyField { get; set; }
    /// <summary>
    /// Зачения измерения
    /// </summary>
    public List<IDimensionValueConfiguration> Values { get; set; }
}