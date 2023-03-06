using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Model.CubeModel
{
    public class Cube
    {
        public List<CubeAxis> Axes { get; set; } = new List<CubeAxis>();
        public List<CubeValue> Values { get; set; } = new List<CubeValue> { };
        public void AddAxis(CubeAxis axis)
        {
            Axes.Add(axis);
        }

    }
}
