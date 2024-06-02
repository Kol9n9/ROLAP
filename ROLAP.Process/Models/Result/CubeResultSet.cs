namespace ROLAP.Process.Models.Result;

internal class CubeResultSet
{
    public IEnumerable<CubeResultTuple> Tuples { get; }

    public CubeResultSet(IEnumerable<CubeResultTuple> tuples)
    {
        Tuples = tuples;
    }
} 