using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.QueryProcessor.Interfaces;

internal interface IQueryItem
{
    IQueryItem Execute(CubeConfiguration configurationCube);
}