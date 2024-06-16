using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.CubeItem;

public class ValueCubeItem : ICubeItem
{
    public string Id { get; }
    public string Value { get; }
    public MeasureCubeItem Measure { get; }
    
    public IEnumerable<DimensionCubeItem> Dimensions { get; }

    public ValueCubeItem(string id, string value, MeasureCubeItem measure, IEnumerable<DimensionCubeItem> dimensions)
    {
        Id = id;
        Value = value;
        Measure = measure;
        Dimensions = dimensions;
    }
    
    public CubeItemType GetItemType() => CubeItemType.Value;
    
    public string GetKey() => Id;

    public string GetName() => throw new NotSupportedException();
    
    public ICubeItem Clone(bool withInnerValues = true)
    {
        var dimensions = withInnerValues ? Dimensions.Select(x => x.Clone()).Cast<DimensionCubeItem>() : new List<DimensionCubeItem>();
        return new ValueCubeItem(Id, Value, Measure, dimensions);
    }
    
    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        var clone = Clone(withInnerValues);
        return (T)clone;
    }

    public void AddValue(ICubeItem item)
    {
        throw new NotSupportedException();
    }

    public IEnumerable<ICubeItem> GetValues()
    {
        throw new NotSupportedException();
    }

    public bool NameEqual(string name)
    {
        return Id.Equals(name);
    }
}