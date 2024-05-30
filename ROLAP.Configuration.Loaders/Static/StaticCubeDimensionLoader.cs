using ROLAP.Common.Enums;
using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Static;

internal class StaticCubeDimensionLoader
{
    public CubeItem Load(StaticCubeDimensionOptions options, CubeDimensionLoader baseLoader)
    {
        CubeItem dimension = new CubeItem(options.Name, options.Key, CubeItemType.Dimension, options.GroupKey);
        if (options.Values is not null && options.Values.Any())
        {
            dimension.Values.AddRange(baseLoader.Load(options.Values));
        }
        return dimension;
    }
}