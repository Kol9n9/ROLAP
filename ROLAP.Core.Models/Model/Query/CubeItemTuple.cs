using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.Query;

public class CubeItemTuple
{
    public IEnumerable<ICubeItem> Members { get; }

    public CubeItemTuple(IEnumerable<ICubeItem> members)
    {
        Members = members;
    }
}