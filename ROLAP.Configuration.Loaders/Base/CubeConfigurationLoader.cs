using ROLAP.Common.Model;
using ROLAP.Configuration.Models.Interfaces;
using ROLAP.Parser;

namespace ROLAP.Configuration.Loaders.Base;

public class CubeConfigurationLoader : ICubeConfigurationLoader
{
    private CubeDimensionLoader _dimensionLoader = new CubeDimensionLoader();
    private CubeMeasureLoader _measureLoader = new CubeMeasureLoader();
    
    public Cube Load(string name)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory());
        var text = File.ReadAllText(Path.Combine(path, "3.txt"));
        
        var configuration = CubeConfigurationParser.ParseCubeConfiguration(text);

        var dimensions = _dimensionLoader.Load(configuration.DimensionOptions).ToList();
        var measures = _measureLoader.Load(configuration.MeasureOptions, dimensions).ToList();

        return new Cube(dimensions, measures);
    }
}