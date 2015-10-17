using System;

namespace Core
{
	public class ProcessExecuteResult
	{
		public string Output { get; private set; }
		public string Error { get; private set; }
		public int ExitCode { get; private set; }
		public TimeSpan ExecutionTime { get; private set; }

		public ProcessExecuteResult(string output, string error, int exitCode, TimeSpan executionTime)
		{
			Output = output;
			Error = error;
            if (error == "") Error = output;
			ExitCode = exitCode;
			ExecutionTime = executionTime;
		}
	}
}