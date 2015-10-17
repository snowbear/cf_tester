using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Runners
{
    public abstract class CompilableLanguageRunner : RunnerBase
    {
        private readonly ProcessLauncher _processLauncher = new ProcessLauncher();

        protected abstract string CompilerPath { get; }

        /// <summary>
        /// 0-th parameter is input file path
        /// 1-st parameter is expected output path
        /// </summary>
        protected abstract string CompilerParameters { get; }

        public override List<TestResult> Run(string path, IEnumerable<TestData> tests)
        {
            var compiledFileName = Path.Combine(Path.GetTempPath(), "program1.exe");
            Console.Write("Compiling ... ");
            var compilationResult = _processLauncher.Launch(CompilerPath, null, CompilerParameters, path, compiledFileName);
            if (compilationResult.ExitCode == 0)
            {
                Console.WriteLine("success");
            }
            else
            {
                Console.WriteLine("failed");
                Console.WriteLine(compilationResult.Error);
                return null;
            }

            return Execute(compiledFileName, tests);
        }
    }
}
