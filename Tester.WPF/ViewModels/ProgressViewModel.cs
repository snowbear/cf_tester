namespace Tester.ViewModels
{
	public class ProgressViewModel
	{
		public static readonly ProgressViewModel Instance = new ProgressViewModel();

		public ViewModelProperty<string> Text { get; private set; }

		private ProgressViewModel()
		{
			Text = new ViewModelProperty<string>();
		}
	}
}