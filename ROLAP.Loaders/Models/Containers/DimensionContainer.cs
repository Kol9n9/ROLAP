using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Loaders.Models.Containers;

internal class DimensionContainer : IDimensionContainer
{
    private List<DimensionContainer> _values = new List<DimensionContainer>();
    public IDimensionCubeItem Item { get; }

    public DimensionContainer(IDimensionCubeItem dimensionCubeItem)
    {
        Item = dimensionCubeItem;
    }
    
    public IEnumerable<IContainer> Merge(IEnumerable<IContainer> containers)
    {
        if (!containers.Any())
        {
            return new List<IContainer> { this };
        }
        
        var typeContainers = containers.OfType<DimensionContainer>();
        if (!typeContainers.Any())
        {
            return new List<IContainer>(containers) { this };
        }

        var parentContainer = typeContainers.FirstOrDefault(x => x.Item.GetName() == Item.GetName());
        if (parentContainer is null)
        {
            var container = new DimensionContainer(Item);
            foreach (var value in _values)
            {
                container.AddValue(value);
            }
            return new List<IContainer>(containers) { container };
        }
        
        foreach (var value in _values)
        {
            var findContainer = parentContainer._values.FirstOrDefault(x => x.Item.GetName() == value.Item.GetName());
            if (findContainer is null)
            {
                parentContainer.AddValue(value);
            } else if (value._values.Any())
            {
                foreach (var _value in value._values)
                {
                    _value.Merge(parentContainer._values);
                }
            }
        }
        
        return containers;
    }

    public bool InContainer(IContainer container)
    {
        if (container is not DimensionContainer dimensionContainer) return false;

        if (Item.GetKey() != dimensionContainer.Item.GetKey()) return false;
        
        return _values.All(value =>
        {
            return !dimensionContainer._values.Any() || dimensionContainer._values.Exists(containerValue =>
            {
                var res = value.Item.GetKey() == containerValue.Item.GetKey();
                if (!res) return false;
                if (value._values.Any())
                {
                    if (!containerValue._values.Any()) return false;
                    return value._values.All(innerValue => containerValue._values.Any(innerValue.InContainer));
                }

                return true;
            });
        });
    }
    
    public bool InContainers(IEnumerable<IContainer> containers)
    {
        return containers.Any(InContainer);
    }

    public ICubeItem GetItem()
    {
        return Item;
    }

    public T Clone<T>(bool withValues = true) where T : ICubeItem
    {
        if (typeof(T) != typeof(DimensionContainer) && typeof(T) != typeof(IContainer) && typeof(T) != typeof(ICubeItem)) throw new InvalidCastException($"Получить копию можно только для типа \"{nameof(DimensionContainer)}\"");

        DimensionContainer container = new DimensionContainer(Item.Clone<IDimensionCubeItem>(withValues));
        if(withValues) container.AddValue(_values.Select(x => x.Clone<DimensionContainer>(withValues)));
        return (T)(ICubeItem)container;
    }

    public void AddValue<T>(T item)
    {
        var dimensionContainer = item as DimensionContainer;
        if (dimensionContainer is null) throw new NotSupportedException();
        _values.Add(dimensionContainer);
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
        return _values.Cast<T>();
    }

    public IEnumerable<ICubeItem> GetValues()
    {
        return _values;
    }
    
    public IContainer? FindByHierarchy(string[] hierarchy)
    {
        IContainer result = null;
        DimensionContainer current = null;
        
        DimensionContainer? tmp = null;
        IEnumerable<DimensionContainer> currents = _values;
        int index = 0;
        
        while (index < hierarchy.Length)
        {
            tmp = currents.FirstOrDefault(x => x.Item.GetName() == hierarchy[index]) as DimensionContainer;
            index++;
            if (tmp is null) return null;
            currents = tmp.GetValues<DimensionContainer>();
            var container = new DimensionContainer(tmp.Item);
            
            if (result is null)
            {
                result = container;
                current = container;
                continue;
            }
            
            current.AddValue(container);
            current = container;
        }

        return result;
    }
}