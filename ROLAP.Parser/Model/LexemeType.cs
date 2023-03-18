using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Model
{
    internal enum LexemeType
    {
        SELECT_COMMAND = 0,
        AXIS_ON,
        SELECT_FROM,
        SELECT_WHERE,
        LEFT_BRACE, // {
        RIGHT_BRACE, // }
        LEFT_SQUARE_BRACKET, // [
        RIGHT_SQUARE_BRACKET, // ]
        LEFT_ROUND_BRACKET, // (
        RIGHT_ROUND_BRACKET, // )
        DOT, // .
        WORD, //
        FUNCTION,
        AMPERSAND,
        NUMBER,
        COMMA,
        FINISH
    }
}
