using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class TupleItem : IInterpreterItem
    {
        public List<IInterpreterItem> Items { get; } = new List<IInterpreterItem>();
    }
}
