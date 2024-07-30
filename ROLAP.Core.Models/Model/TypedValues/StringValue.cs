using Newtonsoft.Json;
using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Core.Models.Model.TypedValues;

public class StringValue : IValue
{
    private string _value;
    public StringValue(string value)
    {
        _value = value;
    }
    public string GetStringValue()
    {
        return _value;
    }

    public Type GetValueType()
    {
        return typeof(StringValue);
    }

    public void WriteTo(JsonWriter writer)
    {
        writer.WriteToken(JsonToken.String,_value);
    }
}