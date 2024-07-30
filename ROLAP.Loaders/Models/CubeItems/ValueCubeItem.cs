using ROLAP.Core.Models.Interfaces.Value;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Loaders.Models.CubeItems;

internal class ValueCubeItem : IValueCubeItem
{
    public string Id { get; }
    public IValue Value { get; }
    public IContainer Measure { get; }
    
    public IEnumerable<IDimensionCubeItem> Dimensions { get; }

    public ValueCubeItem(string id, IValue value, IContainer measure, IEnumerable<IDimensionCubeItem> dimensions)
    {
        Id = id;
        Value = value;
        Measure = measure;
        Dimensions = dimensions;
    }
    
    public T Clone<T>(bool withValues) where T : ICubeItem
    {
        var val = (ICubeItem)(new ValueCubeItem(Id, Value, Measure, Dimensions.Select(x => x.Clone<IDimensionCubeItem>(withValues))));
        return (T)val;
    }

    public ICubeItem FindByHierarchy(string[] hierarchy)
    {
        throw new NotImplementedException();
    }

    public bool Contains(ICubeItem item)
    {
        throw new NotImplementedException();
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
        return Measure.GetValues<IMeasureCubeItem>().FirstOrDefault()!;
    }
}