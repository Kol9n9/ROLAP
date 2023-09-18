using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.Token
{
    internal class Token
    {
        public TokenType Type { get; }
        public string Value { get; set; }
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
