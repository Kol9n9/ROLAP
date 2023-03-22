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

        public List<IInterpreterItem> Run()
        {
            TupleItem result = new TupleItem();

            foreach (var item in Items)
            {
                result.Items.AddRange(item.Run());
            }

            return new List<IInterpreterItem>() { result };
        }
    }
}
