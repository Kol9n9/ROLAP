namespace ROLAP.Parser
{
    public static class Parser
    {
        public static void Parse(string mdx)
        {
            var lexes = LexicalAnalyzer.ParseLexemes(mdx);
        }
    }
}