using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.TestParsers;
using Core.Runners;

namespace Tester.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var pathToCode = args[0];
                Console.WriteLine("Source code: {0}", pathToCode);

                var testParser = new SmartTestParser(new CodeForcesTestParser(), new CodeChefTestsParser());
                var pathToTests = Path.ChangeExtension(pathToCode, ".txt");
                var testsContent = File.ReadAllText(pathToTests);
                var tests = testParser.Parse(testsContent);

                var runner = new SmartRunner(new IRunner[] {
                    new CppRunner(),
                    new CSharpRunner(),
                    new PythonRunner(),
                    new HaskellRunner(),
                });
                var results = runner.Run(pathToCode, tests);

                if (results != null)
                {
                    for (int i = 0; i < results.Count(); i++)
                    {
                        Console.WriteLine("Test {0}: {1}, running time: {2}", i + 1, results[i].IsCorrect ? "AC" : "WA", results[i].ExecutionResult.ExecutionTime);
                        if (!results[i].IsCorrect)
                        {
                            if (results[i].ExecutionResult.ExitCode != 0) Console.WriteLine("Process exit code: {0}", results[i].ExecutionResult.ExitCode);
                            Console.WriteLine("Input:");
                            Console.WriteLine(results[i].TestData.Input);
                            Console.WriteLine("Expected:");
                            Console.WriteLine(results[i].TestData.ExpectedOutput);
//                            Console.WriteLine("Actual:");
//                            Console.WriteLine(results[i].ActualOutput);
                            Console.WriteLine("Debug output:");
                            Console.WriteLine(results[i].ErrorOutput);
                        }
                    }
                }

                Console.WriteLine("Finished");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception was thrown");
                Console.WriteLine(e.ToString());
            }
            // Console.ReadKey();
        }
    }
}
