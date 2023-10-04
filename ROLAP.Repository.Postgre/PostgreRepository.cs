using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;

namespace ROLAP.Repository.Postgre;

public class PostgreRepository : IRepository
{
    public List<CubeQueryValue> GetValues(List<CubeMetaItem> measures, List<CubeMetaItem> dimensions)
    {
        var dimensionFields = dimensions.Select(x => new Tuple<string, string>(x.ConnectionField, x.Key)).ToList();
        List<CubeQueryValue> results = new List<CubeQueryValue>();
        foreach (var measure in measures)
        {
            if (!string.IsNullOrWhiteSpace(measure.Table))
            {
                results.AddRange(Helper.GetValues(measure.Schema, measure.Table, measure.ValueField, dimensionFields)
                    .Select(x => new CubeQueryValue
                    {
                        Value = x,
                        Dimensions = new List<CubeMetaItem>(dimensions),
                        Measure = measure
                    }));
            }
        }

        return results;
    }

    public CubeMeta GetCubeMeta(CubeConfiguration configuration)
    {
        CubeMeta meta = new CubeMeta
        {
            Measures = new List<CubeMetaMeasure>(),
            Dimensions= new List<CubeMetaDimension>(),
        };

        foreach (var measure in configuration.Measures)
        {
            meta.Measures.Add(new CubeMetaMeasure {
                Measure = new CubeMetaItem
                {
                    Key = measure.MeasureValue.Id,
                    Schema = measure.MeasureValue.Schema,
                    Table = measure.MeasureValue.Table,
                    Name = measure.MeasureValue.Name,
                    ValueField = measure.MeasureValue.Value,
                    Dimensions = measure.DimensionNames

                }
            });
        }
        foreach (var measureDimension in configuration.Dimensions)
        {
            if(measureDimension is OneTableOneDimensions oneDimension)
            {
                CubeMetaDimension metaDimension = new CubeMetaDimension
                {
                    Dimension = new CubeMetaItem
                    {
                        Key = oneDimension.Key.ToString(),
                        Name = oneDimension.Name
                    },
                    Values = new List<CubeMetaItem>()
                };
                if (measureDimension.TableValues != null)
                {
                    metaDimension.Values.AddRange(Helper.GetDimensionValuesFromOneTableMetaItems(measureDimension.TableValues.Schema, measureDimension.TableValues.Table, measureDimension.TableValues.Key, measureDimension.TableValues.Name, measureDimension.TableValues.ValueConnectionField));
                }
                meta.Dimensions.Add(metaDimension);
            } 
            else if (measureDimension is OneTableManyDimensions manyDimensions)
            {
                foreach(var dimension in Helper.GetDimensions(manyDimensions.Schema, manyDimensions.Table, manyDimensions.Id, manyDimensions.Name))
                {
                    CubeMetaDimension metaDimension = new CubeMetaDimension
                    {
                        Dimension = new CubeMetaItem
                        {
                            Key = dimension.Item1,
                            Name = dimension.Item2
                        },
                        Values = new List<CubeMetaItem>()
                    };
                    if(manyDimensions.TableValues != null)
                    {
                        metaDimension.Values.AddRange(Helper.GetDimensionValuesFromTableMetaItems(manyDimensions.TableValues.Schema, manyDimensions.TableValues.Table, manyDimensions.TableValues.Key, manyDimensions.TableValues.Name, manyDimensions.TableValues.DimensionConnectionField, metaDimension.Dimension.Key));
                    }
                    meta.Dimensions.Add(metaDimension);
                }
            }
            
        }
        return meta;
    }
}