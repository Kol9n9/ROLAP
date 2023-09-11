using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.ExpressionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.Expressions
{
    internal class MemberExpression : IExpression
    {
        private readonly CubeRequestAxisMember _value;
        public MemberExpression(CubeRequestAxisMember value)
        {
            _value = value;
        }

        public IExpressionValue Eval()
        {
            return _value;
        }
    }
}
