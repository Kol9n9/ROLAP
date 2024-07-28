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
    public IContainer Load(IEnumerable<ILoadOptions> options)
    {
        MeasureContainer container = new MeasureContainer();

        foreach (var option in options)
        {
            if (!TryAddValue(option, container)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }

        return container;
    }


    private bool TryAddValue(ILoadOptions options, MeasureContainer container)
    {
        if (options is MeasureStaticOptions staticOptions)
        {
            container.AddValue(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}