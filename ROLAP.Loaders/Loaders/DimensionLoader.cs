using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Loaders;

internal class DimensionLoader : ILoader<DimensionCubeItem>
{
    private readonly DimensionStaticHandler _staticHandler;
    public DimensionLoader()
    {
        _staticHandler = new DimensionStaticHandler(this);
    }
    
    public IEnumerable<DimensionCubeItem> Load(IEnumerable<ILoadOptions> options)
    {
        List<DimensionCubeItem> dimensions = new List<DimensionCubeItem>();

        foreach (var option in options)
        {
            if (!TryGetValue(option, out var values)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
            dimensions.AddRange(values);
        }

        return dimensions;
    }
    
    
    private bool TryGetValue(ILoadOptions options, out IEnumerable<DimensionCubeItem> values)
    {
        if (options is DimensionStaticOptions staticOptions)
        {
            values = _staticHandler.Load(staticOptions);
            return true;
        }

        values = new List<DimensionCubeItem>();
        return false;
    }
}