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
    
        // var values = measures.FirstOrDefault(x => x.Key == "1").ValuesLoader.Load(dimensions);
        
        Assert.Pass();
    }


    [Test]
    public void QueryParserTest()
    {
        string mdx = "SELECT CrossJoin([Dimension].&[62E2E142-8A00-45AB-B8EA-A4CB277EB63F],{[Dimension].&[34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8],[Dimension].&[FC9122BD-4075-42AC-8F29-B7CC44C843D0]}) ON 0, " +
                     "{[Dimension].&[3ac02e75-2988-4bd6-9471-80557bbbcc0d],[Dimension].&[82e6587a-6350-4cb5-ba12-b18174aaec26]} ON 1 " +
                     "FROM [Adventure_Cube]";
        
        string mdx2 =
            "SELECT {[Университет].[ТГУ],[Университет].[ТПУ],[Measure].&[NumberOfApplicants]} ON 0, [Специальность].[Прикладная информатика] ON 1 FROM [Adventure_Cube]";
        
        string mdx3 =
            "SELECT CrossJoin([Университет].[ТГУ],[Measure].&[NumberOfApplicants]) ON 0 FROM [Adventure_Cube]";
        
        string mdx4 = "SELECT CrossJoin([Measure].[Доход],{[Факты и прогнозы].[План],[Факты и прогнозы].[Факт]}) ON 0, {[ОКВЭД].[Тест],[ОКВЭД].[Проверка]} ON 1 FROM [Adventure_Cube]";

        string mdx5 = "SELECT {[Университет].[ТГУ]} ON 1 FROM [Adventure_Cube]";
        
        try
        {
            QueryParser.Parse(mdx5);
        }
        catch (Exception ex)
        {
            Assert.Fail();
            return;
        }
        Assert.Pass();
    }
    
    private record CubeConfigurationTestItem(string Cube, string ExpectedCube);
}