using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class AxisItem : IInterpreterItem
    {
        public int AxisNumber { get; set; }
        public List<TupleItem> Tuples { get; } = new List<TupleItem>();
    }
}
