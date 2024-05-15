using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Models.Interfaces;

public interface ICubeConfigurationStore
{
    CubeConfiguration GetByName(string name);
}