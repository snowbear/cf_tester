using System;
using System.Collections.Generic;
using System.IO;
using Tester.ViewModels;

namespace Tester.Services.Compilers
{
	public class CompilerSelector
	{
		public static readonly CompilerSelector Instance = new CompilerSelector();

        private readonly IDictionary<string, ICompilerService> _compilers = new Dictionary<string, ICompilerService>();

        public CompilerSelector()
        {
            _compilers[".cs"] = new CSharpMsCompilerService();
            _compilers[".cpp"] = new GnuCppCompiler();
        }

		public ICompilerService GetCompiler(ProblemViewModel problem)
		{
			var extension = Path.GetExtension(problem.ProblemPath);
            return _compilers[extension];
		}

        internal bool ExtensionSupported(string extension)
        {
            return _compilers.ContainsKey(extension);
        }
    }
}