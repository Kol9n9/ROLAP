using ROLAP.Parser.Model;
using System.Text.RegularExpressions;

namespace ROLAP.Parser
{
    internal class Scanner
    {
        private string query;
        private int position;
        private Stack<Lexeme> lexemeStack = new Stack<Lexeme>();
        private Regex guidRegexp;
        public Scanner(string query) {
            this.query = query;
            position = 0;
            guidRegexp = new Regex("^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$");

        }
        private char GetNextSymbol()
        {
            return query[position++];
        }
        private bool IsEndQueryString()
        {
            return position >= query.Length;
        }
        public void PutLexemeToStack(Lexeme lexeme)
        {
            lexemeStack.Push(lexeme);
        }
        public Lexeme GetLexeme()
        {
            string buffer = "";
            if(lexemeStack.Count > 0)
            {
                return lexemeStack.Pop();
            }

            if (IsCurrentPositionGuid())
            {
                buffer = string.Join("", query.Skip(position).Take(36));
                position += 36;
                return new Lexeme()
                {
                    Type = LexemeType.IDENTIFIER,
                    Value = buffer
                };
            }
            while (!IsEndQueryString())
            {
                char symbol = GetNextSymbol();
                if (IsSpace(symbol))
                {
                    continue;
                }
                if(IsChar(symbol))
                {
                    while(IsChar(symbol) || IsDigit(symbol))
                    {
                        buffer+= symbol;
                        symbol = GetNextSymbol();
                    }
                    position--;
                    return new Lexeme()
                    {
                        Type = buffer.ToLower() switch
                        {
                            "select" => LexemeType.SELECT,
                            "from" => LexemeType.FROM,
                            "on" => LexemeType.ON,
                            _ => LexemeType.IDENTIFIER
                        },
                        Value = buffer

                    };
                }
                if (IsDigit(symbol))
                {
                    while(IsDigit(symbol))
                    {
                        buffer += symbol;
                        symbol = GetNextSymbol();
                    }
                    position--;
                    return new Lexeme()
                    {
                        Type = LexemeType.NUMBER,
                        Value = buffer

                    };
                }
                return new Lexeme()
                {
                    Type = symbol switch
                    {
                        '{' => LexemeType.LEFT_BRACE,
                        '}' => LexemeType.RIGHT_BRACE,
                        '[' => LexemeType.LEFT_SQUARE_BRACKET,
                        ']' => LexemeType.RIGHT_SQUARE_BRACKET,
                        '(' => LexemeType.LEFT_ROUND_BRACKET,
                        ')' => LexemeType.RIGHT_ROUND_BRACKET,
                        '.' => LexemeType.DOT,
                        '&' => LexemeType.AMPERSAND,
                        ',' => LexemeType.COMMA,
                        _ => throw new Exception("Неизвестный символ " + symbol.ToString())
                    },
                    Value = symbol.ToString()
                };
            }
            return new Lexeme()
            {
                Type = LexemeType.FINISH,
                Value = ""
            };
        }

        private bool IsCurrentPositionGuid()
        {
            string buffer = string.Join("",query.Skip(position).Take(36));
            return guidRegexp.IsMatch(buffer);
        }

        private static bool IsChar(char ch)
        {
            return ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z') || ch == '_' || ch == '-';
        }
        private static bool IsDigit(char ch)
        {
            return ('0' <= ch && ch <= '9');
        }
        private static bool IsSpace(char ch)
        {
            return (ch == '\f') || (ch == '\n') || (ch == '\v') || (ch == '\r') || (ch == ' ');
        }
    }
}
