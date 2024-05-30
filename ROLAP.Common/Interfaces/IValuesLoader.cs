using ROLAP.Common.Model;

namespace ROLAP.Common.Interfaces;

public interface IValuesLoader
{
    IEnumerable<MeasureValue> Load(IEnumerable<CubeItem> dimensions);
}