using Newtonsoft.Json;
using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Core.Models.Model.TypedValues;

public class IntValue : IValue
{
    private int _value;
    public IntValue(int value)
    {
        _value = value;
    }
    
    public int GetValue()
    {
        return _value;
    }

    public void WriteTo(JsonWriter writer)
    {
        writer.WriteToken(JsonToken.Integer,_value);
    }

    public string GetStringValue()
    {
        return _value.ToString();
    }

    public Type GetValueType()
    {
        return typeof(IntValue);
    }
}