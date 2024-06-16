using ROLAP.Core.Models.Helpers;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Handlers;

internal class ValueStaticHandler : ILoaderHandler<ValueCubeItem,ValueStaticOptions>
{
    private readonly ILoader<DimensionCubeItem> _loader;
    private readonly MeasureCubeItem _measureCubeItem;
    private readonly IEnumerable<ValueStaticOptions> _valueOptions;

    public ValueStaticHandler(IEnumerable<ValueStaticOptions> valueOptions, ILoader<DimensionCubeItem> loader, MeasureCubeItem measureCubeItem)
    {
        _valueOptions = valueOptions;
        _loader = loader;
        _measureCubeItem = measureCubeItem;
    }
    public IEnumerable<ValueCubeItem> Load(ValueStaticOptions options)
    {
        var dimensions = _loader.Load(options.Dimensions);
        return FilterValues(dimensions);
    }

    private IEnumerable<ValueCubeItem> FilterValues(IEnumerable<DimensionCubeItem> dimensions)
    {
        var items = _valueOptions
            .Select(x => new ValueCubeItem(x.Id,x.Value,_measureCubeItem,_loader.Load(x.Dimensions)))
            .Where(x => CubeItemHelper.IsValueInDimensions(x, dimensions));
        return items;
    }
}