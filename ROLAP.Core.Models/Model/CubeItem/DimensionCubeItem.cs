using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.CubeItem;

public class DimensionCubeItem : ICubeItem
{
    public string Key { get; }
    public string Name { get; }
    public string? GroupName { get; }
    public IEnumerable<DimensionCubeItem> Values { get; private set; }
    
    public DimensionCubeItem(string key, string name, IEnumerable<DimensionCubeItem> values, string? groupName = null)
    {
        Key = key;
        Name = name;
        Values = values;
        GroupName = groupName;
    }
    
    public CubeItemType GetItemType() => CubeItemType.Dimension;
    
    public string GetKey() => Key;

    public string GetName() => Name;

    public ICubeItem Clone(bool withInnerValues = true)
    {
        var values = withInnerValues ? Values.Select(x => x.Clone()).Cast<DimensionCubeItem>() : new List<DimensionCubeItem>();
        return new DimensionCubeItem(Key, Name, values, GroupName);
    }

    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        var clone = Clone(withInnerValues);
        return (T)clone;
    }

    public void AddValue(ICubeItem item)
    {
        Values = Values.Append((DimensionCubeItem)item);
    }

    public IEnumerable<ICubeItem> GetValues() => Values;

    public bool NameEqual(string name)
    {
        return Name.Equals(name) || Key.Equals(name);
    }
}