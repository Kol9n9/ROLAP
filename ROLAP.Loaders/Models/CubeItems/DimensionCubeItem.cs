using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Models.CubeItems;

internal class DimensionCubeItem : IDimensionCubeItem
{
    public string Key { get; }
    public string Name { get; }
    public string? GroupName { get; }

    public List<IDimensionCubeItem> Values { get; } = new List<IDimensionCubeItem>();

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

    public ICubeItem? FindByHierarchy(string[] hierarchy)
    {
        if (Name != hierarchy[0]) return null;
        DimensionCubeItem clone = Clone<DimensionCubeItem>(false);
        DimensionCubeItem current = clone;

        hierarchy = hierarchy.Skip(1).ToArray();

        IEnumerable<IDimensionCubeItem> currentValues = Values.ToList();

        while (hierarchy.Any())
        {
            IDimensionCubeItem? find = null;

            foreach (var value in currentValues)
            {
                find = value.FindByHierarchy(hierarchy) as IDimensionCubeItem;
                if(find is not null) break;
            }

            if (find is null) return null;

            current.Values.Add(find.Clone<DimensionCubeItem>(false));
            current = current.Values.First() as DimensionCubeItem;

            if (current is null) return null;
            
            currentValues = current.GetDimensions();
            hierarchy = hierarchy.Skip(1).ToArray();
        }

        return clone;
    }

    public bool Contains(ICubeItem item)
    {
        throw new NotImplementedException();
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
}