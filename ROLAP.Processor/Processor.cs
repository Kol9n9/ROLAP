using ROLAP.Common.Enums;
using ROLAP.Configuration.Models.Interfaces;
using ROLAP.Parser;
using ROLAP.Processor.Interfaces;
using ROLAP.Processor.QueryProcessors;

namespace ROLAP.Processor;

public class Processor : IProcessor
{
    private ICubeConfigurationStore _configurationStore;
    private SelectProcessor _selectProcessor;

    public Processor(ICubeConfigurationStore store)
    {
        _configurationStore = store;
        _selectProcessor = new SelectProcessor(_configurationStore);
    }
    public Task ProcessQuery(string query)
    {
        var cubeQuery = QueryParser.Parse(query);
        switch (cubeQuery.QueryType)
        {
            case QueryType.SELECT:
            {
                _selectProcessor.ExecuteQuery(cubeQuery);
                break;
            }
        }
        return Task.CompletedTask;
    }
}