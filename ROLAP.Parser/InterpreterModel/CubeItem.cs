using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class CubeItem : IInterpreterItem
    {
        public List<AxisItem> Axes { get; } = new List<AxisItem>();
        public string CubeName { get; set; }

        public List<IInterpreterItem> Run()
        {
            List<IInterpreterItem> result = new List<IInterpreterItem>();

            foreach (var item in Axes)
            {
                result.AddRange(item.Run());
            }

            return result;
        }
    }
}
