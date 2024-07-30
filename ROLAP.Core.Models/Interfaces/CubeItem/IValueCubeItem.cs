using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Core.Models.Interfaces.CubeItem;

public interface IValueCubeItem : ICubeItem
{
    string GetId();
    IValue GetValue();
    string GetFormattedValue();

    IEnumerable<IContainer> GetDimensions();
    IContainer GetMeasureContainer();
    IMeasureCubeItem GetMeasure();
}