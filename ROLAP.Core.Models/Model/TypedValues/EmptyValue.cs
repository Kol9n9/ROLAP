using Newtonsoft.Json;
using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Core.Models.Model.TypedValues;

public class EmptyValue : IValue
{
    public void WriteTo(JsonWriter writer)
    {
        writer.WriteToken(JsonToken.Null);
    }

    public string GetValue()
    {
        return String.Empty;
    }

    public string GetStringValue()
    {
        return String.Empty;
    }

    public Type GetValueType()
    {
        return typeof(EmptyValue);
    }
}