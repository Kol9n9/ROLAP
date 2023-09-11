using ROLAP.Parser.Models.ExpressionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROLAP.Parser.Models.CubeRequest;

namespace ROLAP.Parser.Models.Expressions
{
    public class SetFunctionExpression : IExpression
    {
        private readonly string _name;
        private readonly List<IExpression> _arguments;
        public SetFunctionExpression(string name, List<IExpression> arguments)
        {
            _name = name;
            _arguments = arguments;
        }

        public IExpressionValue Eval()
        {
            switch(_name.ToLower()) {
                case "crossjoin": return CrossJoin();
                default: return null;
            }
            // Return new Set
        }
        private IExpressionValue CrossJoin()
        {
            var set1 = Helpers.MapToSet(_arguments[0].Eval());
            var set2 = Helpers.MapToSet(_arguments[1].Eval());
            
            
            List<CubeRequestAxisTuple> tuples = new List<CubeRequestAxisTuple>();
            
            foreach (var tuple1 in set1.Tuples)
            {
                foreach (var tuple2 in set2.Tuples)
                {
                    var tuple = new CubeRequestAxisTuple(new List<CubeRequestAxisMember>());
                    tuple.Members.AddRange(tuple1.Members);
                    tuple.Members.AddRange(tuple2.Members);
                    tuples.Add(tuple);
                }
            }
            
            return new CubeRequestAxisSet(tuples);
        }
    }
}
