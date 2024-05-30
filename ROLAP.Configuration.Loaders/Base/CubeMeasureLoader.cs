using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Static;
using ROLAP.Configuration.Models.Enums;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Base;

public class CubeMeasureLoader
{
    private readonly StaticCubeMeasureLoader _staticLoader = new StaticCubeMeasureLoader();
    
    public IEnumerable<CubeItem> Load(IEnumerable<CubeMeasureOptions> options, IEnumerable<CubeItem> dimensions)
    {
        List<CubeItem> measures = new List<CubeItem>();
        foreach (var option in options)
        {
            switch (option.GetSourceType())
            {
                case SourceType.Static:
                {
                    if (option is not StaticCubeMeasureOptions staticOptions) throw new InvalidCastException($"Ожидаемый тип настроек должен быть {nameof(StaticCubeMeasureOptions)}");
                    measures.Add(_staticLoader.Load(staticOptions,this, dimensions));
                    break;
                }
            }
        }
        return measures;
    }
}