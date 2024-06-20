using ROLAP.Core.Models.Interfaces;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;
using ROLAP.Models.Models.IContainers;
using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Loaders.Loaders;

internal class MeasureLoader : ILoader<MeasureCubeItem>
{
    private readonly ILoaderHandler<MeasureCubeItem, MeasureStaticOptions> _staticHandler;
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