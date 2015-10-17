using System;
using System.Collections.Generic;

namespace Core.Runners
{
    public class PythonRunner : RunnerBase
    {
        public override string FileExtension { get { return "py"; } }

        public override List<TestResult> Run(string path, IEnumerable<TestData> tests)
        {
            return Execute("python", tests, path);
        }
    }
}
