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
		private const string BASE_UPDATE_PATH = /*"https://raw.githubusercontent.com/Stock84-dev/Crypto-Watcher/1.0.0-alpha.2/Update/";*/"https://raw.githubusercontent.com/Stock84-dev/Crypto-Watcher/master/Update/";
		private const string NET_VERSION_PATH = BASE_UPDATE_PATH + "VersionInfo.txt";
		private const string VERSIONS_PATH = BASE_UPDATE_PATH + "Versions/";
		private const string TMP_DOWNLOAD_PATH = "download tmp/";
		private VersionData _versionData;
		private VersionInfo _versionInfo;
		private WebClient _webClient = new WebClient();
		
		public LauncherForm()
		{
			InitializeComponent();
			Hide();
			btnYes.Click += (s, e) => DoUpdate();
			btnNo.Click += (s, e) => StartApp();
		}

		public bool ShowMainForm { get; set; } = false;

		private void LauncherForm_Load(object sender, EventArgs e)
		{
			if (Directory.Exists(TMP_DOWNLOAD_PATH))
				Directory.Delete(TMP_DOWNLOAD_PATH);

			if (Settings.Default.LookForUpdates)
				LookForUpdates();
			StartApp();
		}

		private void StartApp()
		{
			Close();
			ShowMainForm = true;
		}

		private void LookForUpdates()
		{
			try
			{
				_versionInfo = JsonConvert.DeserializeObject<VersionInfo>(_webClient.DownloadString(NET_VERSION_PATH));
			}
			catch
			{
				return;
			}

			if (Settings.Default.CurrentVersionId < _versionInfo.Id)
			{
				txtBox.Text = _versionInfo.Changelog;
				lblCurrentVersion.Text = "Current version: " + Settings.Default.CurrentVersion;
				lblNewVersion.Text = "New version: " + _versionInfo.Name;
				Show();
			}
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

			_versionData = JsonConvert.DeserializeObject<VersionData>(_webClient.DownloadString(VERSIONS_PATH + _versionInfo.Name + "/" + _versionInfo.Name + ".txt"));

			DeleteUnnecessaryFiles();
			DownloadNeccessaryFiles();
		}

		private void DownloadNeccessaryFiles()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

			foreach (var reqdir in _versionData.RequiredDirs)
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

			foreach (var reqfile in _versionData.RequiredFiles)
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
					_webClient.DownloadFile(BASE_UPDATE_PATH + "/Files/" + reqfile.Name, reqfile.RelativePath);
				}
			}

			Directory.CreateDirectory("download tmp");
			_webClient.DownloadFile(VERSIONS_PATH + _versionData.Name + "/" + "CryptoWatcher.exe", TMP_DOWNLOAD_PATH + "CryptoWatcher.exe");
			Settings.Default.CurrentVersion = _versionData.Name;
			Settings.Default.CurrentVersionId = _versionData.Id;

			// closes app, starts new process wihich replaces old exe with new exe and then starts app
			string argument = "/C Choice /C Y /N /D Y /T 4 & Del /F /Q \"{0}\" & Choice /C Y /N /D Y /T 2 & Move /Y \"{1}\" \"{2}\" & Start \"\" /D \"{3}\" \"{4}\"";// {5}";
			string currentPath = Application.ExecutablePath;
			string tempFilePath = Path.GetDirectoryName(currentPath) + "\\download tmp\\CryptoWatcher.exe";
			string newPath = Application.ExecutablePath;

			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = string.Format(argument, currentPath, tempFilePath, newPath, Path.GetDirectoryName(newPath), Path.GetFileName(newPath));
			info.WindowStyle = ProcessWindowStyle.Hidden;
			info.CreateNoWindow = true;
			info.FileName = "cmd.exe";
			Process.Start(info);
			Application.Exit();
		}

		private void DeleteUnnecessaryFiles()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

			foreach (var dir in directoryInfo.GetDirectories())
			{
				if (!MyFile.Contains(dir.Name, _versionData.RequiredDirs) && !MyFile.Contains(dir.Name, _versionData.UserGeneratedDirs))
				{
					txtBox.AppendText(Environment.NewLine);
					txtBox.AppendText("Deleting: " + dir.FullName);
					Directory.Delete(dir.FullName, true);
				}
			}

			foreach (var file in directoryInfo.GetFiles())
			{
				if (!MyFile.Contains(file.Name, _versionData.RequiredFiles) && !MyFile.Contains(file.Name, _versionData.UserGeneratedFiles) && file.Name != "CryptoWatcher.exe")
				{
					txtBox.AppendText(Environment.NewLine);
					txtBox.AppendText("Deleting: " + file.FullName);
					File.Delete(file.FullName);
				}
			}
		}
	}
}
