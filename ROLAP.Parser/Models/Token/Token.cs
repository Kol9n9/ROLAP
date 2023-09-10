using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.Token
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Enum.GetName(Type)}: {Value}";
        }
    }
}
