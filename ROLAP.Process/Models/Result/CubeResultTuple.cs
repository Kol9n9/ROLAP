using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Process.Models.Result;

internal class CubeResultTuple
{
    public IEnumerable<ICubeItem> Members { get; }

    public CubeResultTuple(IEnumerable<ICubeItem> members)
    {
        Members = members;
    }
}