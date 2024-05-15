using Newtonsoft.Json;
using ROLAP.Configuration.Loaders.Base;
using ROLAP.Parser;

namespace ROLAP.Configuration.Tests;

public class Tests
{
    private CubeDimensionLoader _dimensionLoader = new CubeDimensionLoader();
    private CubeMeasureLoader _measureLoader = new CubeMeasureLoader();
    
    [SetUp]
    public void Setup()
    {
        //LoadCubeConfigurationRes();
    }

    private void LoadCubeConfigurationRes()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Tests", "CubeConfigurationParser");
        foreach (var file in Directory.GetFiles(path, "*_expected.txt"))
        {
            var expected = ReadAllFromFile(file);
            var text = ReadAllFromFile(file.Replace("_expected", ""));
            //_cubeConfigurations.Add(new CubeConfigurationTestItem(text,expected));
        }
    }

    private string ReadAllFromFile(string path)
    {
        return File.ReadAllText(path);
    }
    
    [Test]
    public void CubeConfigurationTest()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Tests", "CubeConfigurationParser");
        var text = File.ReadAllText(Path.Combine(path, "3.txt"));
        var cubeConfiguration = CubeConfigurationParser.ParseCubeConfiguration(text);

        var str = JsonConvert.SerializeObject(cubeConfiguration);
        
        var dimensions = _dimensionLoader.Load(cubeConfiguration.DimensionOptions);
        var measures = _measureLoader.Load(cubeConfiguration.MeasureOptions,dimensions);

        var values = measures.FirstOrDefault(x => x.Key == "1").ValuesLoader.Load();
        
        Assert.Pass();
    }

    private record CubeConfigurationTestItem(string Cube, string ExpectedCube);
}