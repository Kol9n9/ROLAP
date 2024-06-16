using ROLAP.Core.Models.Interfaces;
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
    public IEnumerable<ValueCubeItem> Load(IEnumerable<ILoadOptions> options)
    {
        List<ValueCubeItem> res = new List<ValueCubeItem>();

        foreach (var option in options)
        {
            if (!TryGetValue(option, out var values)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
            res.AddRange(values!);
        }
        return res;
    }
    
    
    private bool TryGetValue(ILoadOptions options, out IEnumerable<ValueCubeItem>? values)
    {
        if (options is ValueStaticOptions staticOptions)
        {
            values = _staticHandler.Load(staticOptions);
            return true;
        }

        values = null;
        return false;
    }
}