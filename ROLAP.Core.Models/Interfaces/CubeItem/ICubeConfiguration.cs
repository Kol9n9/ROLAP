using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Core.Models.Interfaces.CubeItem;

public interface ICubeConfiguration : ICubeItem
{
    IEnumerable<IDimensionCubeItem> GetDimensions();
    IEnumerable<IMeasureCubeItem> GetMeasures();
}