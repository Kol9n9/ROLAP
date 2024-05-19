using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public class CubeTupleQuery : ICubeQueryItem
{
    public IEnumerable<ICubeQueryItem> Items { get; }

    public CubeTupleQuery(IEnumerable<ICubeQueryItem> items)
    {
        Items = items;
    }
}