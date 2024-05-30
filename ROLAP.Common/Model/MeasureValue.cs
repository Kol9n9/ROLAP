namespace ROLAP.Common.Model;

public class MeasureValue
{
    public string MeasureKey { get; set; }
    public string Id { get; set; }
    public string Value { get; set; }
    
    public IEnumerable<CubeItem> Dimensions { get; set; }
}