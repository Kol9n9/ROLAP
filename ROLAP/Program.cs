using ROLAP.Parser;
using ROLAP.Process;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        Parser.Parse("SELECT CrossJoin([Product].[Color],[Date].[2012]) ON 0, [Product].[Color].&[Black] ON 1 FROM [Adventure_Cube]");
        CubeProcess process = new CubeProcess();
        process.Process();
    }
}