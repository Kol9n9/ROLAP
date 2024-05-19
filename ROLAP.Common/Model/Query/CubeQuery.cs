using ROLAP.Common.Enums;

namespace ROLAP.Common.Model.Query;

public class CubeQuery
{
    public QueryType QueryType { get; }
    
    public IEnumerable<CubeAxisQuery> Axes { get; }

    public string CubeName { get; }

    public CubeQuery(QueryType type, IEnumerable<CubeAxisQuery> axes, string cubeName)
    {
        QueryType = type;
        Axes = axes;
        CubeName = cubeName;
    }
}