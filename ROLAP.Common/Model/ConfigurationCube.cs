namespace ROLAP.Common.Model;

/// <summary>
/// Куб
/// </summary>
public class ConfigurationCube
{
    /// <summary>
    /// Измерения
    /// </summary>
    public IEnumerable<CubeItem> Dimensions { get; }
    
    /// <summary>
    /// Меры
    /// </summary>
    public IEnumerable<CubeItem> Measures { get; }


    public ConfigurationCube(IEnumerable<CubeItem> dimensions, IEnumerable<CubeItem> measures)
    {
        Dimensions = dimensions;
        Measures = measures;
    }
}