using ROLAP.Parser;
using ROLAP.Process;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        //var cubeRequest = Parser.Parse("SELECT CrossJoin(" +
        //    "{" +
        //    "[Dimension].&[706B4D3A-210A-4C7A-8E6C-36658A9712AB]," +
        //    "[Dimension].&[62E2E142-8A00-45AB-B8EA-A4CB277EB63F]" +
        //    "},CROSSJOIN({" +
        //    "[Dimension].&[2182f312-6e6b-42f9-adb0-f0165ba617c7]," +
        //    "[Dimension].&[902c2d8a-b6ac-4732-9918-6637af85dcba]" +
        //    "}," +
        //    "{" +
        //    "[Dimension].&[34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8]," +
        //    "[Dimension].&[FC9122BD-4075-42AC-8F29-B7CC44C843D0]" +
        //    "})) ON 0, [Product].[Color].&[Black] ON 1 FROM [Adventure_Cube]");

        //Parser.Parse("SELECT CrossJoin(" +
        //    "{" +
        //    "[Dimension].&[University]," +
        //    "[Dimension].&[FactAndPlan]" +
        //    "},CROSSJOIN({" +
        //    "[Dimension].&[TGU]," +
        //    "[Dimension].&[TPU]" +
        //    "}," +
        //    "{" +
        //    "[Dimension].&[Fact]," +
        //    "[Dimension].&[Plan]" +
        //    "})) ON 0, [Product].[Color].&[Black] ON 1 FROM [Adventure_Cube]");





        //Parser.Parse("SELECT {CROSSJOIN([Dimension].[University],[Dimension].[FactAndPlan]),CROSSJOIN({[Dimension].&[University],[Dimension].&[TGU],[Dimension].&[TPU]},{[Dimension].[FactAndPlan],[Dimension].[Fact],[Dimension].[Plan]})} ON 0, [Product].[Color].&[Black] ON 1 FROM [Adventure_Cube]");
        //Parser.Parse("SELECT CrossJoin([Product].[Color],{[Date].[2012],[Date].[2013]}) ON 0, [Product].[Color].&[Black] ON 1 FROM [Adventure_Cube]");

        var cubeRequest = Parser.Parse("SELECT CrossJoin([Dimension].&[62E2E142-8A00-45AB-B8EA-A4CB277EB63F],{[Dimension].&[34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8],[Dimension].&[FC9122BD-4075-42AC-8F29-B7CC44C843D0]}) ON 0, {[Dimension].&[3ac02e75-2988-4bd6-9471-80557bbbcc0d],[Dimension].&[82e6587a-6350-4cb5-ba12-b18174aaec26]} ON 1 FROM [Adventure_Cube]");

        CubeProcess process = new CubeProcess();
        process.Process(cubeRequest);
    }
}