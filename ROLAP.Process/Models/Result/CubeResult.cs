using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Process.Models.Result;

/// <summary>
/// Результат выполнения запроса
/// </summary>
internal class CubeResult
{
    public IEnumerable<CubeResultSet> Axes { get; }
    public IEnumerable<ValueCubeItem> Values { get; }
    public CubeResult(IEnumerable<CubeResultSet> axes, IEnumerable<ValueCubeItem> values)
    {
        Values = values;
        Axes = axes;
    }
}