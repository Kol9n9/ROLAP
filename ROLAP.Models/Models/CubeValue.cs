using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Model.Models
{
    public class CubeValue
    {
        public Guid Id { get; set; }
        public double Value { get; set; }
        public List<Guid> Dimensions { get; set; }
        public Guid MeasureId { get; set; } 
    }
}
