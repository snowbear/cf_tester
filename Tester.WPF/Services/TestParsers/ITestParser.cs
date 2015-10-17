using System.Collections.Generic;

namespace Tester.Services.TestParsers
{
	public interface ITestParser
	{
		IEnumerable<TestData> Parse(string path);
	}
}