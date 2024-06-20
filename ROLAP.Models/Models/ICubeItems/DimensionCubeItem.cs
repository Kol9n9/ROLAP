using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Models.Models.ICubeItems;

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
    
    public T Clone<T>(bool withValues) where T: ICubeItem
    {
        if (typeof(T) != typeof(DimensionCubeItem) && typeof(T) != typeof(ICubeItem)) throw new InvalidCastException($"Получить копию можно только для типа \"{nameof(DimensionCubeItem)}\"");
        
        return (T)(ICubeItem)new DimensionCubeItem(Key, Name, GroupName);
    }
}