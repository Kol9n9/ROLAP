using ROLAP.Parser.Models.CubeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.ExpressionValues
{
    internal class DimensionMemberValue : IExpressionValue
    {
        private readonly CubeRequestAxisMember _value;
        public DimensionMemberValue(CubeRequestAxisMember value)
        {
            _value = value;
        }

        public object Raw()
        {
            return _value;
        }
    }
}
    