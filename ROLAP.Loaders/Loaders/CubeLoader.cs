using ROLAP.Core.Models.Interfaces;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;
using ROLAP.Models.Models.IContainers;
using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Loaders.Loaders;

internal class CubeLoader : ILoader<CubeConfiguration>
{
    private readonly ILoaderHandler<CubeConfiguration, CubeStaticOptions> _staticHandler;

    public CubeLoader()
    {
        _staticHandler = new CubeStaticHandler();
    }
    
    public IContainer Load(IEnumerable<ILoadOptions> options)
    {
        CubeConfigurationContainer container = new CubeConfigurationContainer();
        
        foreach (var option in options)
        {
            if (!TryAddValue(option, container)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }

        return container;
    }
    private bool TryAddValue(ILoadOptions options, CubeConfigurationContainer container)
    {
        if (options is CubeStaticOptions staticOptions)
        {
            container.AddValue(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}