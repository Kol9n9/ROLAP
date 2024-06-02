using ROLAP.Common.Model;
using ROLAP.QueryProcessor.Helpers;
using ROLAP.QueryProcessor.Interfaces;
using ROLAP.QueryProcessor.Models.Items;

namespace ROLAP.QueryProcessor.Models.Functions;

internal class CrossJoinFunc : FunctionItem
{
    public CrossJoinFunc(IEnumerable<IQueryItem> args) : base(args)
    {
    }

    protected override IQueryItem Run(ConfigurationCube configurationCube, IEnumerable<IQueryItem> args)
    {
        return Union(configurationCube,  args.ToList());
    }


    private SetItem Union(ConfigurationCube configurationCube, List<IQueryItem> args)
    {
        var set1 = MappingHelper.ToSet(args[0].Execute(configurationCube));
        var set2 = MappingHelper.ToSet(args[1].Execute(configurationCube));

        List<TupleItem> tuples = new List<TupleItem>();

        foreach (var cubeQueryItem1 in set1.Members)
        {
            var tuple1 = MappingHelper.ToTuple(cubeQueryItem1);
            foreach (var cubeQueryItem2 in set2.Members)
            {
                var tuple2 = MappingHelper.ToTuple(cubeQueryItem2);
                List<MemberItem> members = new List<MemberItem>();
                members.AddRange(tuple1.Items.Cast<MemberItem>());
                members.AddRange(tuple2.Items.Cast<MemberItem>());
                
                tuples.Add(new TupleItem(members));
            }
        }

        return new SetItem(tuples);
    }
}