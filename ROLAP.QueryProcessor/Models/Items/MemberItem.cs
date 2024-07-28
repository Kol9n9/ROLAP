using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal class MemberItem : IQueryItem
{
    public string[] Hierarchy { get; }

    public MemberItem(string[] hierarchy)
    {
        Hierarchy = hierarchy;
    }

    public IQueryItem Execute(ICubeConfiguration configurationCube)
    {
        return this;
    }
}