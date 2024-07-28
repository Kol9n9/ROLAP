using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Loaders.Models.CubeItems;

internal class CubeConfiguration : ICubeConfiguration
{
    /// <summary>
    /// Измерения
    /// </summary>
    public IContainer Dimensions { get; }
    
    /// <summary>
    /// Меры
    /// </summary>
    public IContainer Measures { get; }

    public CubeConfiguration(IContainer dimensions, IContainer measures)
    {
        Dimensions = dimensions;
        Measures = measures;
    }

    public T Clone<T>(bool withValues) where T : ICubeItem
    {
        throw new NotSupportedException();
    }

    public IContainer GetDimensions() => Dimensions;

    public IContainer GetMeasures() => Measures;
}