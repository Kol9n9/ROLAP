using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Core.Models.Model.TypedValues;

public static class ValueTypeHelper
{

    private static Dictionary<string, Type> _valueTypes = new Dictionary<string, Type>
    {
        { nameof(IntValue), typeof(IntValue) },
        { nameof(StringValue), typeof(StringValue) },
    };
    
    public static Type GetValueType(string valueType)
    {
        if (_valueTypes.TryGetValue(valueType, out var res)) return res;
        throw new Exception($"Тпи {valueType} не найден");
    }
    
    public static IValue Create(Type valueType, string value)
    {
        switch (valueType.Name)
        {
            case nameof(IntValue):
            {
                if (!int.TryParse(value, out var result))
                {
                    result = 0;
                }

                return new IntValue(result);
            }
        }

        throw new Exception($"{valueType.Name} не является типом значений");
    }
}