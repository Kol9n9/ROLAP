using ROLAP.Parser.Enums;

namespace ROLAP.Parser.Models;

internal class Token
{
    public TokenType Type { get; }
    public string? Value { get; }

    public Token(TokenType type, string? value = null)
    {
        Type = type;
        Value = value;
    }
}