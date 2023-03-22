using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class FunctionItem : IInterpreterItem
    {
        public string Name { get; set; }
        public List<IInterpreterItem> Arguments { get; } = new List<IInterpreterItem>();
    }
}
