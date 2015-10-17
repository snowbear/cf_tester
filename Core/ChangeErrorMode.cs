using System;
using System.Runtime.InteropServices;

namespace Core
{
	public struct ChangeErrorMode : IDisposable
	{
		private readonly int _oldMode;

        public ChangeErrorMode(ErrorModes mode)
		{
			_oldMode = SetErrorMode((int) mode);
		}

		void IDisposable.Dispose()
		{
			SetErrorMode(_oldMode);
		}

		[DllImport("kernel32.dll")]
		private static extern int SetErrorMode(int newMode);
	}
}