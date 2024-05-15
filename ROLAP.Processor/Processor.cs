using ROLAP.Configuration.Models.Interfaces;
using ROLAP.Processor.Interfaces;

namespace ROLAP.Processor;

public class Processor : IProcessor
{
    private ICubeConfigurationStore _configurationStore;

    public Processor(ICubeConfigurationStore store)
    {
        _configurationStore = store;
    }
    public Task ProcessQuery(string query)
    {
        var conf = _configurationStore.GetByName("TEST");
        return Task.CompletedTask;
    }
}