using System.Windows;
using System.Windows.Forms;
using Tester.Helpers;
using Tester.ViewModels;

namespace Tester
{
	public partial class MainWindow
	{
		private MainViewModel _viewModel;

		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			DataContext = _viewModel = new MainViewModel();
			_viewModel.Init();
		}

		private void DockToBottom_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.LeftProperty.Value = 0;
			_viewModel.WidthProperty.Value = (int) SystemParameters.WorkArea.Width;
			_viewModel.HeightProperty.Value = 200;
			_viewModel.TopProperty.Value = (int) (SystemParameters.WorkArea.Height - Height);
		}

		private void OpenFolder_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new FolderBrowserDialog
				{
					SelectedPath = IoHelper.GetDirectory(_viewModel.PathProperty.Value),
				};
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				_viewModel.PathProperty.Value = dialog.SelectedPath;
			}
		}

		private void OpenFile_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			if (!string.IsNullOrEmpty(_viewModel.PathProperty.Value))
			{
				dialog.InitialDirectory = IoHelper.GetDirectory(_viewModel.PathProperty.Value);
			}
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				_viewModel.PathProperty.Value = dialog.FileName;
			}
		}

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.OpenSettings();
        }
	}
}