namespace Tester.ViewModels
{
	public class TestDetailsViewModel
	{
		public string ActualOutput { get; private set; }
		public string ExpectedOutput { get; private set; }
		public string Input { get; private set; }

		public TestDetailsViewModel(string actualResult, string expectedOutput, string input)
		{
			ActualOutput = actualResult;
			ExpectedOutput = expectedOutput;
			Input = input;
		}
	}
}