using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Models.Models.ICubeItems;

public class MeasureCubeItem : IMemberCubeItem
{
    public string Key { get; }
    public string Name { get; }

    private ILoader<ValueCubeItem> _loader;

    public MeasureCubeItem(string key, string name)
    {
        Key = key;
        Name = name;
    }

    public ILoader<ValueCubeItem> GetLoader() => _loader;
    public void SetLoader(ILoader<ValueCubeItem> loader) => _loader = loader;

    public T Clone<T>(bool withValues) where T : ICubeItem
    {
        var item = new MeasureCubeItem(Key,Name);
        item.SetLoader(_loader);
        return (T)(ICubeItem)item;
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