using ROLAP.Core.Models.Enums;
using ROLAP.Core.Models.Interfaces.Loader;

namespace ROLAP.Loaders.Models.Options;

internal class MeasureStaticOptions : ILoadOptions
{
    public string Key { get; }
    public string Name { get; }
    public IEnumerable<ILoadOptions> ValuesOptions { get; }
    
    public Type ValueType { get; }
    public AggregateFunctionType AggregateFunctionType { get; }

    public MeasureStaticOptions(string key, string name, Type valueType, AggregateFunctionType functionType, IEnumerable<ILoadOptions> valuesOptions)
    {
        Key = key;
        Name = name;
        ValueType = valueType;
        AggregateFunctionType = functionType;
        ValuesOptions = valuesOptions;
    }
}