using ROLAP.Core.Models.Interfaces.Loader;

namespace ROLAP.Loaders.Models.Options;

internal class CubeStaticOptions : ILoadOptions
{
    public string Name { get; }

    public CubeStaticOptions(string name)
    {
        Name = name;
    }
}