namespace ROLAP.Common.Model;

/// <summary>
/// Куб
/// </summary>
public class Cube
{
    /// <summary>
    /// Измерения
    /// </summary>
    public List<Dimension> Dimensions { get; private set; } = new List<Dimension>();
    
    /// <summary>
    /// Меры
    /// </summary>
    public List<Measure> Measures { get; private set; } = new List<Measure>();

    /// <summary>
    /// Добавить измерение
    /// </summary>
    /// <param name="dimension">Добавляемое <see cref="Dimension">измерение</see></param>
    public void AddDimension(Dimension dimension) => Dimensions.Add(dimension);
    
    /// <summary>
    /// Добавить меру
    /// </summary>
    /// <param name="measure">Добавляемая <see cref="Dimension">мера</see></param>
    public void AddMeasure(Measure measure) => Measures.Add(measure);
    
    
    /// <summary>
    /// Добавить измерения
    /// </summary>
    /// <param name="dimensions">Добавляемые <see cref="Dimension">измерения</see></param>
    public void AddDimensions(List<Dimension> dimensions) => Dimensions.AddRange(dimensions);
    
    /// <summary>
    /// Добавить меры
    /// </summary>
    /// <param name="measures">Добавляемые <see cref="Dimension">меры</see></param>
    public void AddMeasures(List<Measure> measures) => Measures.AddRange(measures);

    public override bool Equals(object? obj)
    {
        if (obj is Cube cube)
        {
            if (cube.Dimensions.Count != Dimensions.Count) return false;
            if (cube.Measures.Count != Measures.Count) return false;
            for (int i = 0; i < cube.Dimensions.Count; i++)
            {
                if (!cube.Dimensions[i].Equals(Dimensions[i])) return false;
            }
            for (int i = 0; i < cube.Measures.Count; i++)
            {
                if (!cube.Measures[i].Equals(Measures[i])) return false;
            }
            return true;
        }
        return false;
    }
}