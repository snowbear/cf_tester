using System.IO;

namespace Tester.Helpers
{
	public static class IoHelper
	{
		 public static bool IsFile(string path)
		 {
			 return Path.HasExtension(path);
		 }

		 public static string GetDirectory(string path)
		 {
			 return IsFile(path) ? Path.GetDirectoryName(path) : path;
		 }
	}
}