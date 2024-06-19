using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Core.Models.Model.Containers;

public class CubeConfigurationContainer : IContainer
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

    public IContainer FindByHierarchy(string[] hierarchy)
    {
        throw new NotImplementedException();
    }
}