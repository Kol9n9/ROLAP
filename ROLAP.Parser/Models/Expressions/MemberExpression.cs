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

        public ICubeQueryNode Eval(CubeMeta cubeMeta)
        {
            if (_value.Names[0].ToLower() == "measure")
            {
                string measureName = _value.Names[1];

                var measure = cubeMeta.Measures.FirstOrDefault(x =>
                    measureName[0] == '&'
                        ? x.Measure.Key == measureName.Skip(1).ToString()
                        : x.Measure.Name == measureName);
                if (measure == null) return null;
                _value.Names[1] = measure.Measure.Key;
                _value.Type = CubeMemberType.Measure;
            }
            else
            {
                string dimensionGroupName = _value.Names[0];
                string dimensionName = _value.Names[1];

                var dimension = cubeMeta.Measures.FirstOrDefault(x =>
                    x.Dimensions.FirstOrDefault(
                        y => y.Dimension.Name == dimensionGroupName && y.Values.FirstOrDefault(z =>
                            dimensionName[0] == '&' ? z.Key == dimensionName.Skip(1).ToString() : z.Name == dimensionName) != null)
                    != null);
                if (dimension == null) return null;

                _value.Names[1] = dimension.Dimensions.FirstOrDefault(x => x.Dimension.Name == dimensionGroupName)
                    .Values.FirstOrDefault(x =>
                        dimensionName[0] == '&' ? x.Key == dimensionName.Skip(1).ToString() : x.Name == dimensionName).Key;
                
                
                _value.Type = CubeMemberType.Dimension;
            }
            return _value;
        }
    }
}
