using ROLAP.Configuration.Models.Enums;

namespace ROLAP.Configuration.Models.Models;

public class Source
{
    public SourceType Type { get; }
    public SourceConnectionInfo ConnectionInfo { get; }
}

public class SourceConnectionInfo
{
    public string KeyField { get; set; } = "Key";
    public string NameField { get; set; } = "Name";
}