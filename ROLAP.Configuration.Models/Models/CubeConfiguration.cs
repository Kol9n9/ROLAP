using ROLAP.Common.Interfaces;

namespace ROLAP.Configuration.Models.Models;

public class CubeConfiguration
{
    public IEnumerable<CubeDimensionOptions> DimensionOptions { get; set; }
    public IEnumerable<CubeMeasureOptions> MeasureOptions { get; set; }
}