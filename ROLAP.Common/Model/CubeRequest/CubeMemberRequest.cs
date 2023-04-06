using ROLAP.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Model.CubeRequest
{
    public class CubeMemberRequest
    {
        public Guid Id { get; set; }
        public CubeMemberType Type { get; set; }
    }
}
