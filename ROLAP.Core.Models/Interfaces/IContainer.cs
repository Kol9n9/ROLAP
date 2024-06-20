namespace ROLAP.Core.Models.Interfaces;

public interface IContainer : ICubeItem
{
    void AddValue<T>(T item);
    void AddValue<T>(IEnumerable<T> items);

    IEnumerable<ICubeItem> GetValues();
    
    IEnumerable<T> GetValues<T>();
    
    IContainer? FindByHierarchy(string[] hierarchy);

    IEnumerable<IContainer> Merge(IEnumerable<IContainer> containers);

    bool InContainer(IContainer container);

    bool InContainers(IEnumerable<IContainer> containers);
}