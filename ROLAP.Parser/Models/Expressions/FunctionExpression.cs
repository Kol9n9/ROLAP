using ROLAP.Parser.Models.ExpressionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.Expressions
{
    public class FunctionExpression : IExpression
    {
        private readonly string _name;
        private readonly List<IExpression> _arguments;
        public FunctionExpression(string name, List<IExpression> arguments)
        {
            _name = name;
            _arguments = arguments;
        }

        public IExpressionValue Execute()
        {
            switch(_name.ToLower()) {
                case "crossjoin": return CrossJoin();
                default: return null;
            }
        }
        private IExpression CrossJoin()
        {

        }
    }
}
