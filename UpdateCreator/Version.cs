using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Update algorithm
 * 
 * in GitHubURL/Update
 * contains txt file with JSON string
 * string contains version name and changelog
 * 
 * in GithubURL/Update/Files
 * contains files that are required to download
 * 
 * in GitHubURL/Update/Versions/VersionNumber
 * contains exe file and txt file in which is a json string
 * JSON contains version name, version ID, required filenames, user/program generated files
 * 
 * when program starts it downloads txt file in Update dir
 * looks for difference in versions
 * notifies user + displays changelog
 * downloads files from GitHubURL/Update/Versions/VersionNumber
 * deletes all files that arent in either of lists in txt file
 * downloads files from in GithubURL/Update/Files if they are missing
 * closes app replaces exe starts app
 */

namespace UpdateCreator
{
	class VersionData
	{
		public string Name { get; set; }
		public List<MyFile> RequiredFiles { get; set; }
		/// <summary>
		/// Files that can be in directory but not required.
		/// </summary>
		public List<MyFile> UserGeneratedFiles { get; set; }
		public List<MyFile> RequiredDirs { get; set; }
		public List<MyFile> UserGeneratedDirs { get; set; }
		public int Id { get; set; } = 0;
		public string Changelog { get; set; }

		public void CreateVersion()
		{
			PopulateData();

			AddRequiredFiles();
		}

		private void AddRequiredFiles()
		{
			string downloadPath = Paths.ProjectPath + @"\Update\Files";
			DirectoryInfo downloadLocation = new DirectoryInfo(downloadPath);

			// copies required files that doesn't exist already in /Update/Files
			foreach (var file in RequiredFiles)
			{
				if (!File.Exists(downloadPath + @"\" + file.Name))
					File.Copy(Paths.ProgramDirPath + @"\" + file.RelativePath, downloadPath + @"\" + file.Name);
			}

			// copies required directories that doesn't exist already in \Update\Files
			foreach (var dir in RequiredDirs)
			{
				if (!Directory.Exists(downloadPath + @"\" + dir.Name))
				{
					CopyDir(Paths.ProgramDirPath + @"\" + dir.RelativePath, downloadPath);
				}
			}
			// gets previous version id from \Update\VersionInfo.txt
			VersionInfo versionInfo = JsonConvert.DeserializeObject<VersionInfo>(File.ReadAllText(Paths.VersionInfoPath));
			Id = versionInfo.Id + 1;

			// need to write thes every time you create version
			Changelog = GetChangelog();
			Name = GetVersionName();

			// creates new dir in \Update\Versions\{versionName}
			string versionDirPath = Paths.VersionsPath + @"\" + Name;
			Directory.CreateDirectory(versionDirPath);

			string versionInstructionsPath = versionDirPath + @"\" + Name + ".txt";
			// creates instructions for version in \Update\Versions\{versionName}\{versionName}.txt
			File.WriteAllText(versionInstructionsPath, JsonConvert.SerializeObject(this));
			// copies program to \Update\Versions\{versionName}\
			File.Copy(Paths.ProgramDirPath + @"\CryptoWatcher.exe", versionDirPath + @"\CryptoWatcher.exe");
			// creates version info in Update\VersionInfo.txt
			File.WriteAllText(Paths.VersionInfoPath, JsonConvert.SerializeObject(new VersionInfo(Name, Changelog, Id)));
		}

		private string GetVersionName()
		{
			// Write version name here.
			return "1.0.0-alpha.2";
		}

		private string GetChangelog()
		{
			// Write changelog here.
			return "Testing new launcher.";
		}

		private void PopulateData()
		{
			DirectoryInfo programDir = new DirectoryInfo(Paths.ProgramDirPath);

			RequiredFiles = new List<MyFile>();
			RequiredDirs = new List<MyFile>();

			PopulateUserGeneratedFiles();
			PopulateUserGeneratedDirs();

			foreach (var file in programDir.GetFiles())
			{
				if (!MyFile.Contains(file.Name, UserGeneratedFiles))
					RequiredFiles.Add(new MyFile(file.Name, GetRelativePath(file.FullName, programDir.FullName)));
			}

			foreach (var dir in programDir.GetDirectories())
			{
				if (!MyFile.Contains(dir.Name, UserGeneratedDirs))
					RequiredDirs.Add(new MyFile(dir.Name, GetRelativePath(dir.FullName, programDir.FullName)));
			}
		}

		private void PopulateUserGeneratedFiles()
		{
			UserGeneratedFiles = new List<MyFile>() { new MyFile("CryptoWatcher.exe", "") };
		}

		private void PopulateUserGeneratedDirs()
		{
			UserGeneratedDirs = new List<MyFile>() { new MyFile("Saves", "") };
		}

		private string GetRelativePath(string filespec, string folder)
		{
			Uri pathUri = new Uri(filespec);
			// Folders must end in a slash
			if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				folder += Path.DirectorySeparatorChar;
			}
			Uri folderUri = new Uri(folder);
			return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('\'', Path.DirectorySeparatorChar));
		}

		/// <summary>
		/// Copies directory and all its files and folders.
		/// </summary>
		private static void CopyDir(string sourcePath, string destPath)
		{
			DirectoryInfo di = new DirectoryInfo(sourcePath);
			string destDir = destPath + @"\" + di.Name;
			Directory.CreateDirectory(destDir);

			foreach (var file in di.GetFiles())
			{
				File.Copy(file.FullName, destDir + @"\" + file.Name);
			}

			foreach (var dir in di.GetDirectories())
			{
				CopyDir(dir.FullName, destDir);
			}
		}
	}

	public class VersionInfo
	{
		public VersionInfo(string name, string changelog, int id)
		{
			Name = name;
			Changelog = changelog;
			Id = id;
		}

		public string Name { get; set; }
		public string Changelog { get; set; }
		public int Id { get; set; }
	}

	/// <summary>
	/// These paths only work on my computer.
	/// </summary>
	public class Paths
	{
		public static string ProjectPath { get; } = @"D:\Documents\Visual studio 2017\Projects\Crypto Watcher";
		public static string VersionInfoPath { get; } = ProjectPath + @"\Update\VersionInfo.txt";
		public static string VersionsPath { get; } = ProjectPath + @"\Update\Versions\";
		public static string ProgramDirPath { get; } = ProjectPath + @"\Crypto Watcher\bin\Release\";
	}

	public class MyFile
	{
		public MyFile(string name, string relativePath)
		{
			Name = name;
			RelativePath = relativePath;
		}

		public string Name { get; set; }
		public string RelativePath { get; set; }

		public static bool Contains(string name, List<MyFile> files)
		{
			foreach (var file in files)
			{
				if (file.Name == name)
					return true;
			}
			return false;
		}
	}
}
