using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.QueryProcessor.Interfaces;

internal interface IQueryItem
{
    IQueryItem Execute(ICubeConfiguration configurationCube);
}