using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.QueryProcessor.Interfaces;

internal interface IQueryItem
{
    IQueryItem Execute(CubeConfiguration configurationCube);
}