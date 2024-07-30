using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Core.Models.Helpers;

public static class CubeItemHelper
{
    // public static bool IsCubeItemInContainers(ICubeItem value, IEnumerable<IContainer> containers, bool fullMatches = false)
    // {
    //     if (value is IValueCubeItem valueCubeItem) return IsValueInContainers(valueCubeItem,containers,fullMatches);
    //     return false;
    // }
    //
    // private static bool IsValueInContainers(IValueCubeItem value, IEnumerable<IContainer> containers, bool fullMatches)
    // {
    //     int matches = 0;
    //     foreach (var valueDimension in value.GetDimensions())
    //     {
    //         if (valueDimension.InContainers(containers)) matches++;
    //     }
    //
    //     if (value.GetMeasureContainer().InContainers(containers)) matches++;
    //
    //     return fullMatches ? value.GetDimensions().Count() == matches : containers.Count() == matches;
    // }

    public static bool IsCubeItemInContainers(ICubeItem value, IEnumerable<IDimensionCubeItem> dimensions,
        bool fullMatches = false)
    {
        return false;
    }
}