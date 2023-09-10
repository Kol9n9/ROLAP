using ROLAP.Parser.Models.ExpressionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.Expressions
{
    public interface IExpression
    {
        IExpressionValue Execute();
    }
} 
