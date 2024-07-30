using ROLAP.Core.Models.Interfaces.Value;

namespace ROLAP.Formatter.Models;

internal class ValueResult
{
    public IValue Value { get; }
    public string FormattedValue { get; }

    public ValueResult(IValue value, string formattedValue)
    {
        Value = value;
        FormattedValue = formattedValue;
    }
}