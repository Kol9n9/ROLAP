using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public class CubeAxisQuery : ICubeQueryItem
{
    public ICubeQueryItem Member { get; }
    public int Number { get; }

    public CubeAxisQuery(ICubeQueryItem member, int number)
    {
        Member = member;
        Number = number;
    }

    public ICubeQueryItem Execute(Cube cube)
    {
        var newMember = Member.Execute(cube);
        return new CubeAxisQuery(newMember,Number);
    }
}