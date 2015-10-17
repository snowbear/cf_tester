using System.Collections.Generic;

namespace Core.Runners
{
    public interface IRunner
    {
        bool Matches(string extension);
        List<TestResult> Run(string path, IEnumerable<TestData> tests);
    }
}
