using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.CubeItem;

public class MeasureCubeItem : ICubeItem
{
    public string Key { get; }
    public string Name { get; }

    private ILoader<ValueCubeItem> _loader;

    public MeasureCubeItem(string key, string name)
    {
        Key = key;
        Name = name;
    }

    public CubeItemType GetItemType() => CubeItemType.Measure;
    
    public string GetKey() => Key;

    public string GetName() => Name;

    public ILoader<ValueCubeItem> GetLoader() => _loader;
    public void SetLoader(ILoader<ValueCubeItem> loader) => _loader = loader;

    public ICubeItem Clone(bool withInnerValues = true)
    {
        var item = new MeasureCubeItem(Key,Name);
        item.SetLoader(_loader);
        return item;
    }

    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        var clone = Clone(withInnerValues);
        return (T)clone;
    }

    public void AddValue(ICubeItem item)
    {
        throw new NotSupportedException();
    }

    public bool IsContainer() => false;

    public IEnumerable<ICubeItem> GetValues()
    {
        throw new NotSupportedException();
    }

    public bool NameEqual(string name)
    {
        return Name.Equals(name) || Key.Equals(name);
    }
}