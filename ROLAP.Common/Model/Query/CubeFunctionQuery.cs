using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model.Query;

public abstract class CubeFunctionQuery : ICubeQueryItem
{
    public IEnumerable<ICubeQueryItem> Args { get; }

    public CubeFunctionQuery(IEnumerable<ICubeQueryItem> args)
    {
        Args = args;
    }
}