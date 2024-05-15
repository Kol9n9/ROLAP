namespace ROLAP.Common.Model;

/// <summary>
/// Измерение в кубе
/// </summary>
public class Dimension : ICubeItem
{
    /// <summary>
    /// Название группы, если многоуровневое измерение
    /// </summary>
    public string? GroupName { get; private set; }

    /// <summary>
    /// Значения измерения
    /// </summary>
    public List<Dimension> Values { get; set; } = new List<Dimension>();

    public Dimension(string key, string name, string? groupName) : base(key,name)
    {
        GroupName = groupName;
    }
    
    /// <summary>
    /// Добавить измерение в качестве значения
    /// </summary>
    /// <param name="dimension">Добавляемое <see cref="Dimension">измерение</see></param>
    public void AddDimensionValue(Dimension dimension) => Values.Add(dimension);

    public override bool Equals(object? obj)
    {
        if (obj is Dimension dimension)
        {
            if (dimension.GroupName != GroupName) return false;
            if (dimension.Values.Count != Values.Count) return false;
            for (int i = 0; i < dimension.Values.Count; i++)
            {
                if (!dimension.Values[i].Equals(Values[i])) return false;
            }

            return true;
        }
        return false;
    }

    public override object Clone()
    {
        return new Dimension(Key, Name, GroupName)
        {
            Values = Values.Select(x => x.Clone()).Cast<Dimension>().ToList()
        };
    }
}