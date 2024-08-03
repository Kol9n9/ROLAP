using ROLAP.Core.Models.Enums;

namespace ROLAP.Core.Models.Model.Query;

public class CubeQuery
{
    public IEnumerable<CubeItemSet> Sets { get; }
    
    public CubeItemSet? Where { get; set; }

    public QueryType QueryType { get; }
    
    public CubeQuery(QueryType type, IEnumerable<CubeItemSet> sets, CubeItemSet? where)
    {
        QueryType = type;
        Sets = sets;
        Where = where;
    }
}