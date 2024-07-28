using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Loaders.Models.Containers;

internal class MeasureContainer : IMeasureContainer
{
    private List<IMeasureCubeItem> _values = new List<IMeasureCubeItem>();

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
            var findContainer = measureContainers.FirstOrDefault(x => x.FindByHierarchy(new string[]{"",value.GetName()}) != null);
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
        return _values.All(value => !measureContainer._values.Any() || measureContainer._values.Exists(val => val.GetKey() == value.GetKey()));
    }
    
    public bool InContainers(IEnumerable<IContainer> containers)
    {
        return containers.Any(InContainer);
    }

    public void AddValue<T>(T item)
    {
        var measure = item as IMeasureCubeItem;
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
    
    public T Clone<T>(bool withValues = true) where T : ICubeItem
    {
        MeasureContainer container = new MeasureContainer();
        if (withValues) container.AddValue(_values.Select(x => x.Clone<IMeasureCubeItem>(withValues)));
        return (T)(ICubeItem)container;
    }

    public IEnumerable<T> GetValues<T>()
    {
        return _values.Cast<T>();
    }

    public IEnumerable<ICubeItem> GetValues()
    {
        return _values;
    }
    
    public IContainer? FindByHierarchy(string[] hierarchy)
    {
        var measure = _values.FirstOrDefault(x => x.GetName() == hierarchy[1]);
        if (measure is null) return null;
        var container = new MeasureContainer();
        container.AddValue(measure.Clone<IMemberCubeItem>(false));
        return container;
    }
}