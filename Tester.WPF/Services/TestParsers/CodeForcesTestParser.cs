namespace Tester.Services.TestParsers
{
	public class CodeForcesTestParser : StandardTestParserBase
	{
		protected override string[] InputMarkerTokens
		{
			get { return new[] {"входные данные", "input"}; }
		}

		protected override string[] OutputMarkerTokens
		{
			get { return new[] {"выходные данные", "output"}; }
		}
	}
}