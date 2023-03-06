using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Model.CubeModel
{
    public class CubeAxis
    {
        public List<CubeAxisMember> Members { get; set;} = new List<CubeAxisMember>();
        public void AddMember(CubeAxisMember member)
        {
            Members.Add(member);
        }
    }
}
