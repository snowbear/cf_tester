using System.Collections.Generic;

namespace Core.TestParsers
{
	public interface ITestParser
	{
		IEnumerable<TestData> Parse(string test);
        bool CanParseTest(string test);
	}
}