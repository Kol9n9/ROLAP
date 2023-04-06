using ROLAP.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Model.CubeModel
{
    public class CubeAxisMember
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CubeMemberType2 Type { get; set; }
    }
}
