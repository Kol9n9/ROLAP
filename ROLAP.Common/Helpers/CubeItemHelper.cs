using ROLAP.Common.Model;

namespace ROLAP.Common.Helpers;

public static class CubeItemHelper
{
    public static bool IsValueInDimensions(MeasureValue value, IEnumerable<CubeItem> dimensions)
    {
        foreach (var dimension in dimensions)
        {
            var valueDimensions = value.Dimensions.ToList();
            var currentDimension = dimension;

            do
            {
                var valueDimension = valueDimensions.FirstOrDefault(x => x.Key == currentDimension.Key);
                if (valueDimension is null)
                {
                    return false;
                }

                if (currentDimension.Values.Any())
                {
                    var val = valueDimension.Values.FirstOrDefault(x =>
                        currentDimension.Values.FirstOrDefault(y => x.Key == y.Key) is not null);
                    if (val is null) return false;
                    currentDimension = currentDimension.Values.FirstOrDefault(x => x.Key == val.Key);
                    valueDimensions = val.Values;
                }
            } while (currentDimension != null && currentDimension.Values.Any());
        }

        return true;
    }
}