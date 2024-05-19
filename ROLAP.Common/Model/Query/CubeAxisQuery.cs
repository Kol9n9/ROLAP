using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public class CubeAxisQuery
{
    public ICubeQueryItem Member { get; }
    public int Number { get; }

    public CubeAxisQuery(ICubeQueryItem member, int number)
    {
        Member = member;
        Number = number;
    }
}