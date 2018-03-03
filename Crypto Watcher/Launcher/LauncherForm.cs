/******************************************************************************
 * CRYPTO WATCHER - cryptocurrency alert system that notifies you when certain 
 * cryptocurrency fulfills your condition.
 * Copyright (c) 2017 Stock84-dev
 * https://github.com/Stock84-dev/Crypto-Watcher
 *
 * This file is part of CRYPTO WATCHER.
 *
 * CRYPTO WATCHER is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * CRYPTO WATCHER is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with CRYPTO WATCHER.  If not, see <http://www.gnu.org/licenses/>.
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.IO.Compression;
using System.Web.Script.Serialization;
using System.Reflection;
using CryptoWatcher.Properties;
using Newtonsoft.Json;

namespace CryptoWatcher.Launcher
{
	public partial class LauncherForm : MetroFramework.Forms.MetroForm
	{
		private VersionData versionData;
		private WebClient wc = new WebClient();
		private const string baseUpdatePath = "https://raw.githubusercontent.com/Stock84-dev/Crypto-Watcher/master/Update/";
		private const string netVersionPath = baseUpdatePath + "CurrentVersion.txt";
		private const string versionsPath = baseUpdatePath + "Versions/";
		private const string tmpDownloadPath = "download tmp/";

		public LauncherForm()
		{
			InitializeComponent();

			if (Directory.Exists(tmpDownloadPath))
				Directory.Delete(tmpDownloadPath);

			if (Settings.Default.LookForUpdates)
				LookForUpdates();
		}

		private void btnYes_Click(object sender, EventArgs e)
		{
			DoUpdate();
		}

		private void btnNo_Click(object sender, EventArgs e)
		{
			StartApp();
		}

		private void StartApp()
		{
			Hide();
			MainForm mainForm = new MainForm();
			mainForm.ShowDialog();
			Close();
		}

		private void LookForUpdates()
		{
			versionData = GetVersion();

			if (Settings.Default.CurrentVersionId < versionData.Id)
			{
				txtBox.Text = versionData.Changelog;
				lblCurrentVersion.Text = "Current version: " + Settings.Default.CurrentVersion;
				lblNewVersion.Text = "New version: " + versionData.Name;
				Show();
			}
		}

		private VersionData GetVersion()
		{
			string netVersion = JsonConvert.DeserializeObject<string>(wc.DownloadString(netVersionPath));
			return JsonConvert.DeserializeObject<VersionData>(wc.DownloadString(versionsPath + netVersion + "/VersionData.txt"));
		}

		private void DoUpdate()
		{
			lbl.Text = "Updating Crypto Watcher";
			lblCurrentVersion.Hide();
			lblNewVersion.Hide();
			lbl1.Text = "Details:";
			btnYes.Hide();
			btnNo.Hide();
			txtBox.Text = "";

			DeleteUnnecessaryFiles();
			DownloadNeccessaryFiles();
		}

		private void DownloadNeccessaryFiles()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

			foreach (var reqdir in versionData.RequiredDirs)
			{
				bool found = false;
				foreach (var dir in directoryInfo.GetDirectories())
				{
					if (reqdir.Name == dir.Name)
					{
						found = true;
						break;
					}
				}
				if (!found)
				{
					// create dir
					txtBox.AppendText(Environment.NewLine);
					txtBox.AppendText("Creating: " + reqdir.Name);
					Directory.CreateDirectory(reqdir.RelativePath);
				}
			}

			foreach (var reqfile in versionData.RequiredFiles)
			{
				bool found = false;
				foreach (var file in directoryInfo.GetFiles())
				{
					if (reqfile.Name == file.Name)
					{
						found = true;
						break;
					}
				}
				if (!found)
				{
					// Download file
					txtBox.AppendText(Environment.NewLine);
					txtBox.AppendText("Downloading: " + Path.GetDirectoryName(Application.ExecutablePath) + "/" + reqfile.RelativePath);
					wc.DownloadFile(baseUpdatePath + "/Files/" + reqfile.Name, reqfile.RelativePath);
				}
			}

			wc.DownloadFile(versionsPath + versionData.Name + "/" + "CryptoWatcher.exe", tmpDownloadPath + "CryptoWatcher.exe");
			Settings.Default.CurrentVersion = versionData.Name;
			Settings.Default.CurrentVersionId = versionData.Id;

			string argument = "/C Choice /C Y /N /D Y /T 4 & Del /F /Q \"{0}\" & Choice /C Y /N /D Y /T 2 & Move /Y \"{1}\" \"{2}\" & Start \"\" /D \"{3}\" \"{4}\"";// {5}";
			string currentPath = Application.ExecutablePath;
			string tempFilePath = Path.GetDirectoryName(Application.ExecutablePath) + "/download tmp/CryptoWatcher.exe";
			string newPath = Application.ExecutablePath;

			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = string.Format(argument, currentPath, tempFilePath, newPath, Path.GetDirectoryName(newPath), Path.GetFileName(newPath)/*, launchArgs*/);
			info.WindowStyle = ProcessWindowStyle.Hidden;
			info.CreateNoWindow = true;
			info.FileName = "cmd.exe";
			Process.Start(info);
		}

		private void DeleteUnnecessaryFiles()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

			foreach (var dir in directoryInfo.GetDirectories())
			{
				if (!MyFile.Contains(dir.Name, versionData.RequiredDirs) && !MyFile.Contains(dir.Name, versionData.UserGeneratedDirs))
				{
					txtBox.AppendText(Environment.NewLine);
					txtBox.AppendText("Deleting: " + dir.FullName);
					Directory.Delete(dir.FullName, true);
				}
			}

			foreach (var file in directoryInfo.GetFiles())
			{
				if (!MyFile.Contains(file.Name, versionData.RequiredFiles) && !MyFile.Contains(file.Name, versionData.UserGeneratedFiles) && file.Name != "CryptoWatcher.exe")
				{
					txtBox.AppendText(Environment.NewLine);
					txtBox.AppendText("Deleting: " + file.FullName);
					File.Delete(file.FullName);
				}
			}
		}
	}
}
