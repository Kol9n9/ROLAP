using ROLAP.Configuration.Interfaces;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Loaders;

namespace ROLAP.Configuration.Models;

internal class ConfigurationStore : IConfigurationStore
{
    private Dictionary<string, ICubeConfiguration> _store = new Dictionary<string, ICubeConfiguration>();
    public ICubeConfiguration GetConfiguration(string cubeName)
    {
        if (_store.TryGetValue(cubeName, out var cube)) return cube;
        cube = LoadersExtensions.LoadCubeConfiguration(cubeName);
        _store.Add(cubeName,cube);
        return cube;
    }
}