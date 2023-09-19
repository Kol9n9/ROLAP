using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;

namespace ROLAP.Repository.Postgre;

public class PostgreRepository : IRepository
{
    public List<object> GetValues(List<CubeMetaItem> measures, List<CubeMetaItem> dimensions)
    {
        var dimensionFields = dimensions.Select(x => new Tuple<string, string>(x.ConnectionField, x.Key)).ToList();
        List<object> results = new List<object>();
        foreach (var measure in measures)
        {
            if (!string.IsNullOrWhiteSpace(measure.Table))
            {
                results.AddRange(Helper.GetValues(measure.Schema, measure.Table, measure.ValueField, dimensionFields));
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
                    ValueField = measure.MeasureValue.Value

                }
            });
        }
        foreach (var measureDimension in configuration.Dimensions)
        {
            CubeMetaDimension dimension = new CubeMetaDimension
            {
                Dimension = new CubeMetaItem
                {
                    Key = measureDimension.Key.ToString(),
                    Name = measureDimension.Name
                },
                Values = new List<CubeMetaItem>()
            };
            if (measureDimension.TableValues != null)
            {
                dimension.Values.AddRange(Helper.GetMetaItems(measureDimension.TableValues.Schema,measureDimension.TableValues.Table,measureDimension.TableValues.Key,measureDimension.TableValues.Name,measureDimension.TableValues.ConnectionField));
            }
            meta.Dimensions.Add(dimension);
        }
        return meta;
    }
}