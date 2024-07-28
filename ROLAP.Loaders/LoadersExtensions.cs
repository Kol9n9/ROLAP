using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Loaders;
using ROLAP.Loaders.Models.CubeItems;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders;

public static class LoadersExtensions
{
    private static ILoader<CubeConfiguration> _loader = new CubeLoader();
    
    public static ICubeConfiguration LoadCubeConfiguration(string name)
    {
        var cube = _loader.Load(new List<ILoadOptions> { new CubeStaticOptions(name) }).GetValues<CubeConfiguration>().FirstOrDefault();
        if (cube is null) throw new Exception("Конфигурация куба не найдена");
        return cube;
    }
}