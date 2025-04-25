using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Model.Query;

namespace ROLAP.Process.Models.Result;

/// <summary>
/// Результат выполнения запроса
/// </summary>
internal class CubeResult
{
    public IEnumerable<CubeItemSet> Axes { get; }
    public IEnumerable<ICubeItem> Values { get; }
    public bool IsAggregated { get; }
    public CubeResult(IEnumerable<CubeItemSet> axes, IEnumerable<ICubeItem> values, bool isAggregated)
    {
        Axes = axes;
        Values = values;
        IsAggregated = isAggregated;
    }
}