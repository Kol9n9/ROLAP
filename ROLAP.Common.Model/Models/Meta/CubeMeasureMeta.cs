namespace ROLAP.Common.Model.Models.Meta;

public class CubeMeasureMeta : ICubeMeta
{
    /// <summary>
    /// Идентификатор меры
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Ключ
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Наименование меры
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Связь со значениями
    /// </summary>
    public CubeMeasureValueConnectionMeta ValueConnection { get; set; }

    public object Clone()
    {
        return new CubeMeasureMeta
        {
            Id = this.Id,
            Key = this.Key,
            Name = this.Name,
            ValueConnection = new CubeMeasureValueConnectionMeta
            {
                SchemaName = this.ValueConnection.SchemaName,
                TableName = this.ValueConnection.TableName,
                ValueConnectionField = this.ValueConnection.ValueConnectionField,
                ValueField = this.ValueConnection.ValueField,
            }
        };
    }
}