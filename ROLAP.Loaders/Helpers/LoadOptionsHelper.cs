using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Models.Options;
using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Helpers;

internal static class LoadOptionsHelper
{
    public static IEnumerable<ILoadOptions> GetValueOptions(IEnumerable<IDimensionCubeItem> dimensions)
    {
        List<ILoadOptions> options = new List<ILoadOptions>();
        options.Add(GetDimensionOptions(dimensions));

        return options;
    }


    private static ILoadOptions GetDimensionOptions(IEnumerable<IDimensionCubeItem> dimensions)
    {
        List<ILoadOptions> values = new List<ILoadOptions>();
        foreach (var value in dimensions)
        {
            values.Add(GetDimensionOption(value));
        }

        return new ValueStaticOptions("", "", values);
    }

    private static ILoadOptions GetDimensionOption(IDimensionCubeItem dimension)
    {
        List<ILoadOptions> options = new List<ILoadOptions>();
        List<ILoadOptions> values = new List<ILoadOptions>();
        foreach (var value in dimension.GetDimensions())
        {
            values.Add(GetDimensionOption(value));
        }
        return new DimensionStaticOptions(dimension.GetKey(),dimension.GetName(),values);
    }
}