using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Loaders;

internal class MeasureLoader : ILoader<MeasureCubeItem>
{
    private readonly ILoaderHandler<MeasureCubeItem, MeasureStaticOptions> _staticHandler;
    public MeasureLoader()
    {
        _staticHandler = new MeasureStaticHandler();
    }
    public IEnumerable<MeasureCubeItem> Load(IEnumerable<ILoadOptions> options)
    {
        List<MeasureCubeItem> measures = new List<MeasureCubeItem>();

        foreach (var option in options)
        {
            if (!TryGetValue(option, out var values)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
            measures.AddRange(values);
        }

        return measures;
    }


    private bool TryGetValue(ILoadOptions options, out IEnumerable<MeasureCubeItem> values)
    {
        if (options is MeasureStaticOptions staticOptions)
        {
            values = _staticHandler.Load(staticOptions);
            return true;
        }

        values = new List<MeasureCubeItem>();
        return false;
    }
}