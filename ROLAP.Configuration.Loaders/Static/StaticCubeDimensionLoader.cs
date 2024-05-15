using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Static;

internal class StaticCubeDimensionLoader
{
    public Dimension Load(StaticCubeDimensionOptions options, CubeDimensionLoader baseLoader)
    {
        Dimension dimension = new Dimension(options.Key, options.Name, options.GroupKey);
        if (options.Values is not null && options.Values.Any())
        {
            dimension.Values.AddRange(baseLoader.Load(options.Values));
        }
        return dimension;
    }
}