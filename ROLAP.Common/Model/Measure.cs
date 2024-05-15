using ROLAP.Common.Interfaces;

namespace ROLAP.Common.Model;

/// <summary>
/// Мера куба
/// </summary>
public class Measure : ICubeItem
{
    public IValuesLoader ValuesLoader { get; }
    public Measure(string key, string name, IValuesLoader valuesLoader) : base(key, name)
    {
        ValuesLoader = valuesLoader;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Measure measure)
        {
            return Equals(measure);
        }
        return false;
    }

    protected bool Equals(Measure other)
    {
        return other.Key == Key && other.Name == Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Name);
    }

    public override object Clone()
    {
        return new Measure(Key, Name, ValuesLoader);
    }
}