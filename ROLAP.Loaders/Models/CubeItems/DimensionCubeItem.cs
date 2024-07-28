using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Models.CubeItems;

internal class DimensionCubeItem : IDimensionCubeItem
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