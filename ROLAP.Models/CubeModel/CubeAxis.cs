using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Model.CubeModel
{
    public class CubeAxis
    {
        public List<CubeAxisTuple> Tuples { get; set;} = new List<CubeAxisTuple>();
        public void AddTuple(CubeAxisTuple tuple)
        {
            Tuples.Add(tuple);
        }
    }
}
