using ROLAP.Parser.Model;

namespace ROLAP.Parser
{
    public static class Parser
    {
        public static void Parse(string mdx)
        {
            Scanner scanner = new Scanner(mdx);
            QueryState(scanner);
        }
        private static void QueryState(Scanner scanner)
        {
            Console.WriteLine("QueryState");
            var lexeme = scanner.GetLexeme();
            if(lexeme.Type != Model.LexemeType.SELECT)
            {
                throw new Exception("");
            }
            AxesState(scanner);
            lexeme = scanner.GetLexeme();
            if(lexeme.Type != Model.LexemeType.FROM)
            {
                throw new Exception("");
            }
            HierarchyIdentifierState(scanner);
            lexeme = scanner.GetLexeme();
            if(lexeme.Type != Model.LexemeType.FINISH)
            {
                throw new Exception("");
            }
        }
        private static void AxesState(Scanner scanner)
        {
            Console.WriteLine("AxesState");
            AxisState(scanner);
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.COMMA)
            {
                scanner.PutLexemeToStack(lexeme);
            } 
            else
            {
                while (lexeme.Type == LexemeType.COMMA)
                {
                    AxisState(scanner);
                    lexeme = scanner.GetLexeme();
                    if (lexeme.Type != LexemeType.COMMA)
                    {
                        scanner.PutLexemeToStack(lexeme);
                    }
                }
            }
            
        }
        private static void AxisState(Scanner scanner)
        {
            Console.WriteLine("AxisState");
            TupleState(scanner);
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.ON)
            {
                throw new Exception("");
            }
            lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.NUMBER)
            {
                throw new Exception("");
            }
        }
        private static void TupleState(Scanner scanner)
        {
            Console.WriteLine("TupleState");
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type == LexemeType.LEFT_BRACE)
            {
                scanner.PutLexemeToStack(lexeme);
                SetState(scanner);
            }
            else if(lexeme.Type == LexemeType.RIGHT_BRACE)
            {
                scanner.PutLexemeToStack(lexeme);
            }
            else if(lexeme.Type == LexemeType.IDENTIFIER)
            {
                scanner.PutLexemeToStack(lexeme);
                FuncState(scanner);
            }
            else if(lexeme.Type == LexemeType.LEFT_SQUARE_BRACKET)
            {
                scanner.PutLexemeToStack(lexeme);
                MemberState(scanner);
            }
            else
            {
                throw new Exception("");
            }

        }
        private static void FuncState(Scanner scanner)
        {
            
            Lexeme lexeme = scanner.GetLexeme();
            string funcName = lexeme.Value.ToString();
            Console.WriteLine("FuncState - " + funcName);
            lexeme = scanner.GetLexeme(); // (
            if(lexeme.Type != LexemeType.LEFT_ROUND_BRACKET)
            {
                throw new Exception("");
            }
            TupleState(scanner);
            lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.COMMA)
            {
                scanner.PutLexemeToStack(lexeme);
            }
            else
            {
                while (lexeme.Type == LexemeType.COMMA)
                {
                    TupleState(scanner);
                    lexeme = scanner.GetLexeme();
                    if (lexeme.Type != LexemeType.COMMA)
                    {
                        scanner.PutLexemeToStack(lexeme);
                    }
                }
            }
            lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.RIGHT_ROUND_BRACKET)
            {
                throw new Exception("");
            }
        }
        private static void SetState(Scanner scanner)
        {
            Console.WriteLine("SetState");
            Lexeme lexeme = scanner.GetLexeme();
            TupleState(scanner);
            lexeme = scanner.GetLexeme();
            if (lexeme.Type != LexemeType.COMMA)
            {
                scanner.PutLexemeToStack(lexeme);
            } 
            else
            {
                while (lexeme.Type == LexemeType.COMMA)
                {
                    TupleState(scanner);
                    lexeme = scanner.GetLexeme();
                    if (lexeme.Type != LexemeType.COMMA)
                    {
                        scanner.PutLexemeToStack(lexeme);
                    }
                }
            }
            lexeme= scanner.GetLexeme();
            if(lexeme.Type != LexemeType.RIGHT_BRACE)
            {
                throw new Exception("");
            }
        }
        private static void MemberState(Scanner scanner)
        {
            Console.WriteLine("MemberState");
            HierarchyItemState(scanner);
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.DOT)
            {
                scanner.PutLexemeToStack(lexeme);
            } 
            else
            {
                while(lexeme.Type == LexemeType.DOT)
                {
                    HierarchyItemState(scanner);
                    lexeme = scanner.GetLexeme();
                    if(lexeme.Type != LexemeType.DOT)
                    {
                        scanner.PutLexemeToStack(lexeme);
                    }
                }
            }
        }
        private static void HierarchyItemState(Scanner scanner)
        {
            Console.WriteLine("HierarchyItemState");
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type == LexemeType.IDENTIFIER)
            {
                scanner.PutLexemeToStack(lexeme);
                MemberFunc(scanner);
            }
            else if(lexeme.Type == LexemeType.AMPERSAND)
            {
                scanner.PutLexemeToStack(lexeme);
                HierarchyKeyState(scanner);
            }
            else if(lexeme.Type == LexemeType.LEFT_SQUARE_BRACKET)
            {
                scanner.PutLexemeToStack(lexeme);
                HierarchyNameState(scanner);
            }
        }
        private static void HierarchyNameState(Scanner scanner)
        {
            Console.WriteLine("HierarchyNameState");
            HierarchyIdentifierState(scanner);

        }
       
        private static void HierarchyKeyState(Scanner scanner)
        {
            Lexeme lexeme = scanner.GetLexeme(); // &
            Console.WriteLine("HierarchyKeyState");
            HierarchyIdentifierState(scanner);
        }
        private static void MemberFunc(Scanner scanner)
        {
            Lexeme lexeme = scanner.GetLexeme();
            Console.WriteLine("MemberFunc - " + lexeme.Value);
          
        }
        private static void HierarchyIdentifierState(Scanner scanner)
        {
            Lexeme lexeme = scanner.GetLexeme(); // [
            lexeme = scanner.GetLexeme();
            Console.WriteLine("IdentifierState - " + lexeme.Value);
            lexeme = scanner.GetLexeme(); // ]
        }
    }
}