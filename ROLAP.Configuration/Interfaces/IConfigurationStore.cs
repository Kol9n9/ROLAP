using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Configuration.Interfaces;

public interface IConfigurationStore
{
    CubeConfiguration GetConfiguration(string cubeName);
}