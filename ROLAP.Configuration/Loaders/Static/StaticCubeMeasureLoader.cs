using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Static;

internal class StaticCubeMeasureLoader
{
    public CubeItem Load(StaticCubeMeasureOptions options, CubeMeasureLoader baseLoader, IEnumerable<CubeItem> dimensions)
    {
        MeasureCubeItem measure = new MeasureCubeItem(options.Name, options.Key, new CubeMeasureValueLoader(dimensions, options.Values, options.Key));
        return measure;
    }
}