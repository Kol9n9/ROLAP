namespace ROLAP.Common.Model;

public class CubeItemTuple
{
    public IEnumerable<CubeItem> Members { get; }

    public CubeItemTuple(IEnumerable<CubeItem> members)
    {
        Members = members;
    }
}