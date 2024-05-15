using ROLAP.Configuration.Models.Enums;
using ROLAP.Configuration.Models.Interfaces;

namespace ROLAP.Configuration.Models.Models;

public abstract class CubeMeasureOptions : CubeBaseItemOptions
{
    public string Key { get; set; }
    public string Name { get; set; }
    
    public IEnumerable<CubeMeasureValueOptions> Values { get; set; }
}