namespace Tester.ViewModels
{
	public class ProblemDetailsViewModel
	{
		public ProblemViewModel ProblemViewModel { get; private set; }

		public ProblemDetailsViewModel(ProblemViewModel problemViewModel)
		{
			ProblemViewModel = problemViewModel;
		}
	}
}