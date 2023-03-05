using ROLAP.Model.Interface;
using ROLAP.Model.Models;
using ROLAP.Utils;

namespace ROLAP.TestLocalRepository
{
    public class TestLocalRepository : IRepository<CubeMeasure>
    {
        public List<CubeMeasure> GetData(List<CubeDimension> dimensions)
        {
            return GetTestMeasures(dimensions);
        }
        private static List<CubeMeasure> GetTestMeasures(List<CubeDimension> dimensions)
        {
            List<CubeMeasure> measures = new List<CubeMeasure>();
            foreach (CubeDimension dim in dimensions)
            {
                measures.Add(new CubeMeasure()
                {
                    Id = Guid.NewGuid(),
                    Value = Utils.Utils.GetRandomDoubleInRange(0,100),
                    Dimension = dim
                });
            }
            return measures;
        }
    }
}