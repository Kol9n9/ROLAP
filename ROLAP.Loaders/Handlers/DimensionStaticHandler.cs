using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.Containers;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Handlers;

internal class DimensionStaticHandler : ILoaderHandler<DimensionContainer,DimensionStaticOptions>
{
    private readonly ILoader<DimensionContainer> _loader;
    public DimensionStaticHandler(ILoader<DimensionContainer> loader)
    {
        _loader = loader;
    }
    public IEnumerable<DimensionContainer> Load(DimensionStaticOptions options)
    {
        DimensionContainer container = new DimensionContainer(options.Name);
        
        if (options.Values.Any())
        {
            container.AddValue(_loader.Load(options.Values).GetValues());
        }
        //DimensionCubeItem cubeItem = new DimensionCubeItem(options.Key, options.Name, values);
        return new List<DimensionContainer> { container };
    }
}