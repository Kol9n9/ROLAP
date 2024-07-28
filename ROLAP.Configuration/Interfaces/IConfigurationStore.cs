using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Configuration.Interfaces;

public interface IConfigurationStore
{
    ICubeConfiguration GetConfiguration(string cubeName);
}