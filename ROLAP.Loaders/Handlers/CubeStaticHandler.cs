using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.Containers;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Loaders;
using ROLAP.Loaders.Models.Options;
using ROLAP.Loaders.Utils.Parser;
using CubeConfiguration = ROLAP.Core.Models.Model.CubeItem.CubeConfiguration;
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