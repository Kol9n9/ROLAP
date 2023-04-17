using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Model
{
    internal class Lexeme
    {
        public LexemeType Type { get; set; }
        public string Value { get; set; }
    }
}
