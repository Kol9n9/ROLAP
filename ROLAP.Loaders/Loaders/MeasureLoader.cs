using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Loaders.Models.Containers;

namespace ROLAP.Loaders.Loaders;

internal class MeasureLoader : ILoader<IMeasureCubeItem>
{
    private readonly ILoaderHandler<IMeasureCubeItem, MeasureStaticOptions> _staticHandler;
    public MeasureLoader()
    {
        _staticHandler = new MeasureStaticHandler();
    }
    public IEnumerable<IMeasureCubeItem> Load(IEnumerable<ILoadOptions> options)
    {
        List<IMeasureCubeItem> measures = new List<IMeasureCubeItem>();

        foreach (var option in options)
        {
            if (!TryAddValue(option, measures)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }

        return measures;
    }


    private bool TryAddValue(ILoadOptions options, List<IMeasureCubeItem> measures)
    {
        if (options is MeasureStaticOptions staticOptions)
        {
            measures.AddRange(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}