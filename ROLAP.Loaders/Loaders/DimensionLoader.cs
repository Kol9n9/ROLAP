using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Loaders.Models.Containers;
using ROLAP.Loaders.Models.CubeItems;

namespace ROLAP.Loaders.Loaders;

internal class DimensionLoader : ILoader<IDimensionCubeItem>
{
    private readonly DimensionStaticHandler _staticHandler;
    public DimensionLoader()
    {
        _staticHandler = new DimensionStaticHandler(this);
    }
    
    public IEnumerable<IDimensionCubeItem> Load(IEnumerable<ILoadOptions> options)
    {

        List<IDimensionCubeItem> dimensions = new List<IDimensionCubeItem>();

        foreach (var option in options)
        {
            if (!TryAddValue(option, dimensions)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }

        return dimensions;
    }
    
    
    private bool TryAddValue(ILoadOptions options, List<IDimensionCubeItem> dimensions)
    {
        if (options is DimensionStaticOptions staticOptions)
        {
            dimensions.AddRange(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}