using ROLAP.Configuration.Models.Enums;

namespace ROLAP.Configuration.Models.Interfaces;

public interface ICubeItemConfigurationOptions
{
    SourceType GetSourceType();
}