using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Loaders.Models.Options;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Loaders.Models.Containers;

namespace ROLAP.Loaders.Helpers;

internal static class LoadOptionsHelper
{
    public static IEnumerable<ILoadOptions> GetValueOptions(IEnumerable<IContainer> containers)
    {
        List<ILoadOptions> options = new List<ILoadOptions>();
        options.Add(GetDimensionOptions(containers.OfType<DimensionContainer>()));

        return options;
    }


    private static ILoadOptions GetDimensionOptions(IEnumerable<DimensionContainer> dimensionContainers)
    {
        List<ILoadOptions> values = new List<ILoadOptions>();
        foreach (var value in dimensionContainers)
        {
            values.Add(GetDimensionOption(value));
        }

        return new ValueStaticOptions("", "", values);
    }

    private static ILoadOptions GetDimensionOption(DimensionContainer dimensionContainers)
    {
        List<ILoadOptions> options = new List<ILoadOptions>();
        List<ILoadOptions> values = new List<ILoadOptions>();
        foreach (var value in dimensionContainers.GetValues<DimensionContainer>())
        {
            values.Add(GetDimensionOption(value));
        }
        return new DimensionStaticOptions(dimensionContainers.Item.GetKey(),dimensionContainers.Item.GetName(),values);
    }
}