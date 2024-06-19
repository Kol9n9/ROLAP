using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Core.Models.Model.Containers;

public class ValueContainer : IContainer
{
    private List<ValueCubeItem> _values = new List<ValueCubeItem>();
    
    public IEnumerable<IContainer> Merge(IEnumerable<IContainer> containers)
    {
        throw new NotImplementedException();
    }

    public bool InContainer(IContainer container)
    {
        if (container is not ValueContainer valueContainer) return false;
        return _values.All(value => valueContainer._values.Exists(val => val.Id == value.Id));
    }

    public bool InContainers(IEnumerable<IContainer> containers)
    {
        return containers.Any(InContainer);
    }


    public void AddValue<T>(T item)
    {
        var value = item as ValueCubeItem;
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

    public IEnumerable<T> GetValues<T>()
    {
        if (typeof(T) != typeof(ValueCubeItem)) throw new NotSupportedException();
        return _values.Cast<T>();
    }

    public IContainer FindByHierarchy(string[] hierarchy)
    {
        throw new NotImplementedException();
    }
}