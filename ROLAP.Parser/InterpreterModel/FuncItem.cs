using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class FuncItem : TupleItem
    {
        public string Name { get; set; }

        public override List<TupleItem> Run()
        {
            switch (Name.ToLower())
            {
                case "crossjoin":
                    {
                        CROSSJOIN();
                        break;
                    }
            }
            return Items;
        }
        private void CROSSJOIN()
        {

            List<TupleItem> firstTuple = GetAllMembers(Items[0].Run());
            List<TupleItem> secondTuple = GetAllMembers(Items[1].Run());

            List<TupleItem> result = new List<TupleItem>();
            foreach (var item1 in firstTuple)
            {
                foreach(var item2 in secondTuple)
                {
                    TupleItem newTuple = new TupleItem();
                    newTuple.Items.Add(item1);
                    newTuple.Items.Add(item2);
                    result.Add(newTuple);
                }
            }

            Items = result;
        }
        private List<TupleItem> GetAllMembers(List<TupleItem> tuples)
        {
            List<TupleItem> allMembers = new List<TupleItem>();
            foreach (var tuple in tuples)
            {
                allMembers.AddRange(tuple.Run());
            }
            return allMembers;
        }
    }
}
