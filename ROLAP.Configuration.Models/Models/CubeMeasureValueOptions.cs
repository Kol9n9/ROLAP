namespace ROLAP.Configuration.Models.Models;

public abstract class CubeMeasureValueOptions : CubeBaseItemOptions
{
    public string Id { get; set; }
    public string Value { get; set; }
    
    public IEnumerable<CubeDimensionOptions> Dimensions { get; set; }
}