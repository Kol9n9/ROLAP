using ROLAP.Common.Enums;

namespace ROLAP.Common.Model;

public class CubeQuery
{
    public IEnumerable<CubeItemTuple> Sets { get; }

    public QueryType QueryType { get; }
    
    public CubeQuery(QueryType type, IEnumerable<CubeItemTuple> sets)
    {
        QueryType = type;
        Sets = sets;
    }
}