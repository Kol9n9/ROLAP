using ROLAP.Model.CubeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class TupleItem
    {
        public List<TupleItem> Items { get; set; } = new List<TupleItem>();

        public virtual List<TupleItem> Run()
        {
            List<TupleItem> newTuples = new List<TupleItem>();
            foreach (var item in Items)
            {
                newTuples.AddRange(item.Run());
            }
            Items = newTuples;
            return Items;
        }

        internal virtual List<CubeMemberRequest> GetCubeMemberRequest()
        {
            List<CubeMemberRequest> members = new List<CubeMemberRequest>();

            foreach (var item in Items)
            {
                members.AddRange(item.GetCubeMemberRequest());
            }

            return members;
        }
    }
}
