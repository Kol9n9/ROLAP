using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.Containers;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Loaders;

internal class ValueLoader : ILoader<ValueCubeItem>
{
    private ILoaderHandler<ValueCubeItem, ValueStaticOptions> _staticHandler;
    public ValueLoader(MeasureCubeItem measure, IEnumerable<ILoadOptions> valuesOptions)
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