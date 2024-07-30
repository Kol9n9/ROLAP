using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Loaders.Models.Containers;
using ROLAP.Loaders.Models.CubeItems;

namespace ROLAP.Loaders.Loaders;

internal class CubeLoader : ILoader<CubeConfiguration>
{
    private readonly ILoaderHandler<CubeConfiguration, CubeStaticOptions> _staticHandler;

    public CubeLoader()
    {
        _staticHandler = new CubeStaticHandler();
    }
    
    public IEnumerable<CubeConfiguration> Load(IEnumerable<ILoadOptions> options)
    {
        List<CubeConfiguration> configurations = new List<CubeConfiguration>();
        
        foreach (var option in options)
        {
            if (!TryAddValue(option, configurations)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }

        return configurations;
    }
    private bool TryAddValue(ILoadOptions options,  List<CubeConfiguration> configurations)
    {
        if (options is CubeStaticOptions staticOptions)
        {
            configurations.AddRange(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}