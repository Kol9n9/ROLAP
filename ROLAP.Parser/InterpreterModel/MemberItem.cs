using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class MemberItem : IInterpreterItem
    {
        public List<string> Hierarchy { get; } = new List<string>();
        public string FunctionName { get; set; }

        public List<IInterpreterItem> Run()
        {
            return new List<IInterpreterItem>() { this };
        }
    }
}
