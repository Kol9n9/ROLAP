using ROLAP.Common.Model.Models.CubeRequest;

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

        internal List<CubeAxisTupleRequest> GetAxisTuplesRequest()
        {
            List<CubeAxisTupleRequest> request = new List<CubeAxisTupleRequest>();

            foreach (var tuple in Tuples)
            {
                request.Add(new CubeAxisTupleRequest()
                {
                    Members = tuple.GetCubeMemberRequest()
                });
            }

            return request;
        }
    }
}
