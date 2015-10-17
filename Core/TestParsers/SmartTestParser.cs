using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Core.TestParsers
{
    public class SmartTestParser
    {
        private readonly IEnumerable<ITestParser> _parsers;

        public SmartTestParser(params ITestParser[] parsers)
        {
            _parsers = parsers;
        }

        public IEnumerable<TestData> Parse(string test)
        {
            return _parsers.First(p => p.CanParseTest(test)).Parse(test);
        }
    }
}
