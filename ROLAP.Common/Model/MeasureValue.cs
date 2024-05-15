namespace ROLAP.Common.Model;

public class MeasureValue
{
    public string Id { get; set; }
    public string Value { get; set; }
    
    public IEnumerable<Dimension> Dimensions { get; set; }
}