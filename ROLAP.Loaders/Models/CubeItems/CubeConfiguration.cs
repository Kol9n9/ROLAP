using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Container;

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

    public T Clone<T>(bool withValues) where T : ICubeItem
    {
        throw new NotSupportedException();
    }

    public ICubeItem FindByHierarchy(string[] hierarchy)
    {
        throw new NotImplementedException();
    }

    public bool Contains(ICubeItem item)
    {
        throw new NotImplementedException();
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