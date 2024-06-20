using ROLAP.Core.Models.Helpers;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Loaders.Models.Options;
using ROLAP.Models.Helpers;
using ROLAP.Models.Models.IContainers;
using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Loaders.Handlers;

internal class ValueStaticHandler : ILoaderHandler<ValueCubeItem,ValueStaticOptions>
{
    private readonly ILoader<DimensionContainer> _dimensionLoader;
    private readonly IContainer _measure;
    private readonly IEnumerable<ValueStaticOptions> _valueOptions;

    public ValueStaticHandler(IEnumerable<ValueStaticOptions> valueOptions, ILoader<DimensionContainer> dimensionLoader, MeasureCubeItem measureCubeItem)
    {
        _valueOptions = valueOptions;
        _dimensionLoader = dimensionLoader;
        _measure = new MeasureContainer();
        _measure.AddValue(measureCubeItem);
    }
    public IEnumerable<ValueCubeItem> Load(ValueStaticOptions options)
    {
        var dimensions = _dimensionLoader.Load(options.Dimensions).GetValues<DimensionContainer>();
        return FilterValues(dimensions);
    }

    private IEnumerable<ValueCubeItem> FilterValues(IEnumerable<DimensionContainer> dimensions)
    {
        var items = _valueOptions
            .Select(x => new ValueCubeItem(x.Id,x.Value,_measure,_dimensionLoader.Load(x.Dimensions).GetValues<DimensionContainer>()))
            .Where(x => CubeItemHelper.IsCubeItemInContainers(x, dimensions));
        return items;
    }
}