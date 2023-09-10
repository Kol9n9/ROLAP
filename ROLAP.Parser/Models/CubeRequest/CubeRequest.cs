using ROLAP.Parser.Models.ExpressionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.CubeRequest
{
    public class CubeRequest
    {
        public string CubeName { get; }
        public List<CubeRequestAxis> Axes { get; }
        public CubeRequest(string cubeName)
        {
            CubeName = cubeName;
            Axes = new List<CubeRequestAxis>();
        }
    }
    public class CubeRequestAxis
    {
        public List<CubeRequestAxisTuple> Tuples { get; }
        public int AxisNumber { get; }
        public CubeRequestAxis(int axisNumber)
        {
            Tuples = new List<CubeRequestAxisTuple>();
            AxisNumber = axisNumber;
        }
    }
    public class CubeRequestAxisTuple : IExpressionValue
    {
        public List<IExpressionValue> Members { get; }
        public CubeRequestAxisTuple()
        {
            Members = new List<IExpressionValue> ();
        }

        public object Raw()
        {
            return Members;
        }
    }
    public class CubeRequestAxisMember : IExpressionValue
    {
        public string DimensionName { get; }
        public List<string> Hierarchy { get; }
        public CubeRequestAxisMember(string dimensionName, List<string> hierarchy)
        {
            DimensionName = dimensionName;
            Hierarchy = hierarchy;
        }

        public object Raw()
        {
            return this;
        }
    }
}
