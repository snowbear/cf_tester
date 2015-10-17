using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Runners
{
    public class CSharpRunner : CompilableLanguageRunner
    {
        private const string _compilerPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\Csc.exe";

        private const string _parameters = ""
                                           + @""
                                           + " /out:{1} {0}";

        protected override string CompilerPath { get { return _compilerPath; } }

        protected override string CompilerParameters { get { return _parameters; } }

        public override string FileExtension
        {
            get { return "cs"; }
        }
    }
}
