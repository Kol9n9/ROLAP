using ROLAP.Models.Models.ICubeItems;
using ROLAP.QueryProcessor.Interfaces;

namespace ROLAP.QueryProcessor.Models.Items;

internal abstract class FunctionItem : IQueryItem
{
    private IEnumerable<IQueryItem> _args;

    public FunctionItem(IEnumerable<IQueryItem> args)
    {
        _args = args;
    }

    protected abstract IQueryItem Run(CubeConfiguration copyConfigurationCubeCopy, IEnumerable<IQueryItem> args);

    public IQueryItem Execute(CubeConfiguration copyConfigurationCubeCopy) => Run(copyConfigurationCubeCopy, _args);
}