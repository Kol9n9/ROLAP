using System.Text;
using Newtonsoft.Json;
using ROLAP.Common.Enums;
using ROLAP.Common.Interfaces;
using ROLAP.Configuration.Models.Interfaces;
using ROLAP.Parser;
using ROLAP.Process.Interfaces;
using ROLAP.Process.Models.Result;
using ROLAP.Process.QueryProcessors;

namespace ROLAP.Process;

public class Processor : IProcessor
{
    private SelectProcessor _selectProcessor;
    private IQueryProcessor _queryProcessor;

    public Processor(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
        _selectProcessor = new SelectProcessor();
    }
    public Task ProcessQuery(string query)
    {
        var cubeQuery = _queryProcessor.ProcessQuery(query);
        switch (cubeQuery.QueryType)
        {
            case QueryType.Select:
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