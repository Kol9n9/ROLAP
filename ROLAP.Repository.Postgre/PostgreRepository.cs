using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;

namespace ROLAP.Repository.Postgre;

public class PostgreRepository : IRepository
{
    public List<CubeDimension> GetDimensions(List<Guid> dimensionIds)
    {
        throw new NotImplementedException();
    }

    public List<CubeMeasure> GetMeasures(List<Guid> measuresIds)
    {
        throw new NotImplementedException();
    }

    public List<CubeValue> GetValues(List<List<Guid>> dimensionIds, List<Guid> measureIds)
    {
        throw new NotImplementedException();
    }

    public CubeMeta GetCubeMeta(CubeConfiguration configuration)
    {
        CubeMeta meta = new CubeMeta
        {
            Measures = new List<CubeMetaMeasure>()
        };

        foreach (var measure in configuration.Measures)
        {
            CubeMetaMeasure metaMeasure = new CubeMetaMeasure
            {
                Measure = new CubeMetaItem
                {
                    Key = "",
                    Schema = measure.MeasureValue.Schema,
                    Table = measure.MeasureValue.Table,
                    Name = measure.MeasureValue.Name
                },
                Dimensions = new List<CubeMetaDimension>()
            };
            foreach (var measureDimension in measure.Dimensions)
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
                    
                metaMeasure.Dimensions.Add(dimension);
            }
            meta.Measures.Add(metaMeasure);
        }

        return meta;
    }
}