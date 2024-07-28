using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Loaders.Models.Containers;

namespace ROLAP.Loaders.Loaders;

internal class ValueLoader : ILoader<IValueCubeItem>
{
    private ILoaderHandler<IValueCubeItem, ValueStaticOptions> _staticHandler;
    public ValueLoader(IMeasureCubeItem measure, IEnumerable<ILoadOptions> valuesOptions)
    {
        _staticHandler = new ValueStaticHandler(valuesOptions.Where(x => x is ValueStaticOptions).Cast<ValueStaticOptions>(), new DimensionLoader(), measure);
    }
    public IContainer Load(IEnumerable<ILoadOptions> options)
    {
        ValueContainer container = new ValueContainer();

        foreach (var option in options)
        {
            if (!TryAddValue(option,container)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }
        return container;
        
    }
    
    
    private bool TryAddValue(ILoadOptions options, ValueContainer container)
    {
        if (options is ValueStaticOptions staticOptions)
        {
            container.AddValue(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}