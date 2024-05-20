using System.Collections;
using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Static;

public class StaticCubeMeasureValueLoader 
{
    public MeasureValue? Load(IEnumerable<Dimension> allDimensions, IEnumerable<Dimension> filterDimensions, StaticCubeMeasureValueOptions options, CubeMeasureValueLoader baseLoader)
    {
        var value = new MeasureValue
        {
            Id = options.Id,
            Value = options.Value,
            Dimensions = options.Dimensions.Select(x =>
            {
                return FindDimension(allDimensions, x.Key);
            })
        };
        return IsValueInDimensions(value, filterDimensions) ? value : null;
    }

    private Dimension FindDimension(IEnumerable<Dimension> dimensions, string key)
    {
        var parts = GetParts(key);
        List<Dimension> innerDimensions = new List<Dimension>();
        Dimension dimension = null;
        foreach (var part in parts)
        {
            dimension = dimensions.FirstOrDefault(x => x.Key == part);
            dimensions = dimension.Values;
            innerDimensions.Add(dimension.Clone() as Dimension);
        }

        dimension = innerDimensions[0];
        Dimension innerDimension = dimension;

        for (int i = 1; i < innerDimensions.Count; i++)
        {
            innerDimension.Values = new List<Dimension>
            {
                innerDimensions[i]
            };
        }
        
        return dimension;
    }

    private string[] GetParts(string key)
    {
        string[] parts =key.Split("].[");

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i][0] == '[') parts[i] = parts[i].Substring(1);
            if (parts[i][parts[i].Length - 1] == ']') parts[i] = parts[i].Substring(0, parts[i].Length - 1);
        }
        
        return parts;
    }
    
    private bool IsValueInDimensions(MeasureValue value, IEnumerable<Dimension> dimensions)
    {
        if (!dimensions.Any()) return true;

        foreach (var dimension in dimensions)
        {
            Dimension? currentDimension = dimension;
            Dimension? currentMeasureDimension = value.Dimensions.FirstOrDefault(x => x.Key == dimension.Key);
            do
            {
                if (currentMeasureDimension is null) return false;

                currentDimension = currentDimension.Values[0];
                currentMeasureDimension = currentMeasureDimension.Values.FirstOrDefault(x => x.Key == dimension.Key);

            } while (currentDimension is not null);
        }

        return true;
    }
}