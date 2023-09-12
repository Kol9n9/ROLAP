using ROLAP.Common.Model.Models;

namespace ROLAP.Parser.Models.Expressions
{
    internal class MemberExpression : IExpression
    {
        private readonly CubeQueryMember _value;
        public MemberExpression(CubeQueryMember value)
        {
            _value = value;
        }

        public ICubeQueryNode Eval()
        {
            return _value;
        }
    }
}
