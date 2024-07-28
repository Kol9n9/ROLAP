using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Core.Models.Interfaces.CubeItem;

public interface ICubeConfiguration : ICubeItem
{
    IContainer GetDimensions();
    IContainer GetMeasures();
}