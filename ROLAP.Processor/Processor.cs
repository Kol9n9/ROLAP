using System.Text;
using Newtonsoft.Json;
using ROLAP.Common.Enums;
using ROLAP.Common.Model.CubeResult;
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
               var cube = _selectProcessor.ExecuteQuery(cubeQuery);
               WriteToFile(cube);
                break;
            }
        }
        return Task.CompletedTask;
    }


    private void WriteToFile(CubeResult cubeResult)
    {
        var json = JsonConvert.SerializeObject(cubeResult);
        using var stream = new FileStream("res.txt",FileMode.Create);
        stream.Write(Encoding.UTF8.GetBytes(json));
        stream.Close();
    }
}