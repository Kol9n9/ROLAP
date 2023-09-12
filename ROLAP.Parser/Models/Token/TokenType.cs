using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.Token
{
    internal enum TokenType
    {
        EOF = -1,

        NUMBER, // 0-9
        WORD, // World
        
        AMPERSAND, // &
        LPAREN, // (
        RPAREN, // )
        LBRACKET, // [
        RBRACKET, // ]
        LBRACE, // {
        RBRACE, // }
        COMMA, // ,
        DOT, // .

        SELECT,
        ON,
        FROM,
        WHERE
    }
}
