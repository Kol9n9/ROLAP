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
    public IEnumerable<IValueCubeItem> Load(IEnumerable<ILoadOptions> options)
    {
        List<IValueCubeItem> values = new List<IValueCubeItem>();

        foreach (var option in options)
        {
            if (!TryAddValue(option,values)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }
        return values;
        
    }
    
    
    private bool TryAddValue(ILoadOptions options, List<IValueCubeItem> values)
    {
        if (options is ValueStaticOptions staticOptions)
        {
            values.AddRange(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}