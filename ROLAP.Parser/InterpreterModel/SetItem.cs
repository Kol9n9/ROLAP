using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class SetItem : IInterpreterItem
    {
        public List<IInterpreterItem> Tuples { get;}
    }
}
