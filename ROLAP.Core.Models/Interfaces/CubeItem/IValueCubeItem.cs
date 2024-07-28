using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Core.Models.Interfaces.CubeItem;

public interface IValueCubeItem : ICubeItem
{
    string GetId();
    string GetValue();
    string GetFormattedValue();

    IEnumerable<IContainer> GetDimensions();
    IContainer GetMeasure();
}