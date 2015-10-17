using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Tester.Helpers;
using Tester.Models;
using Tester.Services;
using Tester.Services.Compilers;

namespace Tester.ViewModels
{
	public class MainViewModel : ModelBase
	{
		private ProblemViewModel _selectedProblem;
        private CompilerSelector _compilerSelector = new CompilerSelector();

		private readonly ViewModelProperty<string> _pathProperty = new ViewModelProperty<string>();
		private readonly ViewModelProperty<int> _leftProperty = new ViewModelProperty<int>();
		private readonly ViewModelProperty<int> _topProperty = new ViewModelProperty<int>();
		private readonly ViewModelProperty<int> _widthProperty = new ViewModelProperty<int>();
		private readonly ViewModelProperty<int> _heightProperty = new ViewModelProperty<int>();
		private readonly ViewModelProperty<bool> _alwaysOnTopProperty = new ViewModelProperty<bool>();

		public ViewModelProperty<string> PathProperty
		{
			get { return _pathProperty; }
		}

		public ViewModelProperty<int> LeftProperty
		{
			get { return _leftProperty; }
		}

		public ViewModelProperty<int> TopProperty
		{
			get { return _topProperty; }
		}

		public ViewModelProperty<int> WidthProperty
		{
			get { return _widthProperty; }
		}

		public ViewModelProperty<int> HeightProperty
		{
			get { return _heightProperty; }
		}

		public ViewModelProperty<bool> AlwaysOnTopProperty
		{
			get { return _alwaysOnTopProperty; }
		}

		public ObservableCollection<ProblemViewModel> Problems { get; private set; }

		public ProblemViewModel SelectedProblem
		{
			get { return _selectedProblem; }
			set
			{
				if (value == _selectedProblem) return;
				_selectedProblem = value;
				OnPropertyChanged("SelectedProblem");
			}
		}

		public ProgressViewModel Progress { get; private set; }

		public MainViewModel()
		{
			Problems = new ObservableCollection<ProblemViewModel>();
		}

		public void Init()
		{
			_widthProperty.Value = 500;
			_heightProperty.Value = 300;

			PathProperty.OnChanged(UpdateTests);

			var settingsStorage = SettingsStorage.Instance;

			_pathProperty.BindTo(settingsStorage.PathProperty);
			_leftProperty.BindTo(settingsStorage.LeftProperty);
			_topProperty.BindTo(settingsStorage.TopProperty);
			_widthProperty.BindTo(settingsStorage.WidthProperty);
			_heightProperty.BindTo(settingsStorage.HeightProperty);
			_alwaysOnTopProperty.BindTo(settingsStorage.IsTopmostProperty);

			Progress = ProgressViewModel.Instance;
		}

		private void UpdateTests()
		{
			if (string.IsNullOrEmpty(PathProperty.Value)) return;
			FileChangesTracker.Instance.Track(PathProperty.Value);
			Problems.Clear();
			var files =
				IoHelper.IsFile(PathProperty.Value) ? new[] {PathProperty.Value}
					: Directory.GetFiles(PathProperty.Value).Where(f => !f.EndsWith(".txt"));
			foreach (var file in files)
			{
                if (!_compilerSelector.ExtensionSupported(Path.GetExtension(file))) continue;
				var problem = new ProblemViewModel(this, file)
					{
						Name = Path.GetFileNameWithoutExtension(file),
					};
				Problems.Add(problem);
			}
		}

        public void OpenSettings()
        {
            SettingsStorage.Instance.OpenSettingsInExternalEditor();
        }
    }
}