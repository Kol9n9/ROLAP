using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal abstract class FunctionItem : IQueryItem
{
    private IEnumerable<IQueryItem> _args;

    public FunctionItem(IEnumerable<IQueryItem> args)
    {
        _args = args;
    }

    protected abstract IQueryItem Run(ICubeConfiguration copyConfigurationCubeCopy, IEnumerable<IQueryItem> args);

    public IQueryItem Execute(ICubeConfiguration copyConfigurationCubeCopy) => Run(copyConfigurationCubeCopy, _args);
}