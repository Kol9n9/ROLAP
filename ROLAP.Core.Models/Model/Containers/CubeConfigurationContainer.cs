using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Core.Models.Model.Containers;

public class CubeConfigurationContainer : ICubeItem, IContainer
{
    private List<CubeConfiguration> _values = new List<CubeConfiguration>();
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
        throw new NotImplementedException();
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

    public void AddValue(ICubeItem item) => _values.Add((CubeConfiguration)item);

    public void AddValue(IEnumerable<ICubeItem> items) => _values.AddRange(items.Cast<CubeConfiguration>());

    public IEnumerable<ICubeItem> GetValues() => _values;
    public IContainer FindByHierarchy(string[] hierarchy)
    {
        throw new NotImplementedException();
    }
}