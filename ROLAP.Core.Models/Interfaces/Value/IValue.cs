using Newtonsoft.Json;
using ROLAP.Core.Models.Converters;

namespace ROLAP.Core.Models.Interfaces.Value;

[JsonConverter(typeof(ValueConverter))]
public interface IValue
{
    string GetStringValue();
    Type GetValueType();
    void WriteTo(JsonWriter writer);
}