using CryptoWatcher.Utilities;
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

	// TODO: create what is stated in first point
	// TODO: get id from previous version and increment it, create third point
namespace CryptoWatcher.Launcher
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
