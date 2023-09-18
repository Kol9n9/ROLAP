using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;

namespace ROLAP.Repository.Postgre;

public class PostgreRepository : IRepository
{
    //public List<CubeDimension> GetDimensions(List<Guid> dimensionIds)
    //{
    //    throw new NotImplementedException();
    //}

    //public List<CubeMeasure> GetMeasures(List<Guid> measuresIds)
    //{
    //    throw new NotImplementedException();
    //}

    //public List<CubeValue> GetValues(List<List<Guid>> dimensionIds, List<Guid> measureIds)
    //{
    //    throw new NotImplementedException();
    //}

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
                    Name = measureDimension.Name,
                    ConnectionField = measureDimension.TableValues.ConnectionField
                },
                Values = new List<CubeMetaItem>()
            };
            if (measureDimension.TableValues != null)
            {
                dimension.Values.AddRange(Helper.GetMetaItems(measureDimension.TableValues.Schema,measureDimension.TableValues.Table,measureDimension.TableValues.Key,measureDimension.TableValues.Name));
            }
            meta.Dimensions.Add(dimension);
        }
        return meta;
    }
}