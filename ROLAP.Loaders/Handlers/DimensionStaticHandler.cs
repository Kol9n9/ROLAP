using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Models.CubeItems;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Handlers;

internal class DimensionStaticHandler : ILoaderHandler<IDimensionCubeItem,DimensionStaticOptions>
{
    private readonly ILoader<IDimensionCubeItem> _loader;
    public DimensionStaticHandler(ILoader<IDimensionCubeItem> loader)
    {
        _loader = loader;
    }
    public IEnumerable<IDimensionCubeItem> Load(DimensionStaticOptions options)
    {
        DimensionCubeItem dimensions = new DimensionCubeItem(options.Key, options.Name);
        
        if (options.Values.Any())
        {
            dimensions.Values.AddRange(_loader.Load(options.Values));
        }
        
        return new List<IDimensionCubeItem> { dimensions };
    }
}