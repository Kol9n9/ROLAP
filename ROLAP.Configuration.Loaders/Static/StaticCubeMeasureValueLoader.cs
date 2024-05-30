using System.Collections;
using ROLAP.Common.Enums;
using ROLAP.Common.Helpers;
using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Static;

public class StaticCubeMeasureValueLoader 
{
    public MeasureValue? Load(IEnumerable<Dimension> allDimensions, IEnumerable<CubeItem> filterDimensions, StaticCubeMeasureValueOptions options, CubeMeasureValueLoader baseLoader)
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
        return CubeItemHelper.IsValueInDimensions(value, filterDimensions) ? value : null;
    }

    private CubeItem FindDimension(IEnumerable<Dimension> dimensions, string key)
    {
        var parts = GetParts(key);
        List<CubeItem> innerDimensions = new List<CubeItem>();
        CubeItem dimension = null;
        foreach (var part in parts)
        {
            var tmpDimension = dimensions.FirstOrDefault(x => x.Key == part);
            dimension = new CubeItem(tmpDimension.Name, tmpDimension.Key, CubeItemType.Dimension);
            dimensions = tmpDimension.Values;
            innerDimensions.Add(dimension.Clone(false));
        }

        dimension = innerDimensions[0];
        CubeItem innerDimension = dimension;

        for (int i = 1; i < innerDimensions.Count; i++)
        {
            innerDimension.Values.Add(innerDimensions[i]);
            innerDimension = innerDimension.Values[0];
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