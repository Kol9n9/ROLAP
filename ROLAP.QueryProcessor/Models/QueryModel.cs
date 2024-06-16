using ROLAP.Core.Models.Enums;
using ROLAP.QueryProcessor.Models.Items;

namespace ROLAP.QueryProcessor.Models;

internal class QueryModel
{
    public QueryType QueryType { get; }
    
    public IEnumerable<AxisItem> Axes { get; }

    public IEnumerable<AxisItem> Where { get; }

    public string CubeName { get; }

    public QueryModel(string cubeName, QueryType type, IEnumerable<AxisItem> axes, IEnumerable<AxisItem> where)
    {
        CubeName = cubeName;
        QueryType = type;
        Axes = axes;
        Where = where;
    }
}