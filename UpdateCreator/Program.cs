using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdateCreator
{
	class Program
	{
		static void Main(string[] args)
		{
			VersionData versionData = new VersionData();
			versionData.CreateVersion();
			Console.WriteLine("Done!");
			//Console.ReadKey();
		}
	}
}
