using ROLAP.Parser.Models.CubeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.ExpressionValues
{
    internal class MemberValue : IExpressionValue
    {
        private readonly CubeRequestAxisMember _value;
        public MemberValue(CubeRequestAxisMember value)
        {
            _value = value;
        }
    }
}
    