using ROLAP.Process;

public class Program
{
    public static void Main(string[] args)
    {
        string mdx = "SELECT CrossJoin([Dimension].&[62E2E142-8A00-45AB-B8EA-A4CB277EB63F],{[Dimension].&[34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8],[Dimension].&[FC9122BD-4075-42AC-8F29-B7CC44C843D0]}) ON 0, " +
            "{[Dimension].&[3ac02e75-2988-4bd6-9471-80557bbbcc0d],[Dimension].&[82e6587a-6350-4cb5-ba12-b18174aaec26]} ON 1 " +
            "FROM [Adventure_Cube]";

        CubeProcess process = new CubeProcess();
        process.Process(mdx);
    }
}