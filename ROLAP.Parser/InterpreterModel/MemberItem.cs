using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class MemberItem : TupleItem
    {
        public List<string> Hierarchy { get; set; } = new List<string>();
        public string FuncName { get; set; }

        public override List<TupleItem> Run()
        {
            return new List<TupleItem> { this as TupleItem };
        }
    }
}
