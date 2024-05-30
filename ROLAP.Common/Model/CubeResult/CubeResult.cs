namespace ROLAP.Common.Model.CubeResult;

public class CubeResult
{
    public IEnumerable<CubeResultSet> Axes { get; }
    public IEnumerable<MeasureValue> Values { get; }
    public CubeResult(IEnumerable<CubeResultSet> axes, IEnumerable<MeasureValue> values)
    {
        Values = values;
        Axes = axes;
    }
}