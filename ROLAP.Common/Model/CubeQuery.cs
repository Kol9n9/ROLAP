using ROLAP.Common.Enums;

namespace ROLAP.Common.Model;

public class CubeQuery
{
    public IEnumerable<CubeItemTuple> Sets { get; }
    
    public IEnumerable<CubeItemTuple> Where { get; set; }

    public QueryType QueryType { get; }
    
    public CubeQuery(QueryType type, IEnumerable<CubeItemTuple> sets, IEnumerable<CubeItemTuple> where)
    {
        QueryType = type;
        Sets = sets;
        Where = where;
    }
}