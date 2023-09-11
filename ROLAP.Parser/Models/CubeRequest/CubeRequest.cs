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
    public class CubeRequestAxis : IExpressionValue
    {
        public CubeRequestAxisSet Set { get; }
        public int AxisNumber { get; }
        public CubeRequestAxis(int axisNumber, CubeRequestAxisSet set)
        {
            Set = set;
            AxisNumber = axisNumber;
        }
    }

    public class CubeRequestAxisSet : IExpressionValue
    {
        public List<CubeRequestAxisTuple> Tuples { get; }

        public CubeRequestAxisSet(List<CubeRequestAxisTuple> tuples)
        {
            Tuples = tuples;
        }
    }
    public class CubeRequestAxisTuple : IExpressionValue
    {
        public List<CubeRequestAxisMember> Members { get; }
        public CubeRequestAxisTuple(List<CubeRequestAxisMember> members)
        {
            Members = members;
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
    }
}
