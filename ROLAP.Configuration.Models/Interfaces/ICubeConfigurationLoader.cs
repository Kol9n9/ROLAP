using ROLAP.Common.Model;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Models.Interfaces;

public interface ICubeConfigurationLoader
{
    ConfigurationCube Load(string name);
}