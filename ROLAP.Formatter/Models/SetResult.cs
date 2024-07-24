namespace ROLAP.Formatter.Models;

internal class SetResult
{
    public IEnumerable<TupleResult> Tuples { get; }

    public SetResult(IEnumerable<TupleResult> tuples)
    {
        Tuples = tuples;
    }
}