namespace Core.TestParsers
{
    public class CodeChefTestsParser : StandardTestParserBase
    {
        protected override string[] InputMarkerTokens
        {
            get { return new[] { "Input:" }; }
        }

        protected override string[] OutputMarkerTokens
        {
            get { return new[] { "Output:" }; }
        }
    }
}
