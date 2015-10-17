using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Tester.Helpers;

namespace Tester.Services
{
	public class FileChangesTracker
	{
		public static readonly FileChangesTracker Instance = new FileChangesTracker();
		private readonly FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher();
		private readonly IDictionary<string, DateTime> _lastUpdates = new Dictionary<string, DateTime>();
		private readonly Dispatcher _dispatcher;

		private const int MillisecondsToIgnore = 500;

		public event FileSystemEventHandler FileChanged;

		private FileChangesTracker()
		{
			_fileSystemWatcher.Changed += FileSystemWatcher_OnFileChanged;
			_fileSystemWatcher.BeginInit();
			_fileSystemWatcher.EndInit();
			_dispatcher = Application.Current.Dispatcher;
		}

		public void Track(string path)
		{
			if (IoHelper.IsFile(path)) path = Path.GetDirectoryName(path);
		
			_fileSystemWatcher.Path = path;
			_fileSystemWatcher.EnableRaisingEvents = true;
		}

		private void FileSystemWatcher_OnFileChanged(object sender, FileSystemEventArgs e)
		{
			if (!_dispatcher.CheckAccess())
			{
				_dispatcher.BeginInvoke(new FileSystemEventHandler(FileSystemWatcher_OnFileChanged), sender, e);
				return;
			}
			DateTime lastUpdate;
			if (_lastUpdates.TryGetValue(e.FullPath, out lastUpdate) &&
			    (DateTime.Now - lastUpdate).TotalMilliseconds < MillisecondsToIgnore)
			{
				return;
			}
			_lastUpdates[e.FullPath] = DateTime.Now;
			if (FileChanged != null)
				FileChanged(this, e);
		}
	}
}