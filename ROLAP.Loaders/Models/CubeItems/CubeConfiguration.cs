using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Models.CubeItems;

internal class CubeConfiguration : ICubeConfiguration
{
    /// <summary>
    /// Измерения
    /// </summary>
    public IEnumerable<IDimensionCubeItem> Dimensions { get; }
    
    /// <summary>
    /// Меры
    /// </summary>
    public IEnumerable<IMeasureCubeItem> Measures { get; }

    public CubeConfiguration(IEnumerable<IDimensionCubeItem> dimensions, IEnumerable<IMeasureCubeItem> measures)
    {
        Dimensions = dimensions;
        Measures = measures;
    }

    public ICubeItem Clone(bool withValues)
    {
        throw new NotSupportedException();
    }
    
    public IEnumerable<IDimensionCubeItem> GetDimensions()
    {
        return Dimensions;
    }

    public IEnumerable<IMeasureCubeItem> GetMeasures()
    {
        return Measures;
    }
}