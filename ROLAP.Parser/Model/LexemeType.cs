using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Model
{
    internal enum LexemeType
    {
        EMPTY = 0,
        SELECT,
        FROM,
        ON,
        IDENTIFIER,
        NUMBER,
        FUNC,
        LEFT_BRACE, // {
        RIGHT_BRACE, // }
        LEFT_SQUARE_BRACKET, // [
        RIGHT_SQUARE_BRACKET, // ]
        LEFT_ROUND_BRACKET, // (
        RIGHT_ROUND_BRACKET, // )
        DOT, // .
        AMPERSAND,
        COMMA,
        FINISH
    }
}
