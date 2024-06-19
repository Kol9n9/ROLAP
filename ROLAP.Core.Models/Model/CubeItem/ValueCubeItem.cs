using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.CubeItem;

public class ValueCubeItem : ICubeItem
{
    public string Id { get; }
    public string Value { get; }
    public MeasureCubeItem Measure { get; }
    
    public IEnumerable<IContainer> Dimensions { get; }

    public ValueCubeItem(string id, string value, MeasureCubeItem measure, IEnumerable<IContainer> dimensions)
    {
        Id = id;
        Value = value;
        Measure = measure;
        Dimensions = dimensions;
    }
    
    public ICubeItem Clone(bool withInnerValues = true)
    {
        //var dimensions = withInnerValues ? Dimensions.Select(x => x.Clone()).Cast<DimensionCubeItem>() : new List<DimensionCubeItem>();
        return new ValueCubeItem(Id, Value, Measure, new List<IContainer>());
    }
    
    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        var clone = Clone(withInnerValues);
        return (T)clone;
    }
}