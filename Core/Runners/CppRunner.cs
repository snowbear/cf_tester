using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Runners
{
    public class CppRunner : CompilableLanguageRunner
    {
		private const string _compilerPath = @"C:\Program Files\mingw-builds\x64-4.8.1-win32-seh-rev5\mingw64\bin\x86_64-w64-mingw32-g++";

		private const string _parameters = ""
                                           + @" -static -fno-optimize-sibling-calls -fno-strict-aliasing -DLOCAL -lm -s -x c++ -Wl,--stack=268435456 -O2 -std=c++11 -D__USE_MINGW_ANSI_STDIO=0 -o"
		                                   + " {1} {0}";

        protected override string CompilerPath { get { return _compilerPath; } }
        
        protected override string CompilerParameters { get { return _parameters; } }

        public override string FileExtension
        {
            get { return "cpp"; }
        }
    }
}
