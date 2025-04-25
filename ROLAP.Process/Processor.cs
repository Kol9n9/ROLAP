using ROLAP.Core.Models.Enums;
using ROLAP.Formatter.Formatters;
using ROLAP.Process.Interfaces;
using ROLAP.Process.QueryProcessors;

namespace ROLAP.Process;

public class Processor : IProcessor
{
    private SelectProcessor _selectProcessor;
    private QueryProcessor.QueryProcessor _queryProcessor;

    public Processor(QueryProcessor.QueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
        _selectProcessor = new SelectProcessor();
    }
    public Task<string> ProcessQuery(string query)
    {
        var cubeQuery = _queryProcessor.ProcessQuery(query);
        switch (cubeQuery.QueryType)
        {
            case QueryType.Select:
            { 
                var cube = _selectProcessor.ExecuteQuery(cubeQuery);
                return Task.FromResult(JsonFormatter.Format(cube.Axes,cube.Values, cube.IsAggregated));
            }
            default:
            {
                throw new NotSupportedException();
            }
        }
    }
}