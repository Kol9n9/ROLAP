using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Core.Models.Model.Containers;

public class DimensionContainer : ICubeItem, IContainer
{
    private List<DimensionContainer> _values = new List<DimensionContainer>();
    public string Name { get; }

    public DimensionContainer(string name)
    {
        Name = name;
    }
    
    public CubeItemType GetItemType()
    {
        throw new NotImplementedException();
    }

    public string GetName() => Name;
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

        var parentContainer = typeContainers.FirstOrDefault(x => x.Name == Name);
        if (parentContainer is null)
        {
            var container = new DimensionContainer(Name);
            foreach (var value in _values)
            {
                container.AddValue(value);
            }
            return new List<IContainer>(containers) { container };
        }
        
        foreach (var value in _values)
        {
            var findContainer = parentContainer._values.FirstOrDefault(x => x.Name == value.Name);
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

    public ICubeItem Clone(bool withInnerValues = true)
    {
        throw new NotImplementedException();
    }

    public T Clone<T>(bool withInnerValues = true) where T : ICubeItem
    {
        throw new NotImplementedException();
    }

    public bool NameEqual(string name) => Name.Equals(name);

    public void AddValue(ICubeItem item) => _values.Add((DimensionContainer)item);
    public void AddValue(IEnumerable<ICubeItem> items) => _values.AddRange(items.Cast<DimensionContainer>());

    public IEnumerable<ICubeItem> GetValues() => _values;
    public IContainer? FindByHierarchy(string[] hierarchy)
    {
        IContainer result = null;
        DimensionContainer current = null;
        
        IContainer? tmp = null;
        IEnumerable<DimensionContainer> currents = _values;
        int index = 0;
        
        while (index < hierarchy.Length)
        {
            tmp = currents.FirstOrDefault(x => x.Name == hierarchy[index]);
            index++;
            if (tmp is null) return null;
            currents = tmp.GetValues().Cast<DimensionContainer>();
            var container = new DimensionContainer(tmp.GetName());
            
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
        
        var dimensionContainer = _values.FirstOrDefault(x => x.Name == hierarchy[0]);
        // private ICubeItem? FindDimension(CubeConfiguration configurationCube, string[] hierarchy)
        // {
        //     int i = 0;
        //
        //     ICubeItem? result = null;
        //     ICubeItem? temp = null;
        //     IEnumerable<DimensionCubeItem> dimensions = configurationCube.Dimensions;
        //     DimensionCubeItem? current = null;
        //
        //     do
        //     {
        //         if (!dimensions.Any()) return null;
        //         current = (DimensionCubeItem)dimensions.FirstOrDefault(x => x.NameEqual(hierarchy[i]));
        //         if (current is null) return null;
        //         if (result is null)
        //         {
        //             result = temp = new DimensionCubeItem(current.Key, current.Name, new List<DimensionCubeItem>());
        //         }
        //         else
        //         {
        //             var newVal = new DimensionCubeItem(current.Key, current.Name, new List<DimensionCubeItem>());
        //             temp.AddValue(newVal);
        //             temp = newVal;
        //         }
        //         dimensions = current.Values;
        //     
        //     } while (++i < hierarchy.Length);
        //
        //
        //     return result;
        // }
        throw new NotImplementedException();
    }
}