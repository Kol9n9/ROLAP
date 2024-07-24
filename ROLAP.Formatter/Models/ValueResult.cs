namespace ROLAP.Formatter.Models;

internal class ValueResult
{
    public string Value { get; }
    public string FormattedValue { get; }

    public ValueResult(string value, string formattedValue)
    {
        Value = value;
        FormattedValue = formattedValue;
    }
}