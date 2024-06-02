using ROLAP.Common.Model;

namespace ROLAP.Process.Models.Result;

/// <summary>
/// Результат выполнения запроса
/// </summary>
internal class CubeResult
{
    public IEnumerable<CubeResultSet> Axes { get; }
    public IEnumerable<MeasureValue> Values { get; }
    public CubeResult(IEnumerable<CubeResultSet> axes, IEnumerable<MeasureValue> values)
    {
        Values = values;
        Axes = axes;
    }
}