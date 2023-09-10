using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.ExpressionValues
{
    public class TupleExpression : IExpression
    {
        private List<IExpression> _expressions;
        public TupleExpression(List<IExpression> expressions)
        {
            _expressions = expressions;
        }

        public IExpressionValue Execute()
        {
            CubeRequestAxisTuple tuple = new CubeRequestAxisTuple();
            foreach (IExpression expression in _expressions)
            {
                tuple.Members.Add(expression.Execute());
            }
            return tuple;
        }
    }
}
