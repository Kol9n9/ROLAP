using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.CubeItem;

public class DimensionCubeItem : ICubeItem
{
    public string Key { get; }
    public string Name { get; }
    public string? GroupName { get; }
    
    public DimensionCubeItem(string key, string name, string? groupName = null)
    {
        Key = key;
        Name = name;
        GroupName = groupName;
    }
    
    public ICubeItem Clone(bool withInnerValues = true)
    {
        return new DimensionCubeItem(Key, Name, GroupName);
    }

    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        var clone = Clone(withInnerValues);
        return (T)clone;
    }
}