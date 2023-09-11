using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.ExpressionValues
{
    public class TupleValue : IExpressionValue
    {
        private CubeRequestAxisTuple _value;
        public TupleValue(CubeRequestAxisTuple value)
        {
            _value = value;
        }
    }
}
