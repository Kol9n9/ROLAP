using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public class CubeSetQuery : ICubeQueryItem
{
    public IEnumerable<ICubeQueryItem> Members { get; }
    public CubeSetQuery(IEnumerable<ICubeQueryItem> members)
    {
        Members = members;
    }
}