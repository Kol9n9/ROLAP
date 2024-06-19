using ROLAP.Core.Models.Helpers;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.Containers;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Handlers;

internal class ValueStaticHandler : ILoaderHandler<ValueCubeItem,ValueStaticOptions>
{
    private readonly ILoader<DimensionContainer> _dimensionLoader;
    private readonly MeasureCubeItem _measureCubeItem;
    private readonly IEnumerable<ValueStaticOptions> _valueOptions;

    public ValueStaticHandler(IEnumerable<ValueStaticOptions> valueOptions, ILoader<DimensionContainer> dimensionLoader, MeasureCubeItem measureCubeItem)
    {
        _valueOptions = valueOptions;
        _dimensionLoader = dimensionLoader;
        _measureCubeItem = measureCubeItem;
    }
    public IEnumerable<ValueCubeItem> Load(ValueStaticOptions options)
    {
        var dimensions = _dimensionLoader.Load(options.Dimensions).GetValues<DimensionContainer>();
        return FilterValues(dimensions);
    }

    private IEnumerable<ValueCubeItem> FilterValues(IEnumerable<DimensionContainer> dimensions)
    {
        var items = _valueOptions
            .Select(x => new ValueCubeItem(x.Id,x.Value,_measureCubeItem,_dimensionLoader.Load(x.Dimensions).GetValues<DimensionContainer>()))
            .Where(x => CubeItemHelper.IsValueInContainers(x, dimensions));
        return items;
    }
}