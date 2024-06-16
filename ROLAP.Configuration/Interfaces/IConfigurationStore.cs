using ROLAP.Core.Models.Model.CubeItem;

namespace ROLAP.Configuration.Interfaces;

public interface IConfigurationStore
{
    CubeConfiguration GetConfiguration(string cubeName);
}