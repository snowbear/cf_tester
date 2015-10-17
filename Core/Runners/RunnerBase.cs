using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Runners
{
    public abstract class RunnerBase : IRunner
    {
        private readonly ProcessLauncher _processLauncher = new ProcessLauncher();

        public abstract string FileExtension { get; }

        public bool Matches(string extension)
        {
            return extension.Equals(FileExtension, StringComparison.InvariantCultureIgnoreCase);
        }

        public abstract List<TestResult> Run(string path, IEnumerable<TestData> tests);

        protected List<TestResult> Execute(string exePath, IEnumerable<TestData> tests, params string[] parameters) {
            var results = new List<TestResult>();
            foreach (var test in tests)
            {
                var testResult = new TestResult(test);
                var executionResult = _processLauncher.Launch(exePath, test.Input, parameters);
                testResult.SetOutput(executionResult);
                results.Add(testResult);
            }

            return results;
        }
    }
}
