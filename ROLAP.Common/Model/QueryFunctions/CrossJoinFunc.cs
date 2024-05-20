using ROLAP.Common.Helpers;
using ROLAP.Common.Interfaces;
using ROLAP.Common.Model.Query;

namespace ROLAP.Common.Model.QueryFunctions;

public class CrossJoinFunc : CubeFunctionQuery
{
    public CrossJoinFunc(IEnumerable<ICubeQueryItem> args) : base(args)
    {
    }

    protected override ICubeQueryItem Run(Cube cube, IEnumerable<ICubeQueryItem> args)
    {
        return Union(cube,  args.ToList());
    }


    private CubeSetQuery Union(Cube cube, List<ICubeQueryItem> args)
    {
        var set1 = MappingHelper.ToSet(args[0].Execute(cube));
        var set2 = MappingHelper.ToSet(args[1].Execute(cube));

        List<CubeTupleQuery> tuples = new List<CubeTupleQuery>();

        foreach (var cubeQueryItem1 in set1.Members)
        {
            var tuple1 = MappingHelper.ToTuple(cubeQueryItem1);
            foreach (var cubeQueryItem2 in set2.Members)
            {
                var tuple2 = MappingHelper.ToTuple(cubeQueryItem2);
                List<CubeMemberQuery> members = new List<CubeMemberQuery>();
                members.AddRange(tuple1.Items.Cast<CubeMemberQuery>());
                members.AddRange(tuple2.Items.Cast<CubeMemberQuery>());
                
                tuples.Add(new CubeTupleQuery(members));
            }
        }

        return new CubeSetQuery(tuples);
    }
}