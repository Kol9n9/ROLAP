using ROLAP.Configuration.Interfaces;
using ROLAP.Loaders;
using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Configuration.Models;

internal class ConfigurationStore : IConfigurationStore
{
    private Dictionary<string, CubeConfiguration> _store = new Dictionary<string, CubeConfiguration>();
    public CubeConfiguration GetConfiguration(string cubeName)
    {
        if (_store.TryGetValue(cubeName, out var cube)) return cube;
        cube = LoadersExtensions.LoadCubeConfiguration(cubeName);
        _store.Add(cubeName,cube);
        return cube;
    }
}