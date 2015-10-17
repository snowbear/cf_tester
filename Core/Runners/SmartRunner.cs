using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Runners
{
    public class SmartRunner
    {
        private readonly IEnumerable<IRunner> _runners;

        public SmartRunner(IEnumerable<IRunner> runners)
        {
            _runners = runners;
        }

        public List<TestResult> Run(string path, IEnumerable<TestData> tests)
        {
            var extension = Path.GetExtension(path).TrimStart('.');
            var runner = _runners.First(r => r.Matches(extension));
            return runner.Run(path, tests);
        }
    }
}
