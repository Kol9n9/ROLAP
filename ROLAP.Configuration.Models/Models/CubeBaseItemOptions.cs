using ROLAP.Configuration.Models.Enums;
using ROLAP.Configuration.Models.Interfaces;

namespace ROLAP.Configuration.Models.Models;

public abstract class CubeBaseItemOptions : ICubeItemConfigurationOptions
{
    public ConnectionInfo ConnectionInfo { get; set; }
    public abstract SourceType GetSourceType();
}