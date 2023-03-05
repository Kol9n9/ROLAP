using ROLAP.Process;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        CubeProcess process = new CubeProcess();
        process.Process();
    }
}