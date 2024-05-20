using ROLAP.Common.Model;

namespace ROLAP.Configuration.Models.Interfaces;

public interface ICubeConfigurationStore
{
    Cube GetByName(string name);
}