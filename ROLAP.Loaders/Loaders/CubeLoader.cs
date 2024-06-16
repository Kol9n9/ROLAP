using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;

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
        List<CubeConfiguration> cubes = new List<CubeConfiguration>();
        foreach (var option in options)
        {
            if (!TryGetValue(option, out var values)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
            cubes.AddRange(values);
        }

        return cubes;
    }
    private bool TryGetValue(ILoadOptions options, out IEnumerable<CubeConfiguration> values)
    {
        if (options is CubeStaticOptions staticOptions)
        {
            values = _staticHandler.Load(staticOptions);
            return true;
        }

        values = new List<CubeConfiguration>();
        return false;
    }
}