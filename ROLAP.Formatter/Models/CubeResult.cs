namespace ROLAP.Formatter.Models;

internal class CubeResult
{
    public IEnumerable<SetResult> Sets { get; }
    public IEnumerable<ValueResult> Values { get; }

    public CubeResult(IEnumerable<SetResult> sets, IEnumerable<ValueResult> values)
    {
        Sets = sets;
        Values = values;
    }
}