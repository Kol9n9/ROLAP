using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.CubeItem;

public class MeasureGroupCubeItem : ICubeItem
{
    public string Name { get; }
    public IEnumerable<MeasureCubeItem> Values { get; private set; }

    public MeasureGroupCubeItem(string name, IEnumerable<MeasureCubeItem> values)
    {
        Name = name;
        Values = values;
    }
    
    public CubeItemType GetItemType()
    {
        throw new NotImplementedException();
    }

    public string GetKey() => throw new NotSupportedException();

    public string GetName() => Name;
    
    public ICubeItem Clone(bool withInnerValues = true)
    {
        throw new NotImplementedException();
    }

    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        var clone = Clone(withInnerValues);
        return (T)clone;
    }

    public void AddValue(ICubeItem item)
    {
        Values = Values.Append((MeasureCubeItem)item);
    }

    public IEnumerable<ICubeItem> GetValues() => Values;

    public bool NameEqual(string name)
    {
        return Name.Equals(name);
    }
}