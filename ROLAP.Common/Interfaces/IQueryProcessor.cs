using ROLAP.Common.Model;

namespace ROLAP.Common.Interfaces;

public interface IQueryProcessor
{
    CubeQuery ProcessQuery(string query);
}