using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Tester.Helpers;

namespace Tester.Services
{
	public class ProcessLauncher
	{
		public Future<ProcessExecuteResult> Launch(string fileName, string input, params string[] parameters)
		{
			var action = new Func<ProcessExecuteResult>(() => LaunchInternal(fileName, input, parameters));
			return Future.FromDelegate(action);
		}

		private ProcessExecuteResult LaunchInternal(string fileName, string input, params string[] parameters)
		{
			var args = parameters == null || !parameters.Any() ? ""
				           : string.Format(parameters.First(), parameters.Skip(1).ToArray());
			var processStartInfo = new ProcessStartInfo(fileName, args)
				{
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
				};

			using (new ChangeErrorMode(ErrorModes.FailCriticalErrors | ErrorModes.NoGpFaultErrorBox))
			{
                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    StringBuilder output = new StringBuilder();
                    StringBuilder error = new StringBuilder();

                    bool finished = false;

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (finished) return;
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                output.AppendLine(e.Data);
                            }
                        };
                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (finished) return;
                            if (e.Data == null)
                            {
                                if (errorWaitHandle
                                    != null) 
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                error.AppendLine(e.Data);
                            }
                        };

                        process.Start();

                        if (input != null)
                        {
                            process.StandardInput.Write(input);
                            process.StandardInput.Close();
                        }

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        var timeout = (int) TimeSpan.FromSeconds(10).TotalMilliseconds;
                        if (process.WaitForExit(timeout) &&
                            outputWaitHandle.WaitOne(timeout) &&
                            errorWaitHandle.WaitOne(timeout))
                        {
                            return new ProcessExecuteResult(output.ToString(), error.ToString(), process.ExitCode, process.ExitTime - process.StartTime);
                            // Process completed. Check process.ExitCode here.
                        }
                        else
                        {
                            finished = true;
                            if (!process.HasExited)
                                process.Kill();
                            return new ProcessExecuteResult("Time Out", "Time Out", -1, DateTime.Now - process.StartTime);
                            // Timed out.
                        }
                    }
                }
			}
		}
	}
}