using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Helpers;
using ROLAP.Loaders.Models.Containers;
using ROLAP.Loaders.Models.CubeItems;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Handlers;

internal class ValueStaticHandler : ILoaderHandler<IValueCubeItem,ValueStaticOptions>
{
    private readonly ILoader<DimensionContainer> _dimensionLoader;
    private readonly IContainer _measure;
    private readonly IEnumerable<ValueStaticOptions> _valueOptions;

    public ValueStaticHandler(IEnumerable<ValueStaticOptions> valueOptions, ILoader<DimensionContainer> dimensionLoader, IMeasureCubeItem measureCubeItem)
    {
        _valueOptions = valueOptions;
        _dimensionLoader = dimensionLoader;
        _measure = new MeasureContainer();
        _measure.AddValue(measureCubeItem);
    }
    public IEnumerable<IValueCubeItem> Load(ValueStaticOptions options)
    {
        var dimensions = _dimensionLoader.Load(options.Dimensions).GetValues<DimensionContainer>();
        return FilterValues(dimensions);
    }

    private IEnumerable<IValueCubeItem> FilterValues(IEnumerable<DimensionContainer> dimensions)
    {
        var items = _valueOptions
            .Select(x => new ValueCubeItem(x.Id,x.Value,_measure,_dimensionLoader.Load(x.Dimensions).GetValues<DimensionContainer>()))
            .Where(x => CubeItemHelper.IsCubeItemInContainers(x, dimensions));
        return items;
    }
}