using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.CubeItem;

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

    public ICubeItem Clone(bool withInnerValues = true)
    {
        throw new NotSupportedException();
    }

    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        throw new NotImplementedException();
    }
    
    public bool NameEqual(string name)
    {
        throw new NotImplementedException();
    }
}