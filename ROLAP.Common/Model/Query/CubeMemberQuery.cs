using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public class CubeMemberQuery : ICubeQueryItem
{
    public string[] Hierarchy { get; }

    public CubeMemberQuery(string[] hierarchy)
    {
        Hierarchy = hierarchy;
    }

    public ICubeQueryItem Execute(Cube cube)
    {
        return this;
    }
}