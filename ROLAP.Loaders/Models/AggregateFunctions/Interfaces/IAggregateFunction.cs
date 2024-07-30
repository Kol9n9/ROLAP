using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Loaders.Models.AggregateFunctions.Interfaces;

public interface IAggregateFunction<out T>
    where T : IValue
{
    Func<IEnumerable<IValueCubeItem>, IValueCubeItem> GetFunctionDelegate();
}