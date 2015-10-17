using System;
using System.Collections.Generic;
using System.Linq;
using Tester.Models;
using Tester.Services;
using Tester.Services.TestParsers;

namespace Tester.ViewModels
{
	public class TestViewModel : ModelBase
	{
		private readonly TestData _testData;
		private readonly ProblemViewModel _problem;
		private readonly ProcessLauncher _processLauncher = new ProcessLauncher();
		private int? _runTime;

		private readonly ViewModelProperty<object> _details = new ViewModelProperty<object>();

		public StateViewModel State { get; private set; }

		public string Name { get; private set; }

		public int? RunTime
		{
			get { return _runTime; }
			private set
			{
				_runTime = value;
				OnPropertyChanged("RunTime");
			}
		}

		public ViewModelProperty<object> Details
		{
			get { return _details; }
		}

		public TestViewModel(string name, TestData testData, ProblemViewModel problem)
		{
			_testData = testData;
			_problem = problem;
			Name = name;
			State = new StateViewModel();
            State.PropertyChanged += State_PropertyChanged;
		}

        void State_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StateType")
                _problem.UpdateStatus();
        }

		public void Run()
		{
			ProgressViewModel.Instance.Text.Value = "Running test";
			_processLauncher.Launch(_problem.CompilerExecutablePath, _testData.Input)
				.OnComplete(result =>
					{

						ProgressViewModel.Instance.Text.Value =
							result.ExitCode == 0
								? "Process completed with no errors"
								: "Process completed with error";
						RunTime = (int)result.ExecutionTime.TotalMilliseconds;
						if (result.ExitCode == 0 && TestOutputEquals(result.Output, _testData.ExpectedOutput))
						{
							Details.Value = null;
							State.SetSuccess();
						}
						else
						{
							var resultText = result.ExitCode == 0 ? result.Output : result.Error ?? "NZEC";
							Details.Value = new TestDetailsViewModel(resultText, _testData.ExpectedOutput, _testData.Input);
							State.SetError();
						}
					});
		}
    
        private static bool TestOutputEquals(string actual, string expected)
        {
            Func<string, List<string>> split = s =>
            {
                return s.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => str.Trim())
                    .Where(str => !string.IsNullOrWhiteSpace(str))
                    .ToList();
            };
            var actualLines = split(actual);
            var expectedLines = split(expected);
            return actualLines.SequenceEqual(expectedLines);
        }
    }
}