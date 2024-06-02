using ROLAP.Common.Model;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal class TupleItem : IQueryItem
{
    public IEnumerable<IQueryItem> Items { get; }

    public TupleItem(IEnumerable<IQueryItem> items)
    {
        Items = items;
    }

    public IQueryItem Execute(ConfigurationCube configurationCube)
    {
        List<IQueryItem> items = new List<IQueryItem>();

        foreach (var item in Items)
        {
            items.Add(item.Execute(configurationCube));
        }
        
        return new TupleItem(items);
    }
}