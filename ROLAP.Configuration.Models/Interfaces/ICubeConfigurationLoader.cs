using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Models.Interfaces;

public interface ICubeConfigurationLoader
{
    CubeConfiguration Load(string name);
}