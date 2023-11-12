using ROLAP.CubeConfiguration.Models.Dimension;
using ROLAP.CubeConfiguration.Models.Measure;

namespace ROLAP.CubeConfiguration.Models;

internal class CubeConfiguration
{
    /// <summary>
    /// Наименование конфигурации
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Меры
    /// </summary>
    public List<IMeasureConfiguration> MeasureConfigurations { get; set; }
    /// <summary>
    /// Группы измерений
    /// </summary>
    public List<IDimensionConfiguration> DimensionGroupConfigurations { get; set; }
}