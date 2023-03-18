using ROLAP.Parser.Model;
using System.Text.RegularExpressions;

namespace ROLAP.Parser
{
    internal static class LexicalAnalyzer
    {
        public static List<Lexeme> ParseLexemes(string mdx)
        {
            List<Lexeme> lexes = new List<Lexeme>();

            for (int i = 0; i < mdx.Length;)
            {
                if (IsSpace(mdx[i]))
                {
                    i++;
                    continue;
                }
                if (IsChar(mdx[i]))
                {
                    string word = "";
                    while (IsChar(mdx[i]) || IsDigit(mdx[i]))
                    {
                        word += mdx[i];
                        i++;
                        if (i >= mdx.Length)
                        {
                            break;
                        }
                    }
                    lexes.Add(new Lexeme()
                    {
                        Value = word,
                        Type = word.ToLower() switch
                        {
                            "select" => LexemeType.SELECT_COMMAND,
                            "on" => LexemeType.AXIS_ON,
                            "from" => LexemeType.SELECT_FROM,
                            "where" => LexemeType.SELECT_WHERE,
                            _ => IsFunction(word + mdx.Skip(i).ToString()) ? LexemeType.FUNCTION : LexemeType.WORD,
                        },
                    });
                    continue;
                }
                if (IsDigit(mdx[i]))
                {
                    string digit = "";
                    while (IsDigit(mdx[i]))
                    {
                        digit += mdx[i];
                        i++;
                        if (i >= mdx.Length)
                        {
                            break;
                        }
                    }
                    lexes.Add(new Lexeme()
                    {
                        Type = LexemeType.NUMBER,
                        Value = digit,
                    });
                    continue;
                }
                lexes.Add(new Lexeme()
                {
                    Type = mdx[i] switch
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
                        _ => throw new Exception("Неизвестный символ " + mdx[i].ToString())
                    },
                    Value = mdx[i].ToString()
                });
                i++;
            }
            lexes.Add(new Lexeme()
            {
                Type = LexemeType.FINISH,
                Value = "FINISH_LEXEME"
            });
            return lexes;
        }
        private static bool IsChar(char ch)
        {
            return ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z') || ch == '_';
        }
        private static bool IsDigit(char ch)
        {
            return ('0' <= ch && ch <= '9');
        }
        private static bool IsSpace(char ch)
        {
            return (ch == '\f') || (ch == '\n') || (ch == '\v') || (ch == '\r') || (ch == ' ');
        }
        private static bool IsFunction(string partQueryWithFunction)
        {
            return new Regex(@"[a-zA-Z]+[(]+.+[)]").Match(partQueryWithFunction).Success;
        }
    }
}
