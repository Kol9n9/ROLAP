using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal class AxisItem : IQueryItem
{
    public IQueryItem Member { get; }
    public int Number { get; }

    public AxisItem(IQueryItem member, int number)
    {
        Member = member;
        Number = number;
    }

    public IQueryItem Execute(ICubeConfiguration configurationCube)
    {
        var newMember = Member.Execute(configurationCube);
        return new AxisItem(newMember, Number);
    }
}