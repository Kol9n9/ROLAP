using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Core.Models.Model.Containers;

public class MeasureContainer : IContainer
{
    private List<MeasureCubeItem> _values = new List<MeasureCubeItem>();
    public CubeItemType GetItemType()
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IContainer> Merge(IEnumerable<IContainer> containers)
    {
        if (!containers.Any())
        {
            return new List<IContainer> { this };
        }

        var typeContainer = containers.OfType<MeasureContainer>();
        if (!typeContainer.Any())
        {
            return new List<IContainer>(containers) { this };
        }

        foreach (var value in _values)
        {
            var findContainer = typeContainer.FirstOrDefault(x => x.FindByHierarchy(new string[]{"",value.GetName()}) != null);
            if (findContainer is null)
            {
                if (typeContainer.Count() == 1)
                {
                    typeContainer.FirstOrDefault().AddValue(value);
                }
                else
                {
                    var container = new MeasureContainer();
                    container.AddValue(value);
                    containers = new List<IContainer>(containers) { container };
                    
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

    public bool NameEqual(string name)
    {
        throw new NotImplementedException();
    }

    public void AddValue(ICubeItem item) => _values.Add((MeasureCubeItem)item);
    public void AddValue(IEnumerable<ICubeItem> items) => _values.AddRange(items.Cast<MeasureCubeItem>());

    public IEnumerable<ICubeItem> GetValues() => _values;
    public IContainer? FindByHierarchy(string[] hierarchy)
    {
        var measure = _values.FirstOrDefault(x => x.Name == hierarchy[1]);
        if (measure is null) return null;
        var container = new MeasureContainer();
        container.AddValue(measure.Clone<MeasureCubeItem>());
        return container;
    }
}