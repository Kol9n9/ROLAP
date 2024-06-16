namespace ROLAP.Core.Models.Model.Query;

public class CubeItemSet
{
    public IEnumerable<CubeItemTuple> Tuples { get; }

    public CubeItemSet(IEnumerable<CubeItemTuple> tuples)
    {
        Tuples = tuples;
    }
}