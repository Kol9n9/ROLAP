using Newtonsoft.Json.Linq;
using ROLAP.Common.Helpers;
using ROLAP.Configuration.Models.Enums;
using ROLAP.Configuration.Models.Models;
using ROLAP.Parser.Enums;

namespace ROLAP.Parser;

public static class CubeConfigurationParser
{
    public static CubeConfiguration ParseCubeConfiguration(string json)
    {
        JObject jObject = JObject.Parse(json);
        var dimensions = ParseDimensions(jObject["Dimensions"]);
        var measures = ParseMeasures(jObject["Measures"]);
        return new CubeConfiguration
        {
            DimensionOptions = dimensions,
            MeasureOptions = measures
        };
    }

    private static IEnumerable<CubeDimensionOptions> ParseDimensions(JToken jToken)
    {
        if(jToken is null) return new List<CubeDimensionOptions>();
        var res = new List<CubeDimensionOptions>();
        foreach (var sourceItem in JArray.FromObject(jToken))
        {
            res.AddRange(ParseSourceItem(sourceItem, SourceLoaderType.Dimension).Cast<CubeDimensionOptions>());
        }

        return res;
    }


    private static IEnumerable<CubeMeasureOptions> ParseMeasures(JToken jToken)
    {
        if(jToken is null) return new List<CubeMeasureOptions>();
        var res = new List<CubeMeasureOptions>();

        foreach (var sourceItem in JArray.FromObject(jToken))
        {
            res.AddRange(ParseSourceItem(sourceItem, SourceLoaderType.Measure).Cast<CubeMeasureOptions>());
        }
        
        return res;
    }

    private static IEnumerable<CubeBaseItemOptions> ParseSourceItem(JToken jToken, SourceLoaderType sourceLoaderType)
    {
        if(jToken is null) throw new Exception("Неизвестный токен");
        ConnectionInfo connectionInfo = GetConnectionInfo(jToken["ConnectionInfo"]);
        return ParseSource(jToken["Source"],connectionInfo, sourceLoaderType);
    }

    private static IEnumerable<CubeBaseItemOptions> ParseSource(JToken jToken, ConnectionInfo connectionInfo, SourceLoaderType sourceLoaderType)
    {
        var res = new List<CubeBaseItemOptions>();
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


    private static CubeBaseItemOptions ParseDataItem(SourceType sourceType, JToken dataItem,ConnectionInfo connectionInfo, SourceLoaderType sourceLoaderType)
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

    private static CubeDimensionOptions ParseDimensionDataItem(SourceType sourceType, JToken token, ConnectionInfo connectionInfo)
    {
        CubeDimensionOptions options;
        switch (sourceType)
        {
            case SourceType.Static:
            {
                var key = token[connectionInfo.KeyField];
                var name = token[connectionInfo.NameField];
                options = new StaticCubeDimensionOptions
                {
                    Key = key?.ToString() ?? "NULL",
                    Name = name?.ToString() ?? "Не задано"
                };
                break;
            }
            default:
            {
                throw new Exception("Неизвестный тип источника");
            }
        }

        options.ConnectionInfo = connectionInfo;
        
        if (token["Values"] != null)
        {
            var values = new List<CubeDimensionOptions>();
            foreach (var valueItem in JArray.FromObject(token["Values"]))
            {
                var value = ParseSourceItem(valueItem, SourceLoaderType.Dimension).Cast<CubeDimensionOptions>();
                if (value is null) throw new Exception("Неправильная конфигурация значения измерения");
                values.AddRange(value);
            }

            options.Values = values;
        }

        return options;
    }

    private static CubeMeasureOptions ParseMeasureDataItem(SourceType sourceType, JToken token,
        ConnectionInfo connectionInfo)
    {
        CubeMeasureOptions options;

        switch (sourceType)
        {
            case SourceType.Static:
            {
                var key = token[connectionInfo.KeyField];
                var name = token[connectionInfo.NameField];
                options = new StaticCubeMeasureOptions
                {
                    Key = key?.ToString() ?? "NULL",
                    Name = name?.ToString() ?? "Не задано"
                };
                break;
            }
            default:
            {
                throw new Exception("Неизвестный тип источника");
            }
        }
        
        if (token["Values"] != null)
        {
            var values = new List<CubeMeasureValueOptions>();
            foreach (var valueItem in JArray.FromObject(token["Values"]))
            {
                var value = ParseSourceItem(valueItem, SourceLoaderType.Value).Cast<CubeMeasureValueOptions>();
                if (value is null) throw new Exception("Неправильная конфигурация значения показателя");
                values.AddRange(value);
            }

            options.Values = values;
        }

        return options;
    }

    private static CubeMeasureValueOptions ParseMeasureValueDataItem(SourceType sourceType, JToken token,
        ConnectionInfo connectionInfo)
    {
        CubeMeasureValueOptions options;

        switch (sourceType)
        {
            case SourceType.Static:
            {
                var id = token["Id"];
                var value = token["Value"];
                options = new StaticCubeMeasureValueOptions
                {
                    Id = id?.ToString() ?? "NULL",
                    Value = value?.ToString() ?? "Не задано"
                };
                break;
            }
            default:
            {
                throw new Exception("Неизвестный тип источника");
            }
        }

        if (token["Dimensions"] != null)
        {
            var dimensions = new List<CubeDimensionOptions>();
            foreach (var dimensionItem in JArray.FromObject(token["Dimensions"]))
            {
                var value = ParseSourceItem(dimensionItem, SourceLoaderType.Dimension).Cast<CubeDimensionOptions>();
                if (value is null) throw new Exception("Неправильная конфигурация значения показателя");
                dimensions.AddRange(value);
            }

            options.Dimensions = dimensions;
        }

        return options;
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