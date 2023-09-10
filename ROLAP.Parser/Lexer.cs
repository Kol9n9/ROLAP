using ROLAP.Parser.Models.Expressions;
using ROLAP.Parser.Models.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser
{
    public class Lexer
    {
        private readonly Dictionary<string, TokenType> _operators = new Dictionary<string, TokenType>()
        {
            {"&",TokenType.AMPERSAND},
            {"(",TokenType.LPAREN},
            {")",TokenType.RPAREN},
            {"[",TokenType.LBRACKET},
            {"]",TokenType.RBRACKET},
            {"{",TokenType.LBRACE},
            {"}",TokenType.RBRACE},
            {",",TokenType.COMMA},
            {".",TokenType.DOT}
        };
        private readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>()
        {
            {"select",TokenType.SELECT},
            {"from",TokenType.FROM},
            {"on",TokenType.ON},
            {"where",TokenType.WHERE},
        };
        private readonly string _input;
        private int _pos;
        private int _oldGuidPos;
        public Lexer(string input)
        {
            _input = input;
            _pos = 0;
            _oldGuidPos = 0;
        }


        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            while (true)
            {
                char current = Peek(0);
                if (current == '\0') break;
                if (IsGuid(out string guid))
                {
                    tokens.Add(new Token(TokenType.WORD, guid));
                }
                else if (Char.IsDigit(current)) tokens.Add(TokenizeNumber());
                else if (_operators.ContainsKey(current.ToString())) tokens.Add(TokenizeOperator());
                else if (Char.IsLetter(current)) tokens.Add(TokenizeWord());
                else Next();
            }
            return tokens;
        }

        private Token TokenizeNumber()
        {
            string buffer = "";
            char current = Peek(0);
            while (Char.IsDigit(current) || current == '.')
            {
                if (current == '.' && buffer.IndexOf('.') != -1) throw new Exception("Invalid float number");
                buffer += current;
                current = Next();
            }
            if (buffer.Length == 0) throw new Exception("Error tokenize number");
            return new Token(TokenType.NUMBER, buffer);
        }

        private Token TokenizeWord()
        {
            string buffer = "";
            char current = Peek(0);
            while (true)
            {
                if (!Char.IsLetterOrDigit(current) && (current != '_'))
                {
                    break;
                }
                buffer += current;
                current = Next();
            }
            if (buffer.Length == 0) throw new Exception("Error tokenize word");

            var bufferKey = buffer.ToLower();

            if (_keywords.ContainsKey(bufferKey))
            {
                return new Token(_keywords[bufferKey], "");
            }
            else
            {
                return new Token(TokenType.WORD, buffer);
            }
        }

        private Token TokenizeOperator()
        {
            var current = _operators[Peek(0).ToString()];
            Next();
            return new Token(current, "");
        }
        private char Next()
        {
            _pos++;
            return Peek(0);
        }

        private char Peek(int offset)
        {
            int pos = _pos + offset;
            if (_pos >= _input.Length) return '\0';
            return _input[pos];
        }

        private bool IsGuid(out string guid)
        {
            guid = String.Empty;
            if (IsGuidTryBySymbols(ref guid, 36))
            {
                _oldGuidPos = _pos + 36;
                _pos += 36;
                return true;
            }
            if (IsGuidTryBySymbols(ref guid, 32))
            {
                _oldGuidPos = _pos + 32;
                _pos += 32;
                return true;
            }
            _oldGuidPos = _pos + 32;
            return false;      
        }

        private bool IsGuidTryBySymbols(ref string guid, int symbols) 
        {
            guid = String.Empty;
            string? buffer = new string(_input.Skip(_pos).Take(symbols).ToArray());
            if (string.IsNullOrEmpty(buffer)) return false;
            Guid guidBuffer;
            if(Guid.TryParse(buffer, out guidBuffer))
            {
                guid = guidBuffer.ToString();
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}
