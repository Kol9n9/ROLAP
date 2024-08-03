using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Models.CubeItems;

internal class MeasureCubeItem : IMeasureCubeItem
{
    private readonly string _key;
    private readonly string _name;

    private ILoader<IValueCubeItem> _loader = null!;

    private Func<IEnumerable<IValueCubeItem>, IValueCubeItem> _aggregateFunction;
    private Type _valueType;

    private bool _isTotal = false;

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

    public ICubeItem Clone(bool withValues)
    {
        var item = new MeasureCubeItem(_key, _name, _valueType, _aggregateFunction);
        item.SetLoader(_loader);
        item._isTotal = _isTotal;
        return item;
    }

    public ICubeItem GetTotalItem()
    {
        var totalMeasure = new MeasureCubeItem("", "Индикаторы", _valueType,_aggregateFunction);
        totalMeasure._isTotal = true;
        return totalMeasure;
    }

    public bool IsTotal() => _isTotal;

    public override bool Equals(object? obj)
    {
        if (obj is not MeasureCubeItem measureCubeItem) return false;
        return measureCubeItem.IsTotal() || _key == measureCubeItem.GetKey() && (string.IsNullOrWhiteSpace(_name) || string.IsNullOrWhiteSpace(measureCubeItem.GetName()) || _name == measureCubeItem.GetName());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_key, _name, "Measure");
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