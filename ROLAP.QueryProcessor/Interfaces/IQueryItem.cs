using ROLAP.Common.Model;

namespace ROLAP.QueryProcessor.Interfaces;

internal interface IQueryItem
{
    IQueryItem Execute(ConfigurationCube configurationCube);
}