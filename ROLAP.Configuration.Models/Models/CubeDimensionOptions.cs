using ROLAP.Configuration.Models.Enums;
using ROLAP.Configuration.Models.Interfaces;

namespace ROLAP.Configuration.Models.Models;

public abstract class CubeDimensionOptions : CubeBaseItemOptions
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string GroupKey { get; set; }
    public IEnumerable<CubeDimensionOptions> Values { get; set; }
}