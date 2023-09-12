using ROLAP.Common.Model.Models;

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

        public ICubeQueryNode Eval()
        {
            switch(_name.ToLower()) {
                case "crossjoin": return CrossJoin();
                default: return null;
            }
            // Return new Set
        }
        private ICubeQueryNode CrossJoin()
        {
            var set1 = Helpers.MapToSet(_arguments[0].Eval());
            var set2 = Helpers.MapToSet(_arguments[1].Eval());
            
            
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
