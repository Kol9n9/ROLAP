using ROLAP.Core.Models.Interfaces.Value;
using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Models.CubeItems;

internal class ValueCubeItem : IValueCubeItem
{
    public string Id { get; }
    public IValue Value { get; }
    public IMeasureCubeItem Measure { get; }
    
    public IEnumerable<IDimensionCubeItem> Dimensions { get; }

    public ValueCubeItem(string id, IValue value, IMeasureCubeItem measure, IEnumerable<IDimensionCubeItem> dimensions)
    {
        Id = id;
        Value = value;
        Measure = measure;
        Dimensions = dimensions;
    }
    
    public ICubeItem Clone(bool withValues)
    {
        var val = (ICubeItem)(new ValueCubeItem(Id, Value, Measure, Dimensions.Select(x => (IDimensionCubeItem)x.Clone(withValues))));
        return val;
    }

    public string GetId()
    {
        return Id;
    }

    public IValue GetValue()
    {
        return Value;
    }

    public string GetFormattedValue()
    {
        return Value.GetStringValue();
    }

    public IEnumerable<IDimensionCubeItem> GetDimensions() => Dimensions;

    public IMeasureCubeItem GetMeasure()
    {
        return Measure;
    }
}