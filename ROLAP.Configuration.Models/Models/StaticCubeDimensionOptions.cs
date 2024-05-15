using ROLAP.Configuration.Models.Enums;

namespace ROLAP.Configuration.Models.Models;

public class StaticCubeDimensionOptions : CubeDimensionOptions
{
    public override SourceType GetSourceType()
    {
        return SourceType.Static;
    }
}