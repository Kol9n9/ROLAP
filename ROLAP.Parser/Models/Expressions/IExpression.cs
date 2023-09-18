using ROLAP.Common.Model.Models;

namespace ROLAP.Parser.Models.Expressions
{
    internal interface IExpression
    {
        ICubeQueryNode Eval(CubeMeta cubeMeta);
    }
} 
