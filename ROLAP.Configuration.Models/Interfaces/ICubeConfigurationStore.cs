using ROLAP.Common.Model;

namespace ROLAP.Configuration.Models.Interfaces;

public interface ICubeConfigurationStore
{
    ConfigurationCube GetByName(string name);
}