using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Loaders.Models.Containers;
using ROLAP.Loaders.Models.CubeItems;

namespace ROLAP.Loaders.Loaders;

internal class DimensionLoader : ILoader<DimensionContainer>
{
    private readonly DimensionStaticHandler _staticHandler;
    public DimensionLoader()
    {
        _staticHandler = new DimensionStaticHandler(this);
    }
    
    public IContainer Load(IEnumerable<ILoadOptions> options)
    {

        DimensionContainer container = new DimensionContainer(new DimensionCubeItem("-1","Измерения"));
        
        foreach (var option in options)
        {
            if (!TryAddValue(option, container)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }

        return container;
    }
    
    
    private bool TryAddValue(ILoadOptions options, DimensionContainer container)
    {
        if (options is DimensionStaticOptions staticOptions)
        {
            container.AddValue(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}