namespace ROLAP.Core.Models.Interfaces;

public interface IContainer
{
    void AddValue(ICubeItem item);
    void AddValue(IEnumerable<ICubeItem> items);
    
    IEnumerable<ICubeItem> GetValues();

    IContainer? FindByHierarchy(string[] hierarchy);

    string GetName();
    
    IEnumerable<IContainer> Merge(IEnumerable<IContainer> containers);
}