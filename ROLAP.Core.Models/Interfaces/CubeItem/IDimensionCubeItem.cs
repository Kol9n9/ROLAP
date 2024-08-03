namespace ROLAP.Core.Models.Interfaces.CubeItem;

public interface IDimensionCubeItem : IMemberCubeItem
{
    IEnumerable<IDimensionCubeItem> GetDimensions();
    void AddDimension(IDimensionCubeItem dimension);
}