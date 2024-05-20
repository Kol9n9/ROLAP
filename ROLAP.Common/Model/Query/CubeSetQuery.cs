using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public class CubeSetQuery : ICubeQueryItem
{
    public IEnumerable<ICubeQueryItem> Members { get; }
    public CubeSetQuery(IEnumerable<ICubeQueryItem> members)
    {
        Members = members;
    }

    public ICubeQueryItem Execute(Cube cube)
    {
        List<ICubeQueryItem> items = new List<ICubeQueryItem>();

        foreach (var member in Members)
        {
            items.Add(member.Execute(cube));
        }

        return new CubeSetQuery(items);
    }
}