namespace ROLAP.Formatter.Models;

internal class TupleResult
{
    public IEnumerable<MemberResult> Members { get; }

    public TupleResult(IEnumerable<MemberResult> members)
    {
        Members = members;
    }
}