namespace ROLAP.Common.Model.Models.Meta;

public class CubeDimensionMeta : ICubeMeta
{
    /// <summary>
    /// Идентификатор измерения
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Ключ
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Наименование измерения
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Значения измерения
    /// </summary>
    public List<CubeDimensionValueMeta> Values { get; set; }

    public object Clone()
    {
        return new CubeDimensionMeta
        {
            Id = this.Id,
            Key = this.Key,
            Name = this.Name,
            Values = Values.Select(x => x.Clone()).Cast<CubeDimensionValueMeta>().ToList()
        };
    }
}