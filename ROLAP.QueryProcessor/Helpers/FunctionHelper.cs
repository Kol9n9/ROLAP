using ROLAP.QueryProcessor.Interfaces;
using ROLAP.QueryProcessor.Models.Functions;
using ROLAP.QueryProcessor.Models.Items;

namespace ROLAP.QueryProcessor.Helpers;

internal static class FunctionHelper
{
    private static Dictionary<string, Type> _functions = new Dictionary<string, Type>()
    {
        {"crossjoin", typeof(CrossJoinFunc)}
    };

    public static FunctionItem GetFunction(string functionName, IEnumerable<IQueryItem> args)
    {
        if (!_functions.TryGetValue(functionName.ToLower(), out var funcType)) throw new Exception($"Неизвестная функция \"{functionName}\"");
        try
        {
            var func = Activator.CreateInstance(funcType, args: args) as FunctionItem;
            if (func is null) throw new Exception("Instance is null");
            return func;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка создания экземпляра функции \"{functionName}\": {ex.Message}");
        }
    }
}