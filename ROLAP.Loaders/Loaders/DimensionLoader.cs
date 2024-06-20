using ROLAP.Core.Models.Interfaces;
using ROLAP.Loaders.Handlers;
using ROLAP.Loaders.Models.Options;
using ROLAP.Models.Models.IContainers;
using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Loaders.Loaders;

internal class DimensionLoader : ILoader<DimensionContainer>
{
    private readonly DimensionStaticHandler _staticHandler;
    public DimensionLoader()
    {
        _staticHandler = new DimensionStaticHandler(this);
    }
    
    public IContainer Load(IEnumerable<ILoadOptions> options)
    {

        DimensionContainer container = new DimensionContainer(new DimensionCubeItem("-1","Измерения"));
        
        foreach (var option in options)
        {
            if (!TryAddValue(option, container)) throw new Exception($"Для типа {option.GetType()} не задан обработчик");
        }

        return container;
    }
    
    
    private bool TryAddValue(ILoadOptions options, DimensionContainer container)
    {
        if (options is DimensionStaticOptions staticOptions)
        {
            container.AddValue(_staticHandler.Load(staticOptions));
            return true;
        }

        return false;
    }
}