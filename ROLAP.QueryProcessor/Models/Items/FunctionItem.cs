using ROLAP.Common.Model;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal abstract class FunctionItem : IQueryItem
{
    private IEnumerable<IQueryItem> _args;

    public FunctionItem(IEnumerable<IQueryItem> args)
    {
        _args = args;
    }

    protected abstract IQueryItem Run(ConfigurationCube configurationCube, IEnumerable<IQueryItem> args);

    public IQueryItem Execute(ConfigurationCube configurationCube) => Run(configurationCube, _args);
}