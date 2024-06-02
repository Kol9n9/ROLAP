using ROLAP.Common.Enums;
using ROLAP.QueryProcessor.Models.Items;

namespace ROLAP.QueryProcessor.Models;

internal class QueryModel
{
    public QueryType QueryType { get; }
    
    public IEnumerable<AxisItem> Axes { get; }

    public string CubeName { get; }

    public QueryModel(QueryType type, IEnumerable<AxisItem> axes, string cubeName)
    {
        QueryType = type;
        Axes = axes;
        CubeName = cubeName;
    }
}