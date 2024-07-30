using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Models.CubeItems;

internal class MeasureCubeItem : IMeasureCubeItem
{
    private string _key;
    private string _name;

    private ILoader<IValueCubeItem> _loader = null!;

    private Func<IEnumerable<IValueCubeItem>, IValueCubeItem> _aggregateFunction;
    private Type _valueType;

    public MeasureCubeItem(string key, string name, Type valueType, Func<IEnumerable<IValueCubeItem>, IValueCubeItem> aggregateFunction)
    {
        _key = key;
        _name = name;
        _valueType = valueType;
        _aggregateFunction = aggregateFunction;
    }

    public ILoader<IValueCubeItem> GetLoader() => _loader;
    public IValueCubeItem Aggregate(IEnumerable<IValueCubeItem> items)
    {
        return _aggregateFunction(items);
    }

    public Type GetValueType()
    {
        return _valueType;
    }

    public void SetLoader(ILoader<IValueCubeItem> loader) => _loader = loader;

    public T Clone<T>(bool withValues) where T : ICubeItem
    {
        var item = new MeasureCubeItem(_key, _name, _valueType, _aggregateFunction);
        item.SetLoader(_loader);
        return (T)(ICubeItem)item;
    }

    public ICubeItem? FindByHierarchy(string[] hierarchy)
    {
        if (_name == hierarchy[1]) return Clone<MeasureCubeItem>(false);
        return null;
    }

    public bool Contains(ICubeItem item)
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        return _name;
    }

    public string GetKey()
    {
        return _key;
    }
}