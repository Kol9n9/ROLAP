using ROLAP.Core.Models.Interfaces;
using ROLAP.Loaders.Models.Options;
using ROLAP.Models.Models.IContainers;
using ROLAP.Models.Models.ICubeItems;

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
        DimensionContainer container = new DimensionContainer(new DimensionCubeItem(options.Key, options.Name));
        
        if (options.Values.Any())
        {
            container.AddValue(_loader.Load(options.Values).GetValues<DimensionContainer>());
        }
        
        return new List<DimensionContainer> { container };
    }
}