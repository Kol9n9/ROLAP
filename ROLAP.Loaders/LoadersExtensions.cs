using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Loaders;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders;

public static class LoadersExtensions
{
    private static ILoader<CubeConfiguration> _loader = new CubeLoader();
    
    public static CubeConfiguration LoadCubeConfiguration(string name)
    {
        var cube = _loader.Load(new List<ILoadOptions> { new CubeStaticOptions(name) }).GetValues().FirstOrDefault();
        if (cube is null) throw new Exception("Конфигурация куба не найдена");
        return cube as CubeConfiguration;
    }
}