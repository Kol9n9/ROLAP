using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.QueryProcessor.Interfaces;
using ROLAP.QueryProcessor.Models.MemberFunctions;

namespace ROLAP.QueryProcessor.Models.Items;

internal class MemberItem : IQueryItem
{
    public string[] Hierarchy { get; }
    public string FunctionName { get; }
    public MemberItem(string[] hierarchy, string functionName)
    {
        Hierarchy = hierarchy;
        FunctionName = functionName;
    }

    public IQueryItem Execute(ICubeConfiguration configurationCube)
    {
        if (!string.IsNullOrWhiteSpace(FunctionName))
        {
            switch (FunctionName.ToLower())
            {
                case "members": return MembersFunc.Execute(this,configurationCube);
                case "children": return ChildrenFunc.Execute(this,configurationCube);
            }
        }

        return this;
    }
}