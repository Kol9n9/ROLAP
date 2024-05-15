using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Static;

public class StaticCubeMeasureValueLoader 
{
    public MeasureValue Load(IEnumerable<Dimension> dimensions, StaticCubeMeasureValueOptions options, CubeMeasureValueLoader baseLoader)
    {
        foreach (var dimension in options.Dimensions)
        {
            FindDimension(dimensions, dimension.Key);
        }
        return new MeasureValue
        {
            Id = options.Id,
            Value = options.Value,
            Dimensions = options.Dimensions.Select(x =>
            {
                return FindDimension(dimensions, x.Key);
            })
        };
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
}