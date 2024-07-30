using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Model.TypedValues;
using ROLAP.Loaders.Models.AggregateFunctions.Functions.Int;
using ROLAP.Loaders.Models.AggregateFunctions.Interfaces;

namespace ROLAP.Loaders.Models.AggregateFunctions.Factories;

internal class IntValueAggregateFunctionFactory : IAggregateFunctionFactory<IntValue>
{
    public IAggregateFunction<IntValue> GetAggregateFunction(AggregateFunctionType functionType)
    {
        switch (functionType)
        {
            case AggregateFunctionType.Sum:
            {
                return new IntSumFunction();
            }
        }

        throw new Exception($"Функция аггрегации {functionType.ToString()} для тип {nameof(IntValue)} не найдена");
    }
}