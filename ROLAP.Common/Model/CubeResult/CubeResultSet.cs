namespace ROLAP.Common.Model.CubeResult;

public class CubeResultSet
{
    public IEnumerable<CubeResultTuple> Tuples { get; }

    public CubeResultSet(IEnumerable<CubeResultTuple> tuples)
    {
        Tuples = tuples;
    }
}