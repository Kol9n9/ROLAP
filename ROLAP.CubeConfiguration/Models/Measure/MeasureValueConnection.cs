namespace ROLAP.CubeConfiguration.Models.Measure;

/// <summary>
/// Модель связи между мерой и значениями
/// </summary>
internal class MeasureValueConnection
{
    /// <summary>
    /// Название схемы
    /// </summary>
    public string SchemaName { get; set; }
    /// <summary>
    /// Назвние таблицы
    /// </summary>
    public string TableName { get; set; }
    /// <summary>
    /// Название поля для связи со значениями
    /// </summary> 
    public string ValueConnectionName { get; set; }
    /// <summary>
    /// Название поля со значенияем
    /// </summary>
    public string ValueFieldName { get; set; }
}