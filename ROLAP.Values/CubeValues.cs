using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;
using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Values;

public static class CubeValues
{
    public static List<CubeQueryValue> GetValues(IRepository repository, List<ICubeMeta> meta)
    {
        List<CubeQueryValue> values = new List<CubeQueryValue>();

        List<CubeDimensionMeta> dimensions = meta.Where(x => x is CubeDimensionMeta).Cast<CubeDimensionMeta>().ToList();
        List<Tuple<string,string>> dimensionsData = new List<Tuple<string,string>>();

        foreach (var dimension in dimensions)
        {
            foreach (var dimensionValue in dimension.Values)
            {
                dimensionsData.Add(new Tuple<string, string>(dimensionValue.ValueConnectionField, dimensionValue.Id));
            }
        }

        foreach (var measure in meta.Where(x => x is CubeMeasureMeta).Cast<CubeMeasureMeta>())
        {
            var metaToAdd = dimensions.Select(x => (ICubeMeta)x.Clone()).ToList();
            metaToAdd.Add(measure);
            repository.GetValues(
                measure.ValueConnection.SchemaName,
                measure.ValueConnection.TableName,
                measure.ValueConnection.ValueField,
                dimensionsData,
                measure.ValueConnection.ValueConnectionField,
                measure.Id).Select(x => new CubeQueryValue
            {
                Value = x,
                MetaInfo = metaToAdd
            });
        }

        return values;
    }
    
}