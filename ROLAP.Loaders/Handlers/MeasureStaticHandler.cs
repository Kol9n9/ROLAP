using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Loaders;
using ROLAP.Loaders.Models.CubeItems;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Handlers;

internal class MeasureStaticHandler : ILoaderHandler<IMeasureCubeItem, MeasureStaticOptions>
{
    public IEnumerable<IMeasureCubeItem> Load(MeasureStaticOptions options)
    {
        var item = new MeasureCubeItem(options.Key, options.Name);
        item.SetLoader(new ValueLoader(item,options.ValuesOptions));
        
        return new List<IMeasureCubeItem>
        {
            item
        };
    }
}