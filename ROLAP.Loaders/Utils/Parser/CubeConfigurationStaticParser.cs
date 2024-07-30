using Newtonsoft.Json.Linq;
using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Helpers;
using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Core.Models.Model.TypedValues;
using ROLAP.Loaders.Enums;
using ROLAP.Loaders.Models.Options;

namespace ROLAP.Loaders.Utils.Parser;

internal static class CubeConfigurationStaticParser
{
    public static CubeConfiguration ParseCubeConfiguration(string json)
    {
        JObject jObject = JObject.Parse(json);
        var dimensions = ParseDimensions(jObject["Dimensions"]);
        var measures = ParseMeasures(jObject["Measures"]);
        return new CubeConfiguration(dimensions, measures);
    }

    private static IEnumerable<ILoadOptions> ParseDimensions(JToken jToken)
    {
        if(jToken is null) return new List<ILoadOptions>();
        var res = new List<ILoadOptions>();
        foreach (var sourceItem in JArray.FromObject(jToken))
        {
            res.AddRange(ParseSourceItem(sourceItem, SourceLoaderType.Dimension));
        }

        return res;
    }


    private static IEnumerable<ILoadOptions> ParseMeasures(JToken jToken)
    {
        if(jToken is null) return new List<ILoadOptions>();
        var res = new List<ILoadOptions>();

        foreach (var sourceItem in JArray.FromObject(jToken))
        {
            res.AddRange(ParseSourceItem(sourceItem, SourceLoaderType.Measure));
        }
        
        return res;
    }

    private static IEnumerable<ILoadOptions> ParseSourceItem(JToken jToken, SourceLoaderType sourceLoaderType)
    {
        if(jToken is null) throw new Exception("Неизвестный токен");
        ConnectionInfo connectionInfo = GetConnectionInfo(jToken["ConnectionInfo"]);
        return ParseSource(jToken["Source"],connectionInfo, sourceLoaderType);
    }

    private static IEnumerable<ILoadOptions> ParseSource(JToken jToken, ConnectionInfo connectionInfo, SourceLoaderType sourceLoaderType)
    {
        var res = new List<ILoadOptions>();
        if(jToken is null) throw new Exception("Неизвестный токен");
        var typeString = jToken["Type"]?.ToString();
        if(string.IsNullOrWhiteSpace(typeString)) throw new Exception("Не указан тип источника");
        var type = EnumHelper.GetEnumValue<SourceType>(typeString);

        if (type is null) throw new Exception("Неизвестный тип источника");

        foreach (var dataItem in JArray.FromObject(jToken["Data"]))
        {
            res.Add(ParseDataItem(type.Value, dataItem, connectionInfo, sourceLoaderType));
        }

        return res;
    }


    private static ILoadOptions ParseDataItem(SourceType sourceType, JToken dataItem, ConnectionInfo connectionInfo, SourceLoaderType sourceLoaderType)
    {
        switch (sourceLoaderType)
        {
            case SourceLoaderType.Dimension:
            {
                return ParseDimensionDataItem(sourceType, dataItem, connectionInfo);
            }
            case SourceLoaderType.Measure:
            {
                return ParseMeasureDataItem(sourceType, dataItem, connectionInfo);
            }
            case SourceLoaderType.Value:
            {
                return ParseMeasureValueDataItem(sourceType, dataItem, connectionInfo);
            }
            default:
            {
                throw new Exception("Неизвестный тип загрузчика");
            }
        }
    }

    private static ILoadOptions ParseDimensionDataItem(SourceType sourceType, JToken token, ConnectionInfo connectionInfo)
    {
        IEnumerable<ILoadOptions> values = new List<ILoadOptions>();
        if (token["Values"] != null) values = ParseValues(token["Values"]!, SourceLoaderType.Dimension);

        return sourceType switch
        {
            SourceType.Static => new DimensionStaticOptions(token[connectionInfo.KeyField]?.ToString() ?? "NULL",
                token[connectionInfo.NameField]?.ToString() ?? "Не задано", values),
            
            _ => throw new Exception("Неизвестный тип источника")
        };
    }

    

    private static ILoadOptions ParseMeasureDataItem(SourceType sourceType, JToken token,
        ConnectionInfo connectionInfo)
    {
        
        IEnumerable<ILoadOptions> values = new List<ILoadOptions>();
        if (token["Values"] != null) values = ParseValues(token["Values"]!, SourceLoaderType.Value);

        Type valueType = ValueTypeHelper.GetValueType(token["ValueType"]?.ToString() ?? nameof(StringValue));
        AggregateFunctionType aggregateFunction =
            EnumHelper.GetEnumValue<AggregateFunctionType>(token["Aggregation"]?.ToString() ?? "") ??
            AggregateFunctionType.Sum;
        
        return sourceType switch
        {
            SourceType.Static => new MeasureStaticOptions(token[connectionInfo.KeyField]?.ToString() ?? "NULL",
                token[connectionInfo.NameField]?.ToString() ?? "Не задано", valueType, aggregateFunction, values),
            
            _ => throw new Exception("Неизвестный тип источника")
        };
    }

    private static ILoadOptions ParseMeasureValueDataItem(SourceType sourceType, JToken token, ConnectionInfo connectionInfo)
    {
        IEnumerable<ILoadOptions> dimensions = new List<ILoadOptions>();
        if (token["Dimensions"] != null) dimensions = ParseValues(token["Dimensions"]!, SourceLoaderType.Dimension);
        
        return sourceType switch
        {
            SourceType.Static => new ValueStaticOptions(token["Id"]?.ToString() ?? "NULL",
                token["Value"]?.ToString() ?? "Не задано", dimensions),
            
            _ => throw new Exception("Неизвестный тип источника")
        };
    }
    
    private static IEnumerable<ILoadOptions> ParseValues(JToken jValues, SourceLoaderType loaderType)
    {
        var values = new List<ILoadOptions>();
        foreach (var valueItem in JArray.FromObject(jValues))
        {
            var value = ParseSourceItem(valueItem, loaderType);
            if (value is null) throw new Exception("Неправильная конфигурация значения измерения");
            values.AddRange(value);
        }
        return values;
    }
    
    private static ConnectionInfo GetConnectionInfo(JToken jToken)
    {
        var info = new ConnectionInfo();
        if (jToken is null) return info;
        var keyField = jToken["KeyField"];
        var nameField = jToken["NameField"];
        return new ConnectionInfo()
        {
            KeyField = keyField?.ToString() ?? info.KeyField,
            NameField = nameField?.ToString() ?? info.NameField,
        };
    }
}