namespace ROLAP.Common.Model.CubeResult;

public class CubeResultTuple
{
    public IEnumerable<ICubeItem> Members { get; }

    public CubeResultTuple(IEnumerable<ICubeItem> members)
    {
        Members = members;
    }
}