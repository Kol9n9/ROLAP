using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal class SetItem : IQueryItem
{
    public IEnumerable<IQueryItem> Members { get; }
    public SetItem(IEnumerable<IQueryItem> members)
    {
        Members = members;
    }

    public IQueryItem Execute(ICubeConfiguration configurationCube)
    {
        List<IQueryItem> items = new List<IQueryItem>();

        foreach (var member in Members)
        {
            var memberExecute = member.Execute(configurationCube);
            if (memberExecute is SetItem setItem)
            {
                items.AddRange(setItem.Members);
            }
            else
            {
                items.Add(memberExecute);
            }
        }

        return new SetItem(items);
    }
}