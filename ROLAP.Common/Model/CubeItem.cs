using ROLAP.Common.Enums;

namespace ROLAP.Common.Model;

public class CubeItem
{
    public string Name { get; }
    public string Key { get; }
    public string? GroupName { get; }
    public CubeItemType Type { get; }
    public List<CubeItem> Values { get; } = new List<CubeItem>();

    public CubeItem(string name, string key, CubeItemType type, string? groupName = null)
    {
        Name = name;
        Key = key;
        Type = type;
        GroupName = groupName;
    }

    public virtual CubeItem Clone(bool withValues = true)
    {
        List<CubeItem> values = withValues ? Values.Select(x => x.Clone()).ToList() : new List<CubeItem>();
        var cubeItem = new CubeItem(Name, Key, Type, GroupName);
        cubeItem.Values.AddRange(values);
        return cubeItem;
    }
    
}