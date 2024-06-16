using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Helpers;

public static class LoadOptionsHelper
{
    public static ILoadOptions GetValueOptionsByDimension(IEnumerable<DimensionCubeItem> dimensionCubeItems)
    {
        List<ILoadOptions> dimensionOptions = new List<ILoadOptions>();
        foreach (var dimensionCubeItem in dimensionCubeItems)
        {
            dimensionOptions.AddRange(SplitDimensionToOptions(dimensionCubeItem));
        }
        return new ValueStaticOptions("", "", dimensionOptions);
    }
    private static IEnumerable<ILoadOptions> SplitDimensionToOptions(DimensionCubeItem dimensionCubeItem)
    {
        List<ILoadOptions> options = new List<ILoadOptions>();
        List<ILoadOptions> values = new List<ILoadOptions>();
        foreach (var value in dimensionCubeItem.Values)
        {
            values.AddRange(SplitDimensionToOptions(value));
        }
        options.Add(new DimensionStaticOptions(dimensionCubeItem.Key,dimensionCubeItem.Name,values));
        return options;
    }
}