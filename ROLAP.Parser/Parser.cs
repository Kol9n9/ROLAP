using ROLAP.Parser.InterpreterModel;
using ROLAP.Parser.Model;

namespace ROLAP.Parser
{
    public static class Parser
    {
        public static void Parse(string mdx)
        {
            Scanner scanner = new Scanner(mdx);
            CubeItem cube = QueryState(scanner);
            cube.Run();
            var request = cube.GetCubeRequest();
        }
        private static CubeItem QueryState(Scanner scanner)
        {
            
            Console.WriteLine("QueryState");
            var lexeme = scanner.GetLexeme();
            if(lexeme.Type != Model.LexemeType.SELECT)
            {
                throw new Exception("");
            }
            CubeItem cube = new CubeItem();
            cube.Axes.AddRange(AxesState(scanner));
            lexeme = scanner.GetLexeme();
            if(lexeme.Type != Model.LexemeType.FROM)
            {
                throw new Exception("");
            }
            cube.CubeName = HierarchyIdentifierState(scanner);
            lexeme = scanner.GetLexeme();
            if(lexeme.Type != Model.LexemeType.FINISH)
            {
                throw new Exception("");
            }
            return cube;
        }
        private static List<AxisItem> AxesState(Scanner scanner)
        {
            Console.WriteLine("AxesState");
            List<AxisItem> axes = new List<AxisItem>
            {
                AxisState(scanner)
            };
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.COMMA)
            {
                scanner.PutLexemeToStack(lexeme);
            } 
            else
            {
                while (lexeme.Type == LexemeType.COMMA)
                {
                    axes.Add(AxisState(scanner));
                    lexeme = scanner.GetLexeme();
                    if (lexeme.Type != LexemeType.COMMA)
                    {
                        scanner.PutLexemeToStack(lexeme);
                    }
                }
            }
            return axes;
            
        }
        private static AxisItem AxisState(Scanner scanner)
        {
            AxisItem axis = new AxisItem();
            Console.WriteLine("AxisState");
            axis.Tuples.AddRange(TupleState(scanner));
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
            axis.AxisNumber = int.Parse(lexeme.Value);
            return axis;
        }
        private static List<TupleItem> TupleState(Scanner scanner)
        {
            Console.WriteLine("TupleState");
            List<TupleItem> tuples = new List<TupleItem>();
            TupleItem tuple = new TupleItem();
            tuples.Add(tuple);
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type == LexemeType.LEFT_BRACE)
            {
                scanner.PutLexemeToStack(lexeme);
                tuple.Items.AddRange(SetState(scanner));
            }
            else if(lexeme.Type == LexemeType.RIGHT_BRACE)
            {
                scanner.PutLexemeToStack(lexeme);
            }
            else if(lexeme.Type == LexemeType.IDENTIFIER)
            {
                scanner.PutLexemeToStack(lexeme);
                tuple.Items.Add(FuncState(scanner));
            }
            else if(lexeme.Type == LexemeType.LEFT_SQUARE_BRACKET)
            {
                scanner.PutLexemeToStack(lexeme);
                tuple.Items.Add(MemberState(scanner));
            }
            else
            {
                throw new Exception("");
            }
            return tuples;

        }
        private static FuncItem FuncState(Scanner scanner)
        {
            
            Lexeme lexeme = scanner.GetLexeme();
            string funcName = lexeme.Value.ToString();
            FuncItem functionItem = new FuncItem();
            functionItem.Name = funcName;
            Console.WriteLine("FuncState - " + funcName);
            lexeme = scanner.GetLexeme(); // (
            if(lexeme.Type != LexemeType.LEFT_ROUND_BRACKET)
            {
                throw new Exception("");
            }
            functionItem.Items.AddRange(TupleState(scanner));
            lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.COMMA)
            {
                scanner.PutLexemeToStack(lexeme);
            }
            else
            {
                while (lexeme.Type == LexemeType.COMMA)
                {
                    functionItem.Items.AddRange(TupleState(scanner));
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
            return functionItem;
        }
        private static List<TupleItem> SetState(Scanner scanner)
        {
            Console.WriteLine("SetState");
            List<TupleItem> tuples= new List<TupleItem>();
            Lexeme lexeme = scanner.GetLexeme();
            tuples.AddRange(TupleState(scanner));
            lexeme = scanner.GetLexeme();
            if (lexeme.Type != LexemeType.COMMA)
            {
                scanner.PutLexemeToStack(lexeme);
            } 
            else
            {
                while (lexeme.Type == LexemeType.COMMA)
                {
                    tuples.AddRange(TupleState(scanner));
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
            return tuples;
        }
        private static MemberItem MemberState(Scanner scanner)
        {
            Console.WriteLine("MemberState");
            MemberItem member = new MemberItem();
            HierarchyItemState(scanner,member);
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type != LexemeType.DOT)
            {
                scanner.PutLexemeToStack(lexeme);
            } 
            else
            {
                while(lexeme.Type == LexemeType.DOT)
                {
                    HierarchyItemState(scanner, member);
                    lexeme = scanner.GetLexeme();
                    if(lexeme.Type != LexemeType.DOT)
                    {
                        scanner.PutLexemeToStack(lexeme);
                    }
                }
            }
            return member;
        }
        private static void HierarchyItemState(Scanner scanner, MemberItem member)
        {
            Console.WriteLine("HierarchyItemState");
            Lexeme lexeme = scanner.GetLexeme();
            if(lexeme.Type == LexemeType.IDENTIFIER)
            {
                scanner.PutLexemeToStack(lexeme);
                MemberFunc(scanner,member);
            }
            else if(lexeme.Type == LexemeType.AMPERSAND)
            {
                scanner.PutLexemeToStack(lexeme);
                HierarchyKeyState(scanner, member);
            }
            else if(lexeme.Type == LexemeType.LEFT_SQUARE_BRACKET)
            {
                scanner.PutLexemeToStack(lexeme);
                HierarchyNameState(scanner, member);
            }
        }
        private static void HierarchyNameState(Scanner scanner, MemberItem member)
        {
            Console.WriteLine("HierarchyNameState");
            member.Hierarchy.Add(HierarchyIdentifierState(scanner));

        }
       
        private static void HierarchyKeyState(Scanner scanner, MemberItem member)
        {
            Lexeme lexeme = scanner.GetLexeme(); // &
            Console.WriteLine("HierarchyKeyState");
            member.Hierarchy.Add("&"+HierarchyIdentifierState(scanner));
        }
        private static void MemberFunc(Scanner scanner,MemberItem member)
        {
            Lexeme lexeme = scanner.GetLexeme();
            Console.WriteLine("MemberFunc - " + lexeme.Value);
            member.FuncName = lexeme.Value;
        }
        private static string HierarchyIdentifierState(Scanner scanner)
        {
            Lexeme lexeme = scanner.GetLexeme(); // [
            lexeme = scanner.GetLexeme();
            string value = lexeme.Value;
            Console.WriteLine("IdentifierState - " + value);
            lexeme = scanner.GetLexeme(); // ]
            return value;
        }
    }
}