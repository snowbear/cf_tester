using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Tester.Helpers;
using Tester.Models;
using Tester.Services;
using Tester.Services.Compilers;
using Tester.Services.TestParsers;

namespace Tester.ViewModels
{
	public class ProblemViewModel : ModelBase
	{
		private readonly MainViewModel _mainViewModel;
		private readonly string _testsPath;
		private readonly ICompilerService _compiler;
		private readonly ITestParser _testParser = new CodeForcesTestParser();
		private string _statusText;
		private object _details;
		private bool _compiled;

		public string Name { get; set; }

		public StateViewModel State { get; private set; }

		public string StatusText
		{
			get { return _statusText; }
			private set
			{
				if (value == _statusText) return;
				_statusText = value;
				OnPropertyChanged("StatusText");
			}
		}

		public ObservableCollection<TestViewModel> Tests { get; private set; }

		public string ProblemPath { get; private set; }

		public string CompilerExecutablePath { get; private set; }

		public object Details
		{
			get { return _details; }
			private set
			{
				if (Equals(value, _details)) return;
				_details = value;
				OnPropertyChanged("Details");
			}
		}

		public ProblemViewModel(MainViewModel mainViewModel, string problemPath)
		{
			_mainViewModel = mainViewModel;
			ProblemPath = problemPath;
			_testsPath = Path.ChangeExtension(ProblemPath, "txt");
			CompilerExecutablePath = Path.Combine(Path.GetTempPath(), string.Format("{0}.exe", Path.GetFileNameWithoutExtension(problemPath)));

			State = new StateViewModel();
			Tests = new ObservableCollection<TestViewModel>();

			FileChangesTracker.Instance.FileChanged += Instance_FileChanged;

			_compiler = CompilerSelector.Instance.GetCompiler(this);

			UpdateSource()
				.OnComplete(res =>
					{
						if (res == null) UpdateTests();
					});
		}

		private void Instance_FileChanged(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath == ProblemPath)
			{
				UpdateSource()
					.OnComplete(res =>
						{
							if (res == null) RunAllTests();
						});
			}
			else if (e.FullPath == _testsPath)
				UpdateTests();
		}

		private Future<string> UpdateSource()
		{
			ProgressViewModel.Instance.Text.Value = "Compiling";
			return _compiler.Compile(this)
				.OnComplete(ContinueAfterCompilation);
		}

		private void ContinueAfterCompilation(string compilationResult)
		{
			if (compilationResult == null)
			{
				State.SetSuccess();
				Details = new ProblemDetailsViewModel(this);
				_compiled = true;
				ProgressViewModel.Instance.Text.Value = "Finished compiling";
			}
			else
			{
				State.SetError();
				Details = "Compilation error:\n\n" + compilationResult;
				ProgressViewModel.Instance.Text.Value = "Compilation errors";
				_mainViewModel.SelectedProblem = this;
				_compiled = false;
			}
		}

		private void UpdateTests()
		{
			Tests.Clear();

			var tests = _testParser.Parse(_testsPath).ToArray();
			for (var i = 0; i < tests.Length; i++)
				Tests.Add(new TestViewModel("Test " + (i + 1), tests[i], this));

			RunAllTests();
		}

		private void RunAllTests()
		{
			if (!_compiled) return;
			foreach (var test in Tests)
				test.Run();
            UpdateStatus();
		}

        public void UpdateStatus()
        {
            if (!Tests.Any())
            {
                State.SetInconclusive();
                StatusText = "0 tests";
            }
            else
            {
                var errors = Tests.Count(t => t.State.StateType == StateType.Error);
                var successfulTests = Tests.Count - errors;
                if (errors > 0) State.SetError();
                else State.SetSuccess();
                StatusText = string.Format("{0}/{1} OK", successfulTests, Tests.Count);
            }
        }
    }
}