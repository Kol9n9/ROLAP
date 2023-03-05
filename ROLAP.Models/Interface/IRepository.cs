using ROLAP.Model.Models;

namespace ROLAP.Model.Interface
{
    public interface IRepository<T>
    {
        public List<T> GetData(List<CubeDimension> dimensions);
    }
}