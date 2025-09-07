using ROLAP.Core.Models.Enums;
using ROLAP.Parser.Enums;
using ROLAP.Parser.Models;
using ROLAP.QueryProcessor.Helpers;
using ROLAP.QueryProcessor.Interfaces;
using ROLAP.QueryProcessor.Models;
using ROLAP.QueryProcessor.Models.Items;

namespace ROLAP.QueryProcessor.Parser;

internal static class QueryParser
{
    private static Lexer _lexer = new Lexer();
    public static QueryModel Parse(string input)
    {
        _lexer.Parse(input);
        if (MatchToken(TokenType.SELECT))
        {
            return GetSelectQuery();
        }

        throw new Exception("Неизвестная операция для выполнения");
    }

    private static QueryModel GetSelectQuery()
    {
        ThrowIfNextTokenFailed();

        List<AxisItem> axes = new List<AxisItem>
        {
            AxisQuery()
        };
        
        while (MatchToken(TokenType.Comma))
        {
            ThrowIfNextTokenFailed();
            axes.Add(AxisQuery());
        }

        ThrowIfTokenTypeNotValid(TokenType.FROM);
        ThrowIfNextTokenFailed();

        string cubeName = GetCubeName();

        List<AxisItem> where = new List<AxisItem>();
        if (MatchToken(TokenType.WHERE))
        {
            ThrowIfNextTokenFailed();
            where.Add(WhereAxisQuery());
        }
        
        ThrowIfTokenTypeNotValid(TokenType.EOF);
        return new QueryModel(cubeName,QueryType.Select, axes, where);
    }

    private static AxisItem AxisQuery()
    {
        var member = SetQuery();

        ThrowIfTokenTypeNotValid(TokenType.ON);
        ThrowIfNextTokenFailed();

        int number;
        if (MatchToken(TokenType.Number))
        {
            if (!TryGetInt(GetToken(), out number))
            {
                throw new Exception("Не указан идентификатор оси");
            }
            ThrowIfNextTokenFailed();
        }
        else if (MatchToken(TokenType.Identifier))
        {
            throw new NotSupportedException("Символьное обозначение идентификатора оси временно не доступно");
        }
        else
        {
            throw new Exception("Не указан идентификатор оси");
        }

        return new AxisItem(member, number);
    }

    private static AxisItem WhereAxisQuery()
    {
        var item = SetQuery();
        return new AxisItem(item, -1);
    }
    
    private static IQueryItem SetQuery()
    {
        if (MatchToken(TokenType.LBrace))
        {
            return GetSet();
        }

        if (MatchToken(TokenType.LParent))
        {
            return GetTuple();
        }

        if (MatchToken(TokenType.Identifier))
        {
             return GetFunction();
        }
        
        return GetMember();
    }

    private static IQueryItem GetSet()
    {
        ThrowIfTokenTypeNotValid(TokenType.LBrace);
        ThrowIfNextTokenFailed();
        
        List<IQueryItem> res = new List<IQueryItem>()
        {
            SetQuery()
        };

        while (MatchToken(TokenType.Comma))
        {
            ThrowIfNextTokenFailed();
            res.Add(SetQuery());
        }

        ThrowIfTokenTypeNotValid(TokenType.RBrace);
        ThrowIfNextTokenFailed();

        return new SetItem(res);
    }

    private static IQueryItem GetTuple()
    {
        ThrowIfTokenTypeNotValid(TokenType.LParent);
        ThrowIfNextTokenFailed();
        
        var items = new List<IQueryItem>
        {
            GetMember()
        };

        while (MatchToken(TokenType.Comma))
        {
            ThrowIfNextTokenFailed();
            items.Add(GetMember());
        }
        
        ThrowIfTokenTypeNotValid(TokenType.RParent);
        ThrowIfNextTokenFailed();
        
        return new TupleItem(items);
    }

    private static IQueryItem GetFunction()
    {
        string name;
        if (!TryGetIdentifier(GetToken(), out name))
        {
            throw new Exception("");
        }
        ThrowIfNextTokenFailed();
        ThrowIfTokenTypeNotValid(TokenType.LParent);
        ThrowIfNextTokenFailed();

        List<IQueryItem> args = new List<IQueryItem>
        {
            SetQuery()
        };

        while (MatchToken(TokenType.Comma))
        {
            ThrowIfNextTokenFailed();
            args.Add(SetQuery());
        }
        
        ThrowIfTokenTypeNotValid(TokenType.RParent);
        ThrowIfNextTokenFailed();

        return FunctionHelper.GetFunction(name, args);
    }

    private static IQueryItem GetMember()
    {
        List<string> hierarchy = new List<string>();
        string memberFunctionName = string.Empty;
        hierarchy.Add(GetHierarchyIdentifier());

        while (MatchToken(TokenType.Dot))
        {
            ThrowIfNextTokenFailed();
            if (!MatchToken(TokenType.Ampersand) && !MatchToken(TokenType.LBracket)) // MemberFunction
            {
                if (!TryGetIdentifier(GetToken(), out memberFunctionName))
                {
                    throw new Exception("");
                }
                ThrowIfNextTokenFailed();
            }
            else
            {
                hierarchy.Add(GetHierarchyIdentifier());
            }
        }
        
        return new MemberItem(hierarchy.ToArray(),memberFunctionName);
    }

    private static bool MatchToken(TokenType type) => _lexer.GetTokenType() == type;

    private static Token GetToken() => _lexer.GetToken();

    private static bool NextToken() => _lexer.Next();



    private static string GetCubeName()
    {
        return GetHierarchyIdentifier();
    }
    
    private static string GetHierarchyIdentifier()
    {
        if (MatchToken(TokenType.Ampersand))
        {
            ThrowIfNextTokenFailed();
            // key;
        }

        ThrowIfTokenTypeNotValid(TokenType.LBracket);
        ThrowIfNextTokenFailed();

        string name = "";


        if (MatchToken(TokenType.Identifier) || MatchToken(TokenType.Number))
        {
            if (!TryGetIdentifier(GetToken(), out name))
            {
                throw new Exception("Не указано название иерархии");
            }
        }

        ThrowIfNextTokenFailed();

        while (MatchToken(TokenType.Identifier) || MatchToken(TokenType.Number))
        {
            if (TryGetIdentifier(GetToken(), out var name2))
            {
                name += " " + name2;
            }
            ThrowIfNextTokenFailed();
        }
        
        
        ThrowIfTokenTypeNotValid(TokenType.RBracket);
        ThrowIfNextTokenFailed();
        
        return name;
    }

    
    private static bool TryGetInt(Token token, out int value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(token.Value)) return false;
        return int.TryParse(token.Value, out value);
    }
    
    private static bool TryGetIdentifier(Token token, out string value)
    {
        value = String.Empty;
        value = token.Value;
        if (string.IsNullOrWhiteSpace(value)) return false;
        return true;
    }

    private static void ThrowIfNextTokenFailed()
    {
        if (!NextToken()) throw new Exception("");
    }

    private static void ThrowIfTokenTypeNotValid(TokenType type)
    {
        if (!MatchToken(type)) throw new Exception("");
    }
}