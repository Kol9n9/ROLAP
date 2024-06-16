using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Loaders.Utils.Parser;

internal class CubeConfiguration
{
    public IEnumerable<ILoadOptions> Dimensions { get; }
    public IEnumerable<ILoadOptions> Measures { get; }

    public CubeConfiguration(IEnumerable<ILoadOptions> dimensions, IEnumerable<ILoadOptions> measures)
    {
        Dimensions = dimensions;
        Measures = measures;
    }
}