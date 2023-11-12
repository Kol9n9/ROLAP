namespace ROLAP.Common.Model.Models.Meta;

public class CubeMeasureValueConnectionMeta
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
    /// Поле для связи со значениями
    /// </summary>
    public string ValueConnectionField { get; set; }
    /// <summary>
    /// Поле значения
    /// </summary>
    public string ValueField { get; set; }
}