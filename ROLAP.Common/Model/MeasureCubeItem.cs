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
}