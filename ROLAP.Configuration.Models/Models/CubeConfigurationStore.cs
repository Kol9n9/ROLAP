using System.Collections.Concurrent;
using ROLAP.Common.Model;
using ROLAP.Configuration.Models.Interfaces;

namespace ROLAP.Configuration.Models.Models;

public class CubeConfigurationStore : ICubeConfigurationStore
{
    private ConcurrentDictionary<string, ConfigurationCube> _store =
        new ConcurrentDictionary<string, ConfigurationCube>();

    private readonly ICubeConfigurationLoader _configurationLoader;

    public CubeConfigurationStore(ICubeConfigurationLoader loader)
    {
        _configurationLoader = loader;
    }
    
    public ConfigurationCube GetByName(string name)
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