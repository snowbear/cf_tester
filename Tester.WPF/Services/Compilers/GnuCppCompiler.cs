using Tester.Helpers;
using Tester.ViewModels;

namespace Tester.Services.Compilers
{
	public class GnuCppCompiler : ICompilerService
	{
		private const string _compilerPath = @"C:\Program Files\mingw-builds\x64-4.8.1-win32-seh-rev5\mingw64\bin\x86_64-w64-mingw32-g++";

		private const string _parameters = ""
                                           + @" -static -fno-optimize-sibling-calls -fno-strict-aliasing -DLOCAL -lm -s -x c++ -Wl,--stack=268435456 -O2 -std=c++11 -D__USE_MINGW_ANSI_STDIO=0 -o"
		                                   + " {1} {0}";

		private readonly ProcessLauncher _processLauncher = new ProcessLauncher();

		public Future<string> Compile(ProblemViewModel problem)
		{
			return _processLauncher.Launch(_compilerPath, null, _parameters,
			                               problem.ProblemPath, problem.CompilerExecutablePath)
				.Transform(processExecuteResult =>
					{
						if (processExecuteResult.ExitCode == 0) return (string) null;
						return processExecuteResult.Error;
					});
		}
	}
}