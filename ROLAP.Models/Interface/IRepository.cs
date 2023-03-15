using ROLAP.Model.Models;

namespace ROLAP.Model.Interface
{
    public interface IRepository
    {
        public List<CubeDimension> GetDimensions(List<Guid> dimensionIds);
        public List<CubeMeasure> GetMeasures(List<Guid> measuresIds);
        public List<CubeValue> GetValues(List<List<Guid>> dimensionIds, List<Guid> measureIds);
    }
}