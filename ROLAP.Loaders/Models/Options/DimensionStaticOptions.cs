using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Loaders.Models.Options;

internal class DimensionStaticOptions : ILoadOptions
{
    public string Key { get; }
    public string Name { get; }
    public IEnumerable<ILoadOptions> Values { get; }

    public DimensionStaticOptions(string key, string name, IEnumerable<ILoadOptions> values)
    {
        Key = key;
        Name = name;
        Values = values;
    }
}