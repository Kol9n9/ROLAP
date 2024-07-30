using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Interfaces.Value;
using ROLAP.Core.Models.Model.TypedValues;
using ROLAP.Loaders.Models.AggregateFunctions.Factories;
using ROLAP.Loaders.Models.AggregateFunctions.Interfaces;

namespace ROLAP.Loaders.Models.AggregateFunctions;

internal static class AggregateFunctionCreator
{
    private static Dictionary<Type, Type> _factoryTypes = new Dictionary<Type, Type>
    {
        { typeof(IntValue), typeof(IntValueAggregateFunctionFactory) }
    };

    private static Dictionary<Type, IAggregateFunctionFactory<IValue>> _factories = new Dictionary<Type, IAggregateFunctionFactory<IValue>>();
    
    public static Func<IEnumerable<IValueCubeItem>, IValueCubeItem> Create<T>(AggregateFunctionType functionType) where T : IValue
    {
        return Create(typeof(T), functionType);
    }
    
    public static Func<IEnumerable<IValueCubeItem>, IValueCubeItem> Create(Type valueType, AggregateFunctionType functionType)
    {
        var factory = GetFactory(valueType);
        var function = factory.GetAggregateFunction(functionType);
        return function.GetFunctionDelegate();
    }

    private static IAggregateFunctionFactory<IValue> GetFactory(Type T)
    {
        if (_factories.TryGetValue(T, out var factory)) return factory;

        if (!_factoryTypes.TryGetValue(T, out var factoryType))
        {
            throw new Exception($"Фабрика аггрегированных функций для типа {T} не найдена");
        }

        
        IAggregateFunctionFactory<IValue> instance = (IAggregateFunctionFactory<IValue>)Activator.CreateInstance(factoryType)!;
        _factories.Add(T, instance);
        return instance;
    }
}