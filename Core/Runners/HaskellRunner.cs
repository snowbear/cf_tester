using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Runners
{
    public class HaskellRunner : CompilableLanguageRunner
    {
        private const string _compilerPath = @"ghc";

        private const string _parameters = "--make -O "
                                           + "-o {1} {0}";

        protected override string CompilerPath { get { return _compilerPath; } }

        protected override string CompilerParameters { get { return _parameters; } }

        public override string FileExtension
        {
            get { return "hs"; }
        }
    }
}
