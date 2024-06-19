using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Core.Models.Model.Containers;

public class MeasureContainer : IContainer
{
    private List<MeasureCubeItem> _values = new List<MeasureCubeItem>();

    public IEnumerable<IContainer> Merge(IEnumerable<IContainer> containers)
    {
        var collection = containers.ToList();
        
        if (!collection.Any())
        {
            return new List<IContainer> { this };
        }

        var typeContainer = collection.OfType<MeasureContainer>();
        var measureContainers = typeContainer as MeasureContainer[] ?? typeContainer.ToArray();
        if (!measureContainers.Any())
        {
            return new List<IContainer>(collection) { this };
        }

        foreach (var value in _values)
        {
            var findContainer = measureContainers.FirstOrDefault(x => x.FindByHierarchy(new string[]{"",value.Name}) != null);
            if (findContainer is null)
            {
                if (measureContainers.Length == 1)
                {
                    measureContainers.FirstOrDefault()?.AddValue(value);
                }
                else
                {
                    var container = new MeasureContainer();
                    container.AddValue(value);
                    collection = new List<IContainer>(collection) { container };
                    
                }
            }
        }

        return collection;
    }
    
    public bool InContainer(IContainer container)
    {
        if (container is not MeasureContainer measureContainer) return false;
        return _values.All(value => measureContainer._values.Exists(val => val.Key == value.Key));
    }
    
    public bool InContainers(IEnumerable<IContainer> containers)
    {
        return containers.Any(InContainer);
    }

    public void AddValue<T>(T item)
    {
        var measure = item as MeasureCubeItem;
        if (measure is null) throw new NotSupportedException();
        _values.Add(measure);
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
        if (typeof(T) != typeof(MeasureCubeItem)) throw new NotSupportedException();
        return _values.Cast<T>();
    }

    public IContainer? FindByHierarchy(string[] hierarchy)
    {
        var measure = _values.FirstOrDefault(x => x.Name == hierarchy[1]);
        if (measure is null) return null;
        var container = new MeasureContainer();
        container.AddValue(measure.Clone<MeasureCubeItem>());
        return container;
    }
}