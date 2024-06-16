using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal abstract class FunctionItem : IQueryItem
{
    private IEnumerable<IQueryItem> _args;

    public FunctionItem(IEnumerable<IQueryItem> args)
    {
        _args = args;
    }

    protected abstract IQueryItem Run(CubeConfiguration configurationCube, IEnumerable<IQueryItem> args);

    public IQueryItem Execute(CubeConfiguration configurationCube) => Run(configurationCube, _args);
}