using ROLAP.Parser.Models.Expressions;
using ROLAP.Parser.Models.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROLAP.Common.Model.Interfaces;
using ROLAP.Parser.Models.Statements;
using ROLAP.Common.Model.Models;

namespace ROLAP.Parser
{
    public class Parser
    {
        private readonly Token _eof = new Token(TokenType.EOF, "");
        private readonly List<Token> _tokens = new List<Token>();
        private int _pos;
        public Parser(string input)
        {
            _tokens = new Lexer(input).Tokenize();
            _pos = 0;
        }

        public CubeQuery GetCubeQuery(IRepository repository)
        {
            var statement = Parse();
            return statement.Execute(repository);
        }
        
        private IStatement Parse()
        {
            var current = Get(0);
            if (Match(TokenType.SELECT))
            {
                return SelectStatement();
            }
            else
            {
                throw new Exception($"Unknown operation {current.Value}");
            }
        }

        private IStatement SelectStatement()
        {
            List<IExpression> expressions = new List<IExpression>();
            expressions.Add(AxisExpression()); 
            while (Match(TokenType.COMMA))
            {
                expressions.Add(AxisExpression());
            }

            if (!Match(TokenType.FROM))
                throw new Exception("Don't specified source");

            var cubeName = GetCubeName();

            return new SelectStatement(cubeName, expressions);
        }

        private string GetCubeName()
        {
            if (!Match(TokenType.LBRACKET))
                throw new Exception("Invalid Cube name format");

            var current = Get(0);
            
            if (current.Type != TokenType.WORD)
                throw new Exception("Invalid Cube name format");

            string cubeName = current.Value;

            Match(TokenType.WORD);
            
            if (!Match(TokenType.RBRACKET))
                throw new Exception("Invalid Cube name format");

            return cubeName;
        }
        
        private IExpression AxisExpression()
        {
            var expression = SetStatement();
            if (Get(0).Type != TokenType.ON)
            throw new Exception("Don't specified axis number");
            Match(TokenType.ON);
            int axisNumber = -1;
            if (Get(0).Type == TokenType.WORD)
            {
                throw new Exception("Invalid axis number format");
            } 
            else if (Get(0).Type == TokenType.NUMBER)
            {
                axisNumber = int.Parse(Get(0).Value);
                Match(TokenType.NUMBER);
            }
            else
            {
                throw new Exception("Invalid axis number format");
            }
            return new AxisExpression(axisNumber,expression);
        }
        private IExpression SetStatement()
        {
            IExpression res;
            if (Get(0).Type == TokenType.LBRACE) return SetExpression();
            if (Get(0).Type == TokenType.LPAREN) return TupleExpression();
            if (Get(0).Type == TokenType.WORD && Get(1).Type == TokenType.LPAREN) return SetFunctionExpression();
            return MemberExpression();
        }

        private IExpression TupleStatement()
        {
            //if (Get(0).Type == TokenType.WORD && Get(1).Type == TokenType.LPAREN) return SetFunctionExpression();
            return TupleExpression();
        }

        private IExpression SetExpression()
        {
            List<IExpression> res = new List<IExpression>();
            if (Get(0).Type == TokenType.LBRACE)
            {
                Match(TokenType.LBRACE);
                res.Add(SetStatement());
                while(Get(0).Type == TokenType.COMMA)
                {
                    Match(TokenType.COMMA);
                    res.Add(SetStatement());
                }
                Match(TokenType.RBRACE);
            }
            else
            {
                res.Add(SetStatement());
            }
            
            return new SetExpression(res);
        }
        private IExpression TupleExpression()
        {
            List<IExpression> res = new List<IExpression>(); 
            if (Get(0).Type == TokenType.LPAREN)
            {
                Match(TokenType.LPAREN);
                res.Add(MemberExpression());
                while(Get(0).Type == TokenType.COMMA)
                {
                    Match(TokenType.COMMA);
                    res.Add(MemberExpression());
                }
                Match(TokenType.RPAREN);
            }
            else
            {
                res.Add(MemberExpression());
            }
            
            return new TupleExpression(res);
        }
        private IExpression SetFunctionExpression()
        {
            string functionName = Get(0).Value;
            Match(TokenType.WORD);
            Match(TokenType.LPAREN);
            List<IExpression> expressions = new List<IExpression>();
            
            expressions.Add(SetStatement());
            while (Match(TokenType.COMMA))
            {
                expressions.Add(SetStatement());
            }
            
            Match(TokenType.RPAREN);

            return new SetFunctionExpression(functionName, expressions);
        }
       
        private IExpression MemberExpression()
        {
            if (Get(0).Type == TokenType.LBRACKET)
            {
                string dimensionName = GetIdentifier();
                if (dimensionName[0] == '&') throw new Exception("Dimension name start with \"&\".");
                List<string> names = new List<string>() { dimensionName};
                while (Get(0).Type == TokenType.DOT)
                {
                    Match(TokenType.DOT);
                    names.Add(GetIdentifier());
                }
                if (names.Count == 0) throw new Exception("Hierarchy is empty");
                return new MemberExpression(new Common.Model.Models.CubeQueryMember()
                {
                    Names = names
                });
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
