using ROLAP.Common.Model.Models;
using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Parser.Models.Expressions
{
    internal class SetFunctionExpression : IExpression
    {
        private readonly string _name;
        private readonly List<IExpression> _arguments;
        public SetFunctionExpression(string name, List<IExpression> arguments)
        {
            _name = name;
            _arguments = arguments;
        }

        public ICubeQueryNode Eval(List<ICubeMeta> cubeMeta)
        {
            switch(_name.ToLower()) {
                case "crossjoin": return CrossJoin(cubeMeta);
                default: return null;
            }
            // Return new Set
        }
        private ICubeQueryNode CrossJoin(List<ICubeMeta> cubeMeta)
        {
            var set1 = Helpers.MapToSet(_arguments[0].Eval(cubeMeta));
            var set2 = Helpers.MapToSet(_arguments[1].Eval(cubeMeta));

            if (set1 == null) set1 = new CubeQuerySet();
            if (set2 == null) set2 = new CubeQuerySet();
            
            List<CubeQueryTuple> tuples = new List<CubeQueryTuple>();
            
            foreach (var tuple1 in set1.Tuples)
            {
                foreach (var tuple2 in set2.Tuples)
                {
                    var tuple = new CubeQueryTuple();
                    tuple.Members.AddRange(tuple1.Members);
                    tuple.Members.AddRange(tuple2.Members);
                    tuples.Add(tuple);
                }
            }

            return new CubeQuerySet
            {
                Tuples = tuples
            };
        }
    }
}
