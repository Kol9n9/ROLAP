using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Models.CubeItems;

internal class DimensionCubeItem : IDimensionCubeItem
{
    public string Key { get; }
    public string Name { get; }
    public string? GroupName { get; }

    public List<IDimensionCubeItem> Values { get; private set; } = new List<IDimensionCubeItem>();

    public DimensionCubeItem(string key, string name, string? groupName = null)
    {
        Key = key;
        Name = name;
        GroupName = groupName;
    }
    
    public ICubeItem Clone(bool withValues)
    {
        var clone = new DimensionCubeItem(Key, Name, GroupName);
        if (withValues) clone.Values = Values.Select(x => (IDimensionCubeItem)x.Clone(withValues)).ToList();
        return clone;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetKey()
    {
        return Key;
    }
    

    public IEnumerable<IDimensionCubeItem> GetDimensions()
    {
        return Values;
    }

    public void AddDimension(IDimensionCubeItem dimension)
    {
        Values.Add(dimension);
    }


    public override bool Equals(object? obj)
    {
        if (obj is not IDimensionCubeItem dimension) return false;
        return Key == dimension.GetKey() && (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(dimension.GetName()) || Name == dimension.GetName());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Name, "Dimension");
    }
}