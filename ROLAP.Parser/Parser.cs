using ROLAP.Parser.Models.CubeRequest;
using ROLAP.Parser.Models.Expressions;
using ROLAP.Parser.Models.ExpressionValues;
using ROLAP.Parser.Models.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser
{
    public class Parser
    {
        private readonly Token _eof = new Token(TokenType.EOF, "");
        private readonly List<Token> _tokens = new List<Token>();
        private int _pos;
        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _pos = 0;
        }

        public TupleExpression Parse()
        {
            var res = TupleExpression();
            res.Execute();
            return res;
        }

        private TupleExpression TupleExpression()
        {
            List<IExpression> res = new List<IExpression>(); 
            if (Get(0).Type == TokenType.LBRACE)
            {
                Match(TokenType.LBRACE);
                res.Add(MemberExpression());
                while(Get(0).Type == TokenType.COMMA)
                {
                    Match(TokenType.COMMA);
                    res.Add(MemberExpression());
                }
            }
            else
            {
                res.Add(MemberExpression());
            }
            return new Models.ExpressionValues.TupleExpression(res);
        }

        private MemberExpression MemberExpression()
        {
            if (Get(0).Type == TokenType.LBRACKET)
            {
                string dimensionName = GetIdentifier();
                if (dimensionName[0] == '&') throw new Exception("Dimension name start with \"&\".");
                List<string> names = new List<string>();
                while (Get(0).Type == TokenType.DOT)
                {
                    Match(TokenType.DOT);
                    names.Add(GetIdentifier());
                }
                if (names.Count == 0) throw new Exception("Hierarchy is empty");
                return new MemberExpression(new Models.ExpressionValues.DimensionMemberValue(new CubeRequestAxisMember(dimensionName,names)));
            }
            throw new Exception("Unknown expression");
        }

        private string GetIdentifier()
        {
            var current = Get(0);
            bool isKey = false;
            if(current.Type != TokenType.LBRACKET && current.Type != TokenType.AMPERSAND) throw new Exception($"Expected \"[\" or \"&\" but gived: {Get(0).Value}");
            if(current.Type == TokenType.AMPERSAND)
            {
                Match(TokenType.AMPERSAND);
                isKey = true;
            }
            Match(TokenType.LBRACKET);
            string identifier = Get(0).Value;
            if (isKey) identifier = "&" + identifier;
            Match(TokenType.WORD);
            Match(TokenType.RBRACKET);
            return identifier;
        }

        /// <summary>
        /// Проверяет текущий токен на ожидаемый тип
        /// </summary>
        /// <param name="type"></param>
        private bool Match(TokenType type)
        {
            Token token = Get(0);
            if (token.Type != type) return false;
            _pos++;
            return true;
        }

        private Token Get(int offset)
        {
            int pos = _pos + offset;
            if (pos >= _tokens.Count) return _eof;
            return _tokens[pos];
        }
    }
}
