using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Models.CubeItems;

internal class MeasureCubeItem : IMeasureCubeItem
{
    public string Key { get; }
    public string Name { get; }

    private ILoader<IValueCubeItem> _loader = null!;

    public MeasureCubeItem(string key, string name)
    {
        Key = key;
        Name = name;
    }

    public ILoader<IValueCubeItem> GetLoader() => _loader;
    public void SetLoader(ILoader<IValueCubeItem> loader) => _loader = loader;

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