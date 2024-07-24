namespace ROLAP.Formatter.Models;

public class MemberResult
{
    public string Name { get; }
    public string Key { get; }
    public MemberResult(string name, string key)
    {
        Name = name;
        Key = key;
    }
}