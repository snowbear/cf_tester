using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Tester.Services.TestParsers
{
	public abstract class StandardTestParserBase : ITestParser
	{
		public IEnumerable<TestData> Parse(string path)
		{
			foreach (var j in Enumerable.Range(0, 5))
			{
				try
				{
					var fileContent = File.ReadAllText(path);
					return fileContent.Split(InputMarkerTokens, StringSplitOptions.RemoveEmptyEntries)
						.Select((ss, i) =>
							{
								var ss2 = ss
									.Split(OutputMarkerTokens, StringSplitOptions.RemoveEmptyEntries)
									.Select(s => s.Trim())
									.ToArray();
								return new TestData(ss2[0], ss2[1]);
							});
				}
				catch (FieldAccessException)
				{
					Thread.Sleep(500);
				}
			}
			throw new InvalidOperationException();
		}

		protected abstract string[] InputMarkerTokens { get; }

		protected abstract string[] OutputMarkerTokens { get; }
	}
}