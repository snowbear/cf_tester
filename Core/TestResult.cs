using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class TestResult
    {
        public TestData TestData { get; private set; }

        public ProcessExecuteResult ExecutionResult { get; private set; }

        public string ActualOutput { get; private set; }

        public string ErrorOutput { get; private set; }

        public bool IsCorrect { get; private set; }

        public TestResult(TestData testData)
        {
            TestData = testData;
        }

        public void SetOutput(ProcessExecuteResult executionResult)
        {
            ExecutionResult = executionResult;
            ActualOutput = executionResult.Output;
            ErrorOutput = executionResult.Error;
            IsCorrect = TestOutputEquals(ActualOutput, TestData.ExpectedOutput) && ExecutionResult.ExitCode == 0;
        }

        private static bool TestOutputEquals(string actual, string expected)
        {
            Func<string, List<string>> split = s =>
            {
                return s.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => str.Trim())
                    .Where(str => !string.IsNullOrWhiteSpace(str))
                    .ToList();
            };
            var actualLines = split(actual);
            var expectedLines = split(expected);
            return actualLines.SequenceEqual(expectedLines);
        }
    }
}
