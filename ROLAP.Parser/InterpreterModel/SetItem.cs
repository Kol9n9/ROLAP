using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class SetItem : IInterpreterItem
    {
        public List<IInterpreterItem> Tuples { get;} = new List<IInterpreterItem>();

        public List<IInterpreterItem> Run()
        {
            List<IInterpreterItem> result = new List<IInterpreterItem>();

            foreach (var item in Tuples)
            {
                result.AddRange(item.Run());
            }

            return result;
        }
    }
}
