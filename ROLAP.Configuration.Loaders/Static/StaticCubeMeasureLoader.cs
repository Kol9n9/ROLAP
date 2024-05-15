using ROLAP.Common.Model;
using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;

namespace ROLAP.Configuration.Loaders.Static;

internal class StaticCubeMeasureLoader
{
    public Measure Load(StaticCubeMeasureOptions options, CubeMeasureLoader baseLoader, IEnumerable<Dimension> dimensions)
    {
        Measure measure = new Measure(options.Key, options.Name, new CubeMeasureValueLoader(dimensions, options.Values));
        return measure;
    }
}