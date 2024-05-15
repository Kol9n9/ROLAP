using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Static;
using ROLAP.Configuration.Models.Enums;
using ROLAP.Configuration.Models.Interfaces;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Base;

public class CubeDimensionLoader : ICubeItemLoader<Dimension,CubeDimensionOptions>
{
    private readonly StaticCubeDimensionLoader _staticLoader = new StaticCubeDimensionLoader();
    public IEnumerable<Dimension> Load(IEnumerable<CubeDimensionOptions> options)
    {
        List<Dimension> dimensions = new List<Dimension>();
        foreach (var option in options)
        {
            switch (option.GetSourceType())
            {
                case SourceType.Static:
                {
                    if (option is not StaticCubeDimensionOptions staticOptions) throw new InvalidCastException($"Ожидаемый тип настроек должен быть {nameof(StaticCubeDimensionOptions)}");
                    dimensions.Add(_staticLoader.Load(staticOptions,this));
                    break;
                }
            }
        }

        return dimensions;
    }
}