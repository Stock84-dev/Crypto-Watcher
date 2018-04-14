using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Utilities
{
	public class Utility
	{
		/// <summary>
		/// Gets substring between 2 same strings in string at specific occurrence.
		/// </summary>
		/// <param name="str">Source string.</param>
		/// <param name="sample">Character that is repeating.</param>
		/// <param name="nOccurances">Get substring after n occurrence.</param>
		/// /// <param name="between">Return string between occurrences or after occurrence.</param>
		/// <returns></returns>
		public static string GetSubstring(string str, char sample, int nOccurances, bool between = true)
		{
			if (nOccurances == 0 && between)
				return str.Substring(0, str.IndexOf(sample));
			else if (nOccurances == 0 && !between)
				return str.Substring(0);
			
			int index = -1;
			for (int i = 0, j = 0; i < str.Length; i++)
			{
				if (str[i] == sample)
					j++;
				if (j == nOccurances)
				{
					index = i + 1;
					break;
				}
			}
			if (index == -1)
				return null;
				//throw new ArgumentException("Could not find sample in string at specific occurance.");
			str = str.Substring(index);
			if (between)
			{
				int sampleId = str.IndexOf(sample);
				if (sampleId == -1)
					return str;
				else
					return str.Substring(0, sampleId);
			}
			else return str;
		}
	}
}
