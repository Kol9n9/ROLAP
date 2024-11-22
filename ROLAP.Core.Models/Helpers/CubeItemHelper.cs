using System.Collections;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Model.Query;

namespace ROLAP.Core.Models.Helpers;

public static class CubeItemHelper
{
    public static bool IsValueInDimensions(IValueCubeItem value, IEnumerable<IDimensionCubeItem> dimensions,
        bool fullMatches = false)
    {
        int matches = 0;
        foreach (var dimension in value.GetDimensions())
        {
            if (IsValueDimensionInDimension(dimension, dimensions, fullMatches)) matches++;
        }
        
        return fullMatches ? value.GetDimensions().Count() == matches : dimensions.Count() == matches;
    }

    private static bool IsValueDimensionInDimension(IDimensionCubeItem valueDimension,
        IEnumerable<IDimensionCubeItem> dimensions, bool fullMatches, bool isPrevTotal = false)
    {
        IDimensionCubeItem? find = dimensions.FirstOrDefault(x => x.Equals(valueDimension));
        if (find is null) return !fullMatches && isPrevTotal;
        var valDimension = valueDimension.GetDimensions().FirstOrDefault();
        if (valDimension is null) return true;
        bool isTotal = !find.GetDimensions().Any();
        return IsValueDimensionInDimension(valDimension, find.GetDimensions(),fullMatches,isTotal);
    }

    public static bool IsValueInMeasure(IValueCubeItem value, IMeasureCubeItem measure)
    {
        return value.GetMeasure().Equals(measure);
    }

    public static bool IsValueInTuple(IValueCubeItem value, CubeItemTuple tuple, bool fullMatches = false)
    {
        var measure = tuple.Members.OfType<IMeasureCubeItem>().FirstOrDefault();
        if (measure is not null && !IsValueInMeasure(value, measure)) return false;
        var dimensions = tuple.Members.OfType<IDimensionCubeItem>();
        return IsValueInDimensions(value, dimensions, fullMatches);
    }
}