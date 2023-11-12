using ROLAP.Common.Model.Models;
using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Common.Model.Interfaces
{
    public interface IRepository
    {
       // public List<CubeDimension> GetDimensions(CubeConfiguration List<Guid> dimensionIds);
        //public List<CubeMeasure> GetMeasures(List<Guid> measuresIds);
        //public List<CubeValue> GetValues(List<List<Guid>> dimensionIds, List<Guid> measureIds);

        //public List<CubeQueryValue> GetValues(List<CubeMetaItem2> measures, List<CubeMetaItem2> dimensions);
        //public List<CubeQueryValue> GetValues(List<Tuple<>>)

        public List<object> GetValues(string schemaName, string tableName, string valueField, List<Tuple<string,string>>? dimensions, string? connectionField = null, string? connectionFieldValue = null);
        public List<CubeMetaData> LoadMetaData(string schemaName, string tableName, string idField, string keyField, string nameField, string? connectionField = null, string? connectionFieldValue = null);
    }
}