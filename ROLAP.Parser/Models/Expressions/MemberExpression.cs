using ROLAP.Common.Model.Models;
using System.Runtime.CompilerServices;
using ROLAP.Common.Model.Helpers;
using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Parser.Models.Expressions
{
    internal class MemberExpression : IExpression
    {
        private readonly CubeQueryMember _value;
        public MemberExpression(CubeQueryMember value)
        {
            _value = value;
        }

        public ICubeQueryNode Eval(List<ICubeMeta>  cubeMeta)
        {
            if (_value.Names[0].ToLower() == "measure")
            {
                if (MetaHelper.GetMeasureMeta(cubeMeta.OfType<CubeMeasureMeta>().ToList(), _value.Names) == null) return null;
                _value.Type = CubeMemberType.Measure;
            }
            else
            {
                if (MetaHelper.GetDimensionWithValueMeta(cubeMeta.OfType<CubeDimensionMeta>().ToList(), _value.Names) == null) return null;
                _value.Type = CubeMemberType.Dimension;
            }
            return _value;
        }
    }
}
