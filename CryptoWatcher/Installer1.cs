using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


// tutorial: http://steptodotnet.blogspot.hr/2012/10/visual-studio-package-and-development.html
namespace CryptoWatcher
{
	[RunInstaller(true)]
	public partial class Installer1 : System.Configuration.Install.Installer
	{
		public Installer1()
		{
			InitializeComponent();
		}

		// have to delete files that application generates on runtime
		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);

			DirectoryInfo directoryInfo = Directory.GetParent(Context.Parameters["assemblypath"]);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			FileInfo[] files = directoryInfo.GetFiles();

			foreach (var file in files)
			{
				// don't have access to delete those files
				if (!(file.FullName.Contains(".exe") || file.FullName.Contains(".dll")))
					File.Delete(file.FullName);
			}
			foreach (var dir in directories)
			{
				Directory.Delete(dir.FullName, true);
			}
		}
	}
}
