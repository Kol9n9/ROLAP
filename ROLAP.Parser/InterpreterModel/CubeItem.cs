using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class CubeItem : IInterpreterItem
    {
        public List<AxisItem> Axes { get; } = new List<AxisItem>();
        public string CubeName { get; set; }
    }
}
