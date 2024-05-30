namespace ROLAP.Common.Model;

/// <summary>
/// Куб
/// </summary>
public class Cube
{

    /// <summary>
    /// Измерения
    /// </summary>
    public IEnumerable<CubeItem> Dimensions { get; private set; } = new List<CubeItem>();
    
    /// <summary>
    /// Меры
    /// </summary>
    public IEnumerable<CubeItem> Measures { get; private set; } = new List<CubeItem>();


    public Cube(IEnumerable<CubeItem> dimensions, IEnumerable<CubeItem> measures)
    {
        Dimensions = dimensions;
        Measures = measures;
    }
}