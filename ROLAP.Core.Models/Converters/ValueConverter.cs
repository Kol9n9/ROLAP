using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Core.Models.Converters;

public class ValueConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        try
        {
            var val = (IValue)value!;
            val.WriteTo(writer);
            
        }
        catch (Exception ex)
        {
            JObject o = (JObject)value;
            o.WriteTo(writer);
        } 
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }
}