namespace ROLAP.Common.Model;

public class CubeItemSet
{
    public IEnumerable<CubeItemTuple> Tuples { get; }

    public CubeItemSet(IEnumerable<CubeItemTuple> tuples)
    {
        Tuples = tuples;
    }
}