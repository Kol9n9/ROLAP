using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Model.Models
{
    public class CubeMeasure
    {
        public Guid Id { get; set; }
        public double Value { get; set; }
        public CubeDimension Dimension { get; set; }
    }
}
