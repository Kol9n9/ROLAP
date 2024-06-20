using ROLAP.Core.Models.Interfaces;
using ROLAP.Loaders.Loaders;
using ROLAP.Loaders.Models.Options;
using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Loaders.Handlers;

internal class MeasureStaticHandler : ILoaderHandler<MeasureCubeItem, MeasureStaticOptions>
{
    public IEnumerable<MeasureCubeItem> Load(MeasureStaticOptions options)
    {
        var item = new MeasureCubeItem(options.Key, options.Name);
        item.SetLoader(new ValueLoader(item,options.ValuesOptions));
        
        return new List<MeasureCubeItem>
        {
            item
        };
    }
}