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
        private readonly DimensionMemberValue _value;
        public MemberExpression(DimensionMemberValue value)
        {
            _value = value;
        }

        IExpressionValue IExpression.Execute()
        {
            return _value;
        }
    }
}
