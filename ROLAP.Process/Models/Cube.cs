using ROLAP.Common.Model.Models;

namespace ROLAP.Process.Models
{
    internal class Cube
    {
        public List<CubeAxis> Axes { get; set; } = new List<CubeAxis>();
        public List<CubeValue> Values { get; set; } = new List<CubeValue>();
    }
    internal class CubeAxis
    {
        public List<CubeTuple> Tuples { get; set; } = new List<CubeTuple>();
    }
    internal class CubeTuple
    {
        public List<CubeMember> Members { get; set; } = new List<CubeMember>();
    }
    internal class CubeMember
    {
        public string Name { get; set; }
    }
    internal class CubeValue
    {
        public double Value { get; set; }
        public string FmtValue { get; set; }
    }
}
