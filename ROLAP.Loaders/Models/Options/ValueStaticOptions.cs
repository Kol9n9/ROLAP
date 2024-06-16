using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Loaders.Models.Options;

internal class ValueStaticOptions : ILoadOptions
{
    public string Id { get; }
    public string Value { get; }
    public IEnumerable<ILoadOptions> Dimensions { get; }

    public ValueStaticOptions(string id, string value, IEnumerable<ILoadOptions> dimensions)
    {
        Id = id;
        Value = value;
        Dimensions = dimensions;
    }
}