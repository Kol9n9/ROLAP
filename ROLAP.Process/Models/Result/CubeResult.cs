using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.Query;

namespace ROLAP.Process.Models.Result;

/// <summary>
/// Результат выполнения запроса
/// </summary>
internal class CubeResult
{
    public IEnumerable<CubeItemSet> Axes { get; }
    public IEnumerable<ICubeItem> Values { get; }
    public CubeResult(IEnumerable<CubeItemSet> axes, IEnumerable<ICubeItem> values)
    {
        Axes = axes;
        Values = values;
    }
}