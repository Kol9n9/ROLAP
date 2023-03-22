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

        public List<IInterpreterItem> Run()
        {
            List<IInterpreterItem> args = new List<IInterpreterItem>();
            foreach (var arg in Arguments)
            {
                args.AddRange(arg.Run());
            }

            return CrossJoinFunction((TupleItem)args[0], (TupleItem)args[1]);
            
        }
        private static List<IInterpreterItem> CrossJoinFunction(TupleItem tuple1, TupleItem tuple2)
        {
            List<IInterpreterItem> result = new List<IInterpreterItem>();

            foreach (MemberItem item1 in tuple1.Items) {
                foreach (MemberItem item2 in tuple2.Items)
                {
                    TupleItem tupleItem = new TupleItem();
                    tupleItem.Items.Add(item1);
                    tupleItem.Items.Add(item2);
                    result.Add(tupleItem);
                }
            }

            return result;
        }

    }
}
