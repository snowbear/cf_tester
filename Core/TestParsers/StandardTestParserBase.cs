using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Core;

namespace Core.TestParsers
{
	public abstract class StandardTestParserBase : ITestParser
	{
        public IEnumerable<TestData> Parse(string test)
        {
            return test.Split(InputMarkerTokens, StringSplitOptions.RemoveEmptyEntries)
                .Select((ss, i) =>
                {
                    var ss2 = ss
                        .Split(OutputMarkerTokens, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToArray();
                    return new TestData(ss2[0], ss2[1]);
                });
        }

        public bool CanParseTest(string testData)
        {
            return InputMarkerTokens.Any(testData.Contains);
        }

		protected abstract string[] InputMarkerTokens { get; }

		protected abstract string[] OutputMarkerTokens { get; }
	}
}