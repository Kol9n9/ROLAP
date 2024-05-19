using ROLAP.Common.Interfaces;
using ROLAP.Common.Model.Query;

namespace ROLAP.Common.Model.QueryFunctions;

public class CrossJoinFunc : CubeFunctionQuery
{
    public CrossJoinFunc(IEnumerable<ICubeQueryItem> args) : base(args)
    {
    }
}