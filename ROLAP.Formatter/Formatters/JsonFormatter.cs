using Newtonsoft.Json;
using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.Query;
using ROLAP.Formatter.Helpers;
using ROLAP.Formatter.Models;

namespace ROLAP.Formatter.Formatters;

public static class JsonFormatter
{
    public static string Format(IEnumerable<CubeItemSet> sets, IEnumerable<ICubeItem> values)
    {
        List<SetResult> setResults = new List<SetResult>();
        List<ValueResult> valueResults = new List<ValueResult>();
        foreach (var set in sets)
        {
            setResults.Add(MappingHelper.Map(set));
        }

        foreach (var value in values.Cast<IValueCubeItem>())
        {
            valueResults.Add(MappingHelper.Map(value));
        }

        CubeResult cubeResult = new CubeResult(setResults, valueResults);
        
        var json = JsonConvert.SerializeObject(cubeResult);
        return json;
    }
}