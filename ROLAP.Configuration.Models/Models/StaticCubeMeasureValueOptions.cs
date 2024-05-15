using ROLAP.Configuration.Models.Enums;

namespace ROLAP.Configuration.Models.Models;

public class StaticCubeMeasureValueOptions : CubeMeasureValueOptions
{
    public override SourceType GetSourceType()
    {
        return SourceType.Static;
    }
}