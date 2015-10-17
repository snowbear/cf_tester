namespace Core
{
	public class TestData
	{
		public string Input { get; private set; }

		public string ExpectedOutput { get; private set; }

		public TestData(string input, string expectedOutput)
		{
			Input = input;
			ExpectedOutput = expectedOutput;
		}
	}
}