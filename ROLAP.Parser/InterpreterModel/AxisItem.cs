using ROLAP.Model.CubeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class AxisItem
    {
        public int AxisNumber { get; set; }
        public List<TupleItem> Tuples = new List<TupleItem>();

        public void Run()
        {
            List<TupleItem> newTuples = new List<TupleItem>();
            foreach (var tuple in Tuples)
            {
                newTuples.AddRange(tuple.Run());
            }
            Tuples = newTuples;
        }

        internal CubeAxisRequest GetAxisRequest()
        {
            CubeAxisRequest request = new CubeAxisRequest();

            foreach (var tuple in Tuples)
            {
                request.Tuples.Add(tuple.GetAxisTupleRequest());
            }

            return request;
        }
    }
}
