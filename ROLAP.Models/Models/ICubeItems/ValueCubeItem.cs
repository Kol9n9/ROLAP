using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Models.Models.ICubeItems;

public class ValueCubeItem : ICubeItem
{
    public string Id { get; }
    public string Value { get; }
    public IContainer Measure { get; }
    
    public IEnumerable<IContainer> Dimensions { get; }

    public ValueCubeItem(string id, string value, IContainer measure, IEnumerable<IContainer> dimensions)
    {
        Id = id;
        Value = value;
        Measure = measure;
        Dimensions = dimensions;
    }
    
    public T Clone<T>(bool withValues) where T : ICubeItem
    {
        if (typeof(T) != typeof(ValueCubeItem) && typeof(T) != typeof(ICubeItem)) throw new InvalidCastException($"Получить копию можно только для типа \"{nameof(ValueCubeItem)}\"");

        var val = (ICubeItem)(new ValueCubeItem(Id, Value, Measure, Dimensions.Select(x => x.Clone<IContainer>(withValues))));
        return (T)val;
    }
}