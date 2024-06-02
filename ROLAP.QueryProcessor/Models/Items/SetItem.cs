using ROLAP.Common.Model;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal class SetItem : IQueryItem
{
    public IEnumerable<IQueryItem> Members { get; }
    public SetItem(IEnumerable<IQueryItem> members)
    {
        Members = members;
    }

    public IQueryItem Execute(ConfigurationCube configurationCube)
    {
        List<IQueryItem> items = new List<IQueryItem>();

        foreach (var member in Members)
        {
            items.Add(member.Execute(configurationCube));
        }

        return new SetItem(items);
    }
}