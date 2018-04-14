using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoWatcher.Utilities
{
	public class IOManager
	{
		private string _filePath;

		public IOManager(string filePath)
		{
			_filePath = filePath;
		}
		/// <summary>
		/// Performs action for each line that is in file.
		/// </summary>
		public async Task<bool> LoadByLineAsync(Action<string> action)
		{
			if (!File.Exists(_filePath))
				return false;

			StreamReader sr = new StreamReader(_filePath);
			string line = string.Empty;
			while ((line = await sr.ReadLineAsync()) != null)
				action(line);

			sr.Close();
			return true;
		}

		public async void SaveByLineAsync(IEnumerable<string> lines)
		{
			StreamWriter sw = new StreamWriter(_filePath);
			foreach (var line in lines)
				await sw.WriteLineAsync(line);
			sw.Close();
		}
	}
}
