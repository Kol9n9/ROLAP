using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Common.Model.Helpers;

public static class MetaHelper
{
    public static CubeMeasureMeta? GetMeasureMeta(List<CubeMeasureMeta> metas, List<string> names)
    {
        if (names[0].ToLower() != "measure") throw new Exception("This must be measure format");
        return metas.FirstOrDefault(x => names[1][0] == '&' ? x.Key == names[1] : x.Name == names[1])?.Clone() as CubeMeasureMeta;
    }

    public static CubeDimensionMeta? GetDimensionWithValueMeta(List<CubeDimensionMeta> metas, List<string> names)
    {
        var dimension = metas.FirstOrDefault(x => names[0][0] == '&' ? x.Key == names[0] : x.Name == names[0]);
        if (dimension == null) return null;
        dimension = (CubeDimensionMeta)dimension.Clone();
        dimension.Values =
            dimension.Values.Where(x => names[1][0] == '&' ? x.Key == names[1] : x.Name == names[1]).ToList();
        return dimension.Values.Any() ? dimension : null;
    }
}