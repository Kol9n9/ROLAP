using ROLAP.Core.Models.Interfaces.Loader;

namespace ROLAP.Loaders.Models.Options;

internal class MeasureStaticOptions : ILoadOptions
{
    public string Key { get; }
    public string Name { get; }
    public IEnumerable<ILoadOptions> ValuesOptions { get; }

    public MeasureStaticOptions(string key, string name, IEnumerable<ILoadOptions> valuesOptions)
    {
        Key = key;
        Name = name;
        ValuesOptions = valuesOptions;
    }
}