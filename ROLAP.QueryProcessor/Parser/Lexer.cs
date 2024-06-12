using System.Text;
using ROLAP.Parser.Enums;

namespace ROLAP.Parser.Models;

internal class Lexer
{
    private List<Token> _tokens = new List<Token>();
    private int _currentTokenIndex = 0;

    private static Dictionary<char, TokenType> _specialSymbols = new Dictionary<char, TokenType>
    {
        {'[',TokenType.LBracket},
        {']',TokenType.RBracket},
        {'(',TokenType.LParent},
        {')',TokenType.RParent},
        {'{',TokenType.LBrace},
        {'}',TokenType.RBrace},
        {'.',TokenType.Dot},
        {'&',TokenType.Ampersand},
        {'-',TokenType.Minus},
        {'+',TokenType.Plus},
        {',',TokenType.Comma},
    };

    private static Dictionary<string, TokenType> _specialKeyword = new Dictionary<string, TokenType>
    {
        {"select",TokenType.SELECT},
        {"on",TokenType.ON},
        {"from",TokenType.FROM},
        {"where", TokenType.WHERE},
    };

    private static readonly char[] DelimiterSymbols = new char[] { ' ' };

    public void Parse(string input)
    {
        Split(input);
        Merge();
        RemoveDelimiters();
    }

    public Token GetToken()
    {
        return _tokens[_currentTokenIndex];
    }

    public TokenType GetTokenType()
    {
        return _tokens[_currentTokenIndex].Type;
    }


    public bool Next()
    {
        if ( _currentTokenIndex + 1 >= _tokens.Count) return false;
        _currentTokenIndex++;
        return true;
    }

    private void Split(string input)
    {
        _currentTokenIndex = 0;
        _tokens.Clear();
        int pos = 0;

        while (pos < input.Length)
        {
            _tokens.Add(ParseToken(input,ref pos));
        }
        
        _tokens.Add(new Token(TokenType.EOF));
    }

    private Token ParseToken(string input, ref int pos)
    {
        if (IsDelimiter(input[pos]))
        {
            pos++;
            while (pos < input.Length && IsDelimiter(input[pos]))
            {
                pos++;
            }

            return new Token(TokenType.Delimiter);
        }

        StringBuilder sb = new StringBuilder();

        
        while (pos < input.Length && !IsSpecialSymbol(input[pos]) && !IsDelimiter(input[pos]))
        {
            sb.Append(input[pos++]);
        }

        TokenType tokenType;
        
        if (sb.Length != 0)
        {
            var buffer = sb.ToString();

            if (_specialKeyword.TryGetValue(buffer.ToLower(), out tokenType))
            {
                return new Token(tokenType);
            }
            
            tokenType = TokenType.Identifier;
            if (buffer.All(x => Char.IsDigit(x)))
            {
                tokenType = TokenType.Number;
            }
            
            return new Token(tokenType,buffer);
        }
        
        if (_specialSymbols.TryGetValue(input[pos], out tokenType))
        {
            pos++;
            return new Token(tokenType);
        }

        throw new Exception($"Неожиданный символ в позиции {pos}");
    }


    private bool IsSpecialSymbol(char symbol)
    {
        return _specialSymbols.ContainsKey(symbol);
    }

    private bool IsDelimiter(char symbol)
    {
        return DelimiterSymbols.Contains(symbol);
    }

    private void Merge()
    {
        int index = 0;
        while (index < _tokens.Count)
        {
            foreach (var mergeRule in _mergeRules)
            {
                var countRules = mergeRule.Key.Count;
                var tokens = _tokens.Skip(index).Take(countRules).ToList();
                if (IsRuleCondition(tokens, mergeRule.Key))
                {
                    Token token = Merge(tokens, mergeRule.Value,mergeRule.Key.Select(x => x.JoinString).ToArray());
                    for (int i = 0; i < countRules; i++)
                    {
                        _tokens.RemoveAt(index);
                    }
                    _tokens.Insert(index,token);
                }
            }
            index++;
        }
    }
    
    private bool IsRuleCondition(List<Token> tokens, List<MergeLexemeRuleItem> ruleItems)
    {
        if (ruleItems.Count != tokens.Count()) return false;
        for (int i = 0; i < ruleItems.Count; i++)
        {
            if (!ruleItems[i].IsType(tokens[i].Type)) return false;
        }
        return true;
    }

    private Token Merge(List<Token> tokens, TokenType type, string[] mergeStrings)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < tokens.Count; i++)
        {
            builder.Append(mergeStrings[i]);
            builder.Append(tokens[i].Value);
        }

        return new Token(type, builder.Length != 0 ? builder.ToString() : null);
    }

    private void RemoveDelimiters()
    {
        _tokens = _tokens.Where(x => x.Type != TokenType.Delimiter).ToList();
    }
    
    private static Dictionary<List<MergeLexemeRuleItem>, TokenType> _mergeRules = new Dictionary<List<MergeLexemeRuleItem>, TokenType>
    {
        {
            new List<MergeLexemeRuleItem> // GUID
            {
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Identifier,TokenType.Number}),
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Minus},"-"),
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Identifier,TokenType.Number}),
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Minus},"-"),
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Identifier,TokenType.Number}),
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Minus},"-"),            
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Identifier,TokenType.Number}),
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Minus},"-"),        
                new MergeLexemeRuleItem(new List<TokenType>{TokenType.Identifier,TokenType.Number}),
            },
            TokenType.Identifier 
        },
        // {
        //     new List<MergeLexemeRuleItem>
        //     {
        //         new MergeLexemeRuleItem(new List<TokenType> { TokenType.Identifier }),
        //         new MergeLexemeRuleItem(new List<TokenType> { TokenType.Delimiter }),
        //         new MergeLexemeRuleItem(new List<TokenType> { TokenType.Identifier }),
        //     }, TokenType.Identifier
        // }
    };

    private class MergeLexemeRuleItem
    {
        private TokenType[] _availableTypes;
        public string JoinString { get; }
        public MergeLexemeRuleItem(IEnumerable<TokenType> tokenTypes, string joinString = "")
        {
            JoinString = joinString;
            _availableTypes = tokenTypes.ToArray();
        }

        public bool IsType(TokenType type)
        {
            return _availableTypes.Contains(type);
        }
    }
}