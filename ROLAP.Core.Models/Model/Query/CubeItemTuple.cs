using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Model.Query;

public class CubeItemTuple
{
    public IEnumerable<IContainer> Members { get; }

    public CubeItemTuple(IEnumerable<IContainer> members)
    {
        Members = members;
    }
}