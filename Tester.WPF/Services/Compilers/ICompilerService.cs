using Tester.Helpers;
using Tester.ViewModels;

namespace Tester.Services.Compilers
{
	public interface ICompilerService
	{
		Future<string> Compile(ProblemViewModel problem);
	}
}