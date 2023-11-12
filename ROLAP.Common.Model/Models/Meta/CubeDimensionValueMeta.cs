namespace ROLAP.Common.Model.Models.Meta;

public class CubeDimensionValueMeta : ICloneable
{
    /// <summary>
    /// Идентификатор значения измерения
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Ключ
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Наименование значения измерения
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Поле связи между значением и значением измерения
    /// </summary>
    public string ValueConnectionField { get; set; }

    public object Clone()
    {
        return new CubeDimensionValueMeta
        {
            Id = this.Id,
            Key = this.Key,
            Name = this.Name,
            ValueConnectionField = this.ValueConnectionField
        };
    }
}