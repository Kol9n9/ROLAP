namespace ROLAP.Formatter.Models;

internal class CubeResult
{
    public IEnumerable<SetResult> Sets { get; }
    public IEnumerable<ValueResult> Values { get; }
    
    public bool IsAggregated { get; }

    public CubeResult(IEnumerable<SetResult> sets, IEnumerable<ValueResult> values, bool isAggregated)
    {
        Sets = sets;
        Values = values;
        IsAggregated = isAggregated;
    }
}