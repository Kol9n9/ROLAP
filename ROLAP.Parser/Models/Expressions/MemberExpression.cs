using ROLAP.Common.Model.Models;
using System.Runtime.CompilerServices;

namespace ROLAP.Parser.Models.Expressions
{
    internal class MemberExpression : IExpression
    {
        private readonly CubeQueryMember _value;
        public MemberExpression(CubeQueryMember value)
        {
            _value = value;
        }

        public ICubeQueryNode Eval(CubeMeta cubeMeta)
        {
            if (_value.Names[0].ToLower() == "measure")
            {
                string measureName = _value.Names[1];

                var measure = cubeMeta.Measures.FirstOrDefault(x =>
                    measureName[0] == '&'
                        ? x.Measure.Key == new String(measureName.Skip(1).ToArray())
                        : x.Measure.Name == measureName);
                if (measure == null) return null;
                _value.Names[1] = measure.Measure.Key;
                _value.Type = CubeMemberType.Measure;
            }
            else
            {
                string dimensionGroupName = _value.Names[0];
                string dimensionName = _value.Names[1];

                var dimension = cubeMeta.Dimensions.FirstOrDefault(x => x.Dimension.Name == dimensionGroupName && x.Values.FirstOrDefault(y =>
                    dimensionName[0] == '&' ? y.Key == new String(dimensionName.Skip(1).ToArray()) : y.Name == dimensionName
                ) != null);
                if (dimension == null) return null;

                _value.Names[1] = dimension.Values.FirstOrDefault(x =>
                        dimensionName[0] == '&' ? x.Key == new String(dimensionName.Skip(1).ToArray()) : x.Name == dimensionName).Key;
                
                _value.Type = CubeMemberType.Dimension;
            }
            return _value;
        }
    }
}
