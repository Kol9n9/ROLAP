using ROLAP.Parser.Models.Token;
using System.Text;

namespace ROLAP.Parser.Tests
{
    public class LexerTests
    {
        [Theory]
        [InlineData("2","Number: 2")]
        [InlineData("2.5","Number: 2.5")]
        [InlineData("1 2","Number: 1Number: 2")]
        public void LexerNumbers(string input, string expected)
        {
            //List<Token> tokens = new ROLAP.Parser.Lexer(input).Tokenize();
            //StringBuilder stringBuilder= new StringBuilder();
            //foreach (Token token in tokens)
            //{
            //    stringBuilder.Append(token.ToString());
            //}
            //Assert.Equal(stringBuilder.ToString(), expected);
        }
    }
}