using ROLAP.Configuration.Models.Enums;

namespace ROLAP.Configuration.Models.Models;

public class StaticCubeMeasureOptions : CubeMeasureOptions
{
    public override SourceType GetSourceType()
    {
        return SourceType.Static;
    }
}