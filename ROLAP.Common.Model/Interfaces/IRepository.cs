using ROLAP.Common.Model.Models;

namespace ROLAP.Common.Model.Interfaces
{
    public interface IRepository
    {
       // public List<CubeDimension> GetDimensions(CubeConfiguration List<Guid> dimensionIds);
        //public List<CubeMeasure> GetMeasures(List<Guid> measuresIds);
        //public List<CubeValue> GetValues(List<List<Guid>> dimensionIds, List<Guid> measureIds);

        public List<object> GetValues(List<CubeMetaItem> measures, List<CubeMetaItem> dimensions);
        public CubeMeta GetCubeMeta(CubeConfiguration configuration);
    }
}