using ROLAP.Common.Enums;
using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model;

public class MeasureCubeItem : CubeItem
{
    private IValuesLoader? _loader;
    public MeasureCubeItem(string name, string key, IValuesLoader? loader) : base(name, key, CubeItemType.Measure, string.Empty)
    {
        _loader = loader;
    }

    public IEnumerable<MeasureValue> LoadValues(IEnumerable<CubeItem> dimensions)
    {
        if (_loader is null) throw new ArgumentNullException(nameof(_loader));
        return _loader.Load(dimensions);
    }

    public override CubeItem Clone(bool withValues = true)
    {
        var measure = new MeasureCubeItem(Name, Key, _loader);
        var values = withValues ? Values.Select(x => x.Clone()) : new List<CubeItem>();
        measure.Values.AddRange(values);
        return measure;
    }
}