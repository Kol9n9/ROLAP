using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.QueryProcessor.Models.Items;

namespace ROLAP.QueryProcessor.Models.MemberFunctions;

internal class MembersFunc
{
    public static SetItem Execute(MemberItem item, ICubeConfiguration cubeConfiguration)
    {
        IEnumerable<IDimensionCubeItem> dimensions = cubeConfiguration.GetDimensions();
        
        var hierarchies = item.Hierarchy;
        foreach (var hierarchy in hierarchies)
        {
            dimensions = dimensions.Where(dimension => dimension.GetName().Equals(hierarchy)).SelectMany(dimension => dimension.GetDimensions());
        }

        List<MemberItem> members = new List<MemberItem>();
        foreach (var dimension in dimensions)
        {
            members.Add(new MemberItem(GetHierarchy(hierarchies, dimension.GetName()),String.Empty));
        }

        return new  SetItem(members);
    }

    private static string[] GetHierarchy(string[] parentHierarchy, string itemName)
    {
        var newHierarchy = new List<string>(parentHierarchy);
        newHierarchy.Add(itemName);
        return newHierarchy.ToArray();
    }
}