using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public class CubeTupleQuery : ICubeQueryItem
{
    public IEnumerable<ICubeQueryItem> Items { get; }

    public CubeTupleQuery(IEnumerable<ICubeQueryItem> items)
    {
        Items = items;
    }

    public ICubeQueryItem Execute(Cube cube)
    {
        List<ICubeQueryItem> items = new List<ICubeQueryItem>();

        foreach (var item in Items)
        {
            items.Add(item.Execute(cube));
        }
        
        return new CubeTupleQuery(items);
    }
}