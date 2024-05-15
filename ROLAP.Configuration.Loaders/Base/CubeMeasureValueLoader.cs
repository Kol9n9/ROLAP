using ROLAP.Common.Interfaces;
using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Static;
using ROLAP.Configuration.Models.Enums;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Base;

public class CubeMeasureValueLoader : IValuesLoader
{

    private StaticCubeMeasureValueLoader _staticLoader = new StaticCubeMeasureValueLoader();
    private readonly IEnumerable<CubeMeasureValueOptions> _options;
    private readonly IEnumerable<Dimension> _dimensions;

    public CubeMeasureValueLoader(IEnumerable<Dimension> dimensions, IEnumerable<CubeMeasureValueOptions> options)
    {
        _dimensions = dimensions;
        _options = options;
    }
    
    private IEnumerable<MeasureValue> Load(IEnumerable<CubeMeasureValueOptions> options)
    {
        List<MeasureValue> values = new List<MeasureValue>();

        foreach (var option in options)
        {
            switch (option.GetSourceType())
            {
                case SourceType.Static:
                {
                    if (option is not StaticCubeMeasureValueOptions staticOptions) throw new InvalidCastException($"Ожидаемый тип настроек должен быть {nameof(StaticCubeMeasureValueOptions)}");
                    values.Add(_staticLoader.Load(_dimensions, staticOptions,this));
                    break;
                }
            }
        }
        
        return values;
    }

    public IEnumerable<MeasureValue> Load()
    {
        if (_options is null) throw new ArgumentNullException(nameof(_options));
        return Load(_options);
    }
}