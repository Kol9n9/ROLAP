using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public abstract class CubeFunctionQuery : ICubeQueryItem
{
    private IEnumerable<ICubeQueryItem> _args;

    public CubeFunctionQuery(IEnumerable<ICubeQueryItem> args)
    {
        _args = args;
    }

    protected abstract ICubeQueryItem Run(Cube cube, IEnumerable<ICubeQueryItem> args);

    public ICubeQueryItem Execute(Cube cube) => Run(cube, _args);
}