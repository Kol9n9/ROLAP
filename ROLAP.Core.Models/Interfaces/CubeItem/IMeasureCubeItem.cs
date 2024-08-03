using ROLAP.Core.Models.Interfaces.Loader;

namespace ROLAP.Core.Models.Interfaces.CubeItem;

public interface IMeasureCubeItem : IMemberCubeItem
{
    ILoader<IValueCubeItem> GetLoader();

    IValueCubeItem Aggregate(IEnumerable<IValueCubeItem> items);

    Type GetValueType();
    
    ICubeItem GetTotalItem();

    bool IsTotal();
}