using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.Containers;

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
    
    public CubeItemType GetItemType() => CubeItemType.CubeConfiguration;
    public string GetKey()
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        throw new NotImplementedException();
    }

    public ICubeItem Clone(bool withInnerValues = true)
    {
        throw new NotSupportedException();
    }

    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        throw new NotImplementedException();
    }

    public void AddValue(ICubeItem item)
    {
        throw new NotSupportedException();
    }

    public bool IsContainer() => false;

    public IEnumerable<ICubeItem> GetValues()
    {
        throw new NotSupportedException();
    }

    public bool NameEqual(string name)
    {
        throw new NotImplementedException();
    }
}