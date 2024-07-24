using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Models.Models.ICubeItems;

public class DimensionCubeItem : IMemberCubeItem
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
    
    public T Clone<T>(bool withValues) where T: ICubeItem
    {
        return (T)(ICubeItem)new DimensionCubeItem(Key, Name, GroupName);
    }

    public string GetName()
    {
        return Name;
    }

    public string GetKey()
    {
        return Key;
    }
}