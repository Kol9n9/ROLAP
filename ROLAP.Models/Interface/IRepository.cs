using ROLAP.Model.Models;

namespace ROLAP.Model.Interface
{
    public interface IRepository
    {
        public List<CubeMeasure> GetMeasures(List<Guid> dimensionIds);
        public List<CubeDimension> GetDimensions(List<Guid> dimensionIds);
    }
}