using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Utilities
{
	class Logger
	{
		public static async void LogLine(string path, string data)
		{
			StreamWriter sw = new StreamWriter(path, true);
			await sw.WriteLineAsync(data);
			sw.Close();
		}

		public static async void Log(string path, string data, bool append)
		{
			StreamWriter sw = new StreamWriter(path, append);
			await sw.WriteAsync(data);
			sw.Close();
		}
	}
}
