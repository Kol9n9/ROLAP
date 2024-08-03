using ROLAP.Core.Models.Helpers;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Core.Models.Model.TypedValues;
using ROLAP.Loaders.Models.CubeItems;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Handlers;

internal class ValueStaticHandler : ILoaderHandler<IValueCubeItem,ValueStaticOptions>
{
    private readonly ILoader<IDimensionCubeItem> _dimensionLoader;
    private readonly IMeasureCubeItem _measure;
    private readonly IEnumerable<ValueStaticOptions> _valueOptions;
    private readonly IMeasureCubeItem _measureCubeItem;
    
    public ValueStaticHandler(IEnumerable<ValueStaticOptions> valueOptions, ILoader<IDimensionCubeItem> dimensionLoader, IMeasureCubeItem measureCubeItem)
    {
        _valueOptions = valueOptions;
        _dimensionLoader = dimensionLoader;
        _measureCubeItem = measureCubeItem;
        _measure = measureCubeItem;
    }
    public IEnumerable<IValueCubeItem> Load(ValueStaticOptions options)
    {
        var dimensions = _dimensionLoader.Load(options.Dimensions);
        return FilterValues(dimensions,_measureCubeItem.GetValueType());
    }

    private IEnumerable<IValueCubeItem> FilterValues(IEnumerable<IDimensionCubeItem> dimensions,Type valueType)
    {
        List<IValueCubeItem> values = new List<IValueCubeItem>();

        foreach (var valueOption in _valueOptions)
        {
            var value = new ValueCubeItem(valueOption.Id, ValueTypeHelper.Create(valueType, valueOption.Value), _measure,
                _dimensionLoader.Load(valueOption.Dimensions));
            if(CubeItemHelper.IsValueInDimensions(value,dimensions)) values.Add(value);
        }
        
        return values;
    }
}