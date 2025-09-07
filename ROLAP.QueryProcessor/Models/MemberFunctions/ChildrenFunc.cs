using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.QueryProcessor.Models.Items;

namespace ROLAP.QueryProcessor.Models.MemberFunctions;

internal class ChildrenFunc
{
    public static SetItem Execute(MemberItem item, ICubeConfiguration cubeConfiguration)
    {
        IEnumerable<IDimensionCubeItem> dimensions = cubeConfiguration.GetDimensions();
        
        var hierarchies = item.Hierarchy;
        foreach (var hierarchy in hierarchies)
        {
            dimensions = dimensions.Where(dimension => dimension.GetName().Equals(hierarchy)).SelectMany(dimension => dimension.GetDimensions());
        }
        
        return new  SetItem(GetAllDimensions(hierarchies,dimensions));
    }

    private static List<MemberItem> GetAllDimensions(string[] baseHierarchy, IEnumerable<IDimensionCubeItem> dimensions)
    {
        List<MemberItem> members = new List<MemberItem>();
        List<Tuple<IDimensionCubeItem, string[]>> memory = new  List<Tuple<IDimensionCubeItem, string[]>>();
        foreach (var dimension in dimensions)
        {
            members.Add(new MemberItem(GetHierarchy(baseHierarchy, dimension.GetName()), string.Empty));
            memory.Add(new Tuple<IDimensionCubeItem, string[]>(dimension,GetHierarchy(baseHierarchy, dimension.GetName())));
        }
        while (memory.Count > 0)
        {
            var first = memory.First();
            memory.RemoveAt(0);
            foreach (var item in first.Item1.GetDimensions())
            {
                members.Add(new MemberItem(GetHierarchy(first.Item2, item.GetName()), string.Empty));
                memory.Add(new Tuple<IDimensionCubeItem, string[]>(item,GetHierarchy(first.Item2, item.GetName())));
            }
        }
        return members;
    }
    
    private static string[] GetHierarchy(string[] parentHierarchy, string itemName)
    {
        var newHierarchy = new List<string>(parentHierarchy);
        newHierarchy.Add(itemName);
        return newHierarchy.ToArray();
    }
}