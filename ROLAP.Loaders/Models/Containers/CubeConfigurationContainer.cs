using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Loaders.Models.CubeItems;

namespace ROLAP.Loaders.Models.Containers;

internal class CubeConfigurationContainer : IContainer
{
    private List<CubeConfiguration> _values = new List<CubeConfiguration>();

    public IEnumerable<IContainer> Merge(IEnumerable<IContainer> containers)
    {
        throw new NotImplementedException();
    }

    public bool InContainer(IContainer container)
    {
        return false;
    }
    
    public bool InContainers(IEnumerable<IContainer> containers)
    {
        return containers.Any(InContainer);
    }

    public ICubeItem Clone(bool withValues = true)
    {
        throw new NotImplementedException();
    }

    public T Clone<T>(bool withValues) where T : ICubeItem
    {
        throw new NotImplementedException();
    }

    public void AddValue<T>(T item)
    {
        var configuration = item as CubeConfiguration;
        if (configuration is null) throw new NotSupportedException();
        _values.Add(configuration);
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
        if (typeof(T) == typeof(CubeConfiguration)) return _values.Cast<T>();
        throw new NotSupportedException();
    }
    
    public IEnumerable<ICubeItem> GetValues()
    {
        return _values;
    }

    public IContainer FindByHierarchy(string[] hierarchy)
    {
        throw new NotImplementedException();
    }
}