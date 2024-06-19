using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Core.Models.Helpers;

public static class CubeItemHelper
{
    public static bool IsValueInContainers(ValueCubeItem value, IEnumerable<IContainer> containers, bool fullMatches = false)
    {
        int matches = 0;
        foreach (var valueDimension in value.Dimensions)
        {
            if (valueDimension.InContainers(containers)) matches++;
        }

        return fullMatches ? value.Dimensions.Count() == matches : containers.Count() == matches;
    }
}