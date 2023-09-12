using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Common.Model.Models
{
    public class CubeSettings
    {
        public string Name { get; set; }
        public List<CubeSetting> Settings { get; set; } = new List<CubeSetting>();
    }
    public class CubeSetting
    {

    }
}
