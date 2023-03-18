using ROLAP.Parser;
using ROLAP.Process;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        Parser.Parse("SELECT {} ON 0, {} ON 1 FROM [Adventure_Cube]");
        CubeProcess process = new CubeProcess();
        process.Process();
    }
}