namespace ROLAP.Common.Model;

/// <summary>
/// Базовый элемент в кубе
/// </summary>
public abstract class ICubeItem : ICloneable
{
    /// <summary>
    /// Ключ
    /// </summary>
    public string Key { get; }
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; }

    public ICubeItem(string key, string name)
    {
        Key = key;
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ICubeItem cubeItem)
        {
            return Equals(cubeItem);
        }

        return false;
    }

    protected bool Equals(ICubeItem other)
    {
        return Key == other.Key && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Name);
    }

    public abstract object Clone();
}