using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Models.Models.ICubeItems;

public class CubeConfiguration : ICubeItem
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
    
    public bool NameEqual(string name)
    {
        throw new NotImplementedException();
    }
}