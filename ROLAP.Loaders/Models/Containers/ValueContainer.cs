using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Loaders.Models.Containers;

internal class ValueContainer : IValueContainer
{
    private List<IValueCubeItem> _values = new List<IValueCubeItem>();
    
    public IEnumerable<IContainer> Merge(IEnumerable<IContainer> containers)
    {
        throw new NotImplementedException();
    }

    public bool InContainer(IContainer container)
    {
        if (container is not ValueContainer valueContainer) return false;
        return _values.All(value => valueContainer._values.Exists(val => val.GetId() == value.GetId()));
    }

    public bool InContainers(IEnumerable<IContainer> containers)
    {
        return containers.Any(InContainer);
    }

    public T Clone<T>(bool withValues = true) where T : ICubeItem
    {
        ValueContainer container = new ValueContainer();
        if (withValues) container.AddValue(_values.Select(x => x.Clone<IValueCubeItem>(withValues)));
        return (T)(ICubeItem)container;
    }


    public void AddValue<T>(T item)
    {
        var value = item as IValueCubeItem;
        if (value is null) throw new NotSupportedException();
        _values.Add(value);
    }

    public void AddValue<T>(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            AddValue(item);
        }
    }

    public IEnumerable<ICubeItem> GetValues()
    {
        return _values;
    }

    public IEnumerable<T> GetValues<T>()
    {
        return _values.Cast<T>();
    }

    public IContainer FindByHierarchy(string[] hierarchy)
    {
        throw new NotImplementedException();
    }
}