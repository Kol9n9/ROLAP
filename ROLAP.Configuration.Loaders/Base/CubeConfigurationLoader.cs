using ROLAP.Configuration.Models.Interfaces;
using ROLAP.Configuration.Models.Models;
using ROLAP.Parser;

namespace ROLAP.Configuration.Loaders.Base;

public class CubeConfigurationLoader : ICubeConfigurationLoader
{
    public CubeConfiguration Load(string name)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory());
        var text = File.ReadAllText(Path.Combine(path, "3.txt"));
        
        return CubeConfigurationParser.ParseCubeConfiguration(text);
    }
}