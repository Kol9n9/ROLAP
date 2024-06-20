using ROLAP.Core.Models.Interfaces;
using ROLAP.Loaders.Loaders;
using ROLAP.Loaders.Models.Options;
using ROLAP.Loaders.Utils.Parser;
using ROLAP.Models.Models.IContainers;
using ROLAP.Models.Models.ICubeItems;
using CubeConfiguration = ROLAP.Models.Models.ICubeItems.CubeConfiguration;
namespace ROLAP.Loaders.Handlers;

internal class CubeStaticHandler : ILoaderHandler<CubeConfiguration,CubeStaticOptions>
{
    private readonly ILoader<DimensionContainer> _dimensionLoader;
    private readonly ILoader<MeasureCubeItem> _measureLoader;

    public CubeStaticHandler()
    {
        _dimensionLoader = new DimensionLoader();
        _measureLoader = new MeasureLoader();
    }
    
    public IEnumerable<CubeConfiguration> Load(CubeStaticOptions options)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory());
        var text = File.ReadAllText(Path.Combine(path, options.Name+".txt"));
        
        var configuration = CubeConfigurationStaticParser.ParseCubeConfiguration(text);

        var dimensions = _dimensionLoader.Load(configuration.Dimensions);
        var measures = _measureLoader.Load(configuration.Measures);

        return new List<CubeConfiguration>
        {
            new CubeConfiguration(dimensions, measures)
        };
    }
}