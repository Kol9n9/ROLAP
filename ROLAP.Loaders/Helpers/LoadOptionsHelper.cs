using ROLAP.Core.Models.Interfaces;
using ROLAP.Loaders.Models.Options;
using ROLAP.Models.Models.IContainers;

namespace ROLAP.Loaders.Helpers;

public static class LoadOptionsHelper
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
        return new DimensionStaticOptions(dimensionContainers.Item.Key,dimensionContainers.Item.Name,values);
    }
}

//
// using ROLAP.Core.Models.Interfaces;
// using ROLAP.Core.Models.Models.CubeItem;
// using ROLAP.Loaders.Models.Options;
//
// namespace ROLAP.Loaders.Helpers;
//
// public static class LoadOptionsHelper
// {
//     public static ILoadOptions GetValueOptionsByDimension(IEnumerable<DimensionCubeItem> dimensionCubeItems)
//     {
//         List<ILoadOptions> dimensionOptions = new List<ILoadOptions>();
//         foreach (var dimensionCubeItem in dimensionCubeItems)
//         {
//             dimensionOptions.AddRange(SplitDimensionToOptions(dimensionCubeItem));
//         }
//         return new ValueStaticOptions("", "", dimensionOptions);
//     }
//     private static IEnumerable<ILoadOptions> SplitDimensionToOptions(DimensionCubeItem dimensionCubeItem)
//     {
//         List<ILoadOptions> options = new List<ILoadOptions>();
//         List<ILoadOptions> values = new List<ILoadOptions>();
//         foreach (var value in dimensionCubeItem.Values)
//         {
//             values.AddRange(SplitDimensionToOptions(value));
//         }
//         options.Add(new DimensionStaticOptions(dimensionCubeItem.Key,dimensionCubeItem.Name,values));
//         return options;
//     }
// }