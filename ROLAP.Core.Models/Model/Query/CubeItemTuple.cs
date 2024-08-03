using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Core.Models.Model.Query;

public class CubeItemTuple
{
    public IEnumerable<ICubeItem> Members { get; private set; }

    public CubeItemTuple(IEnumerable<ICubeItem> members)
    {
        Members = members;
    }

    public void AddMember(ICubeItem cubeItem)
    {
        Members = new List<ICubeItem>(Members) { cubeItem };
    }
}