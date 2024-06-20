using ROLAP.Core.Models.Interfaces;
using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Models.Helpers;

public static class CubeItemHelper
{
    public static bool IsCubeItemInContainers(ICubeItem value, IEnumerable<IContainer> containers, bool fullMatches = false)
    {
        if (value is ValueCubeItem valueCubeItem) return IsValueInContainers(valueCubeItem,containers,fullMatches);
        return false;
    }

    private static bool IsValueInContainers(ValueCubeItem value, IEnumerable<IContainer> containers, bool fullMatches)
    {
        int matches = 0;
        foreach (var valueDimension in value.Dimensions)
        {
            if (valueDimension.InContainers(containers)) matches++;
        }

        if (value.Measure.InContainers(containers)) matches++;

        return fullMatches ? value.Dimensions.Count() == matches : containers.Count() == matches;
    }
}