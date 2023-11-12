using ROLAP.Common.Model.Models;
using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Parser.Models.Expressions
{
    internal interface IExpression
    {
        ICubeQueryNode Eval(List<ICubeMeta> cubeMeta);
    }
} 
