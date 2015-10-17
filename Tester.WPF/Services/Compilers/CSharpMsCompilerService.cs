using System;
using Tester.Helpers;
using Tester.ViewModels;

namespace Tester.Services.Compilers
{
	public class CSharpMsCompilerService : ICompilerService
	{
		private const string _cscPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
		private readonly ProcessLauncher _processLauncher = new ProcessLauncher();

		public Future<string> Compile(ProblemViewModel problem)
		{
            return _processLauncher.Launch(_cscPath, null, "/out:{1} \"{0}\" /r:System.Numerics.dll",
			                               problem.ProblemPath, problem.CompilerExecutablePath)
				.Transform(processExecuteResult =>
					{
						if (processExecuteResult.ExitCode == 0) return (string) null;
						return processExecuteResult.Output;
					});
		}
	}
}