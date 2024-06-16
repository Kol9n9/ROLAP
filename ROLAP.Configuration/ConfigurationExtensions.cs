using ROLAP.Configuration.Interfaces;
using ROLAP.Configuration.Models;

namespace ROLAP.Configuration;

public static class ConfigurationExtensions
{
    private static IConfigurationStore _store;
    public static IConfigurationStore GetConfigurationStore()
    {
        return _store ??= new ConfigurationStore();
    }
}