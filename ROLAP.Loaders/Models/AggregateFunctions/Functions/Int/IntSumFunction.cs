using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Model.TypedValues;
using ROLAP.Loaders.Models.AggregateFunctions.Interfaces;
using ROLAP.Loaders.Models.CubeItems;

namespace ROLAP.Loaders.Models.AggregateFunctions.Functions.Int;

internal class IntSumFunction : IAggregateFunction<IntValue>
{
    public Func<IEnumerable<IValueCubeItem>, IValueCubeItem> GetFunctionDelegate()
    {
        return new Func<IEnumerable<IValueCubeItem>, IValueCubeItem>(items =>
        {
            if (!items.Any()) return new ValueCubeItem("", new EmptyValue(), null, null);
            var first = items.First();
            var firstType = first.GetValue().GetValueType();
            if (firstType != typeof(IntValue)) throw new Exception($"Функция аггрегации Сумма недоступна для типа {firstType.Name}");
            var sum = items.Select(x => ((IntValue)x.GetValue()).GetValue()).Sum();
            return new ValueCubeItem("", new IntValue(sum), null, null);
        });
    }
}