using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Loaders.Models.AggregateFunctions.Interfaces;

internal interface IAggregateFunctionFactory<out T> where T: IValue
{
    IAggregateFunction<T> GetAggregateFunction(AggregateFunctionType functionType);
}