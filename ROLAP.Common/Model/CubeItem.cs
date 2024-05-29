using ROLAP.Common.Enums;

namespace ROLAP.Common.Model;

public class CubeItem
{
    public string Name { get; }
    public string? GroupName { get; }
    public CubeItemType Type { get; }
    public List<CubeItem> Values { get; } = new List<CubeItem>();

    public CubeItem(string name, CubeItemType type, string? groupName = null)
    {
        Name = name;
        Type = type;
        GroupName = groupName;
    }

    public CubeItem Clone(bool withValues = true)
    {
        List<CubeItem> values = withValues ? Values.Select(x => x.Clone()).ToList() : new List<CubeItem>();
        var cubeItem = new CubeItem(Name, Type, GroupName);
        cubeItem.Values.AddRange(values);
        return cubeItem;
    }
    
}