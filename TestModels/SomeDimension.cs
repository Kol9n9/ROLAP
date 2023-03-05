using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    public class SomeDimension
    {
        public string Name { get; set; }
        public List<SomeDimensionValue> Values { get; set; }
    }
}
