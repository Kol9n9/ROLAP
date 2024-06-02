using ROLAP.Common.Model;

namespace ROLAP.Process.Models.Result;

internal class CubeResultTuple
{
    public IEnumerable<CubeItem> Members { get; }

    public CubeResultTuple(IEnumerable<CubeItem> members)
    {
        Members = members;
    }
}