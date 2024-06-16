using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Handlers;

internal class DimensionStaticHandler : ILoaderHandler<DimensionCubeItem,DimensionStaticOptions>
{
    private readonly ILoader<DimensionCubeItem> _loader;
    public DimensionStaticHandler(ILoader<DimensionCubeItem> loader)
    {
        _loader = loader;
    }
    public IEnumerable<DimensionCubeItem> Load(DimensionStaticOptions options)
    {
        List<DimensionCubeItem> values = new List<DimensionCubeItem>();
        if (options.Values.Any())
        {
            values.AddRange(_loader.Load(options.Values));
        }
        DimensionCubeItem cubeItem = new DimensionCubeItem(options.Key, options.Name, values);
        return new List<DimensionCubeItem> { cubeItem };
    }
}