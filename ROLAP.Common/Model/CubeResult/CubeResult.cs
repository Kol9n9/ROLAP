namespace ROLAP.Common.Model.CubeResult;

public class CubeResult
{
    public IEnumerable<CubeResultSet> Axes { get; }

    public CubeResult(IEnumerable<CubeResultSet> axes)
    {
        Axes = axes;
    }
}