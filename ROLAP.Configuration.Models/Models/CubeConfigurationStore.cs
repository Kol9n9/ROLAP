using System.Collections.Concurrent;
using ROLAP.Configuration.Models.Interfaces;

namespace ROLAP.Configuration.Models.Models;

public class CubeConfigurationStore : ICubeConfigurationStore
{
    private ConcurrentDictionary<string, CubeConfiguration> _store =
        new ConcurrentDictionary<string, CubeConfiguration>();

    private readonly ICubeConfigurationLoader _configurationLoader;

    public CubeConfigurationStore(ICubeConfigurationLoader loader)
    {
        _configurationLoader = loader;
    }
    
    public CubeConfiguration GetByName(string name)
    {
        if (_store.TryGetValue(name, out var res))
        {
            return res;
        }

        var cubeConf = _configurationLoader.Load(name);
        _store.TryAdd(name, cubeConf);
        return cubeConf;
    }
}