namespace ROLAP.Common.Helpers;

public static class EnumHelper
{
    public static TEnum? GetEnumValue<TEnum>(string name) where TEnum : struct
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        if (Enum.TryParse<TEnum>(name,true, out var res))
        {
            return res;
        }

        return null;
    }
}