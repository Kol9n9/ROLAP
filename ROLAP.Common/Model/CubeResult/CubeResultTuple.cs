namespace ROLAP.Common.Model.CubeResult;

public class CubeResultTuple
{
    public IEnumerable<CubeItem> Members { get; }

    public CubeResultTuple(IEnumerable<CubeItem> members)
    {
        Members = members;
    }
}