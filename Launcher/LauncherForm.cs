﻿/******************************************************************************
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
using Launcher.Properties;
using System.Reflection;

namespace Launcher
{
    public partial class LauncherForm : MetroFramework.Forms.MetroForm
    {
        private WebClient wc = new WebClient();
        // netversion number is allocated in Version.txt file, each element in their row
        // first element - version number like 0.0.0.1
        // second element - changelog
        private string[] net_version_no = new string[2];
        // reads Version.txt file next to excecutable
        private string current_version_no;
        // where update file will be downloaded
        private string temp_zip = "tmp/Update.zip";
        private string name_of_program = "CryptoWatcher.exe";
        // path to Version.txt file on internet
        private string github_version_file = "https://github.com/Stock84-dev/Crypto-Watcher/raw/master/Version.txt";
        // path to update file on internet
        private string github_update_zip = "https://github.com/Stock84-dev/Crypto-Watcher/raw/master/Update.zip";

        public LauncherForm()
        {
            InitializeComponent();
            // shared settings contains autoUpdate option that main app writes to it
            if (!File.Exists("SharedSettings.txt"))
                return;
            StreamReader sr = new StreamReader("SharedSettings.txt");
            Settings.Default.autoUpdate = (bool)new JavaScriptSerializer().Deserialize(sr.ReadLine(), typeof(bool));
            sr.Close();
        }
        // saving paths to file so it can be changed in the future
        private void LoadFilePaths()
        {
            if (!File.Exists("Server.txt"))
                return;
            StreamReader sr = new StreamReader("Server.txt");
            github_version_file = sr.ReadLine();
            github_update_zip = sr.ReadLine();
            sr.Close();
        }

        private void LauncherForm_Load(object sender, EventArgs e)
        {
            // after app instalation there is no main app so we look for it and download it
            if (!File.Exists(name_of_program))
            {
                LookForUpdates();
                Show();
                UpdateApp();
            }
            // looking if Version.txt file on net contains different version number than in app folder
            else if (Settings.Default.autoUpdate && LookForUpdates())
            {
                txtBox.Text = net_version_no[1];
                lblCurrentVersion.Text = "Current version: " + current_version_no;
                lblNewVersion.Text = "New version: " + net_version_no[0];
                Show();
            }
            // no updates found so we launch app
            else
                StartApp();
        }

        private bool LookForUpdates()
        {
            bool new_update = false;

            LoadFilePaths();

            try
            {
                wc.DownloadFile(new Uri(github_version_file), "NetVersion.txt");
                // reading version numbers from each files
                net_version_no = GetVersionNumber("NetVersion.txt");
                current_version_no = GetVersionNumber("Version.txt")[0];
                File.Delete("NetVersion.txt");
                if (current_version_no != net_version_no[0])
                    new_update = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new_update;
        }

        private string[] GetVersionNumber(string path)
        {
            string[] version_number = new string[2];
            // on first instalation there is no Version.txt
            if(!File.Exists(path))
            {
                version_number[0] = "";
                version_number[1] = "";
                return version_number;
            }
            // Read the file line by line
            StreamReader file = new StreamReader(path);
            string line;
            int counter = 0;
            while ((line = file.ReadLine()) != null)
            {
                version_number[counter] = line;
                counter++;
            }
            file.Close();


            return version_number;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            UpdateApp();
        }

        private void UpdateApp()
        {
            // hiding all elements in form except main label
            btnYes.Visible = false;
            btnNo.Visible = false;
            txtBox.Visible = false;
            lbl1.Visible = false;
            lbl.AutoSize = false;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Dock = DockStyle.Fill;

            lbl.Text = "Downloading files...";
            Application.DoEvents();
            Directory.CreateDirectory("tmp");
            try
            {
                wc.DownloadFile(new Uri(github_update_zip), temp_zip);
            }
            catch
            {
                MessageBox.Show("Files not found on server.");
                StartApp();
                // program exit
            }

            lbl.Text = "Installing updates...";
            Application.DoEvents();
            SaveVersion();
            ZipFile.ExtractToDirectory(temp_zip, AppDomain.CurrentDomain.BaseDirectory + "/tmp");
            
            if (!File.Exists("tmp/Instructions.txt"))
            {
                MessageBox.Show("Instruction file not found in downloaded files. Canceling installation.");
                Directory.Delete("tmp", true);
                StartApp();
                // program exit
            }

            if (!Instruction.Load("tmp/Instructions.txt"))
            {
                Directory.Delete("tmp", true);
                StartApp();
                // program exit
            }
            Instruction.DoWork();
            Directory.Delete("tmp", true);

            StartApp();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            StartApp();
        }
        
        private void StartApp()
        {
            if (!File.Exists(name_of_program))
            {
                MessageBox.Show("Main program file doesn't exist, try reinstalling or updating app.");
                Application.Exit();
            }
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C choice /C Y /N /D Y /T 0 & start " + name_of_program;
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
            Application.Exit();
        }

        private void SaveVersion()
        {
            // creating filestream that can write a file
            FileStream fs = new FileStream("Version.txt", FileMode.Create, FileAccess.Write);
            // if we don't have permission to write we exit function
            if (!fs.CanWrite)
                return;
            
            byte[] buffer = Encoding.ASCII.GetBytes(net_version_no[0]);
            // writing whole buffer array
            fs.Write(buffer, 0, buffer.Length);
            // closing filestream
            fs.Flush();
            fs.Close();
        }
    }

    public enum InstructionType { copy, delete, create, move, extract, createDir }
    // Instruction.txt contains serialized list of instructions
    // this class is used for creating, deleting, moving, copying and exrtacting files
    public class Instruction
    {
        public static List<Instruction> instructions = new List<Instruction>();

        public InstructionType iType;
        public string src;
        public string dest;

        public Instruction() { }

        Instruction(InstructionType instructionType, string source, string destination = "")
        {
            iType = instructionType;
            src = source;
            dest = destination;
        }

        public static bool Load(string file_path)
        {
            bool ret = true;
            FileStream fs = null;
            try
            {
                // creating reading filestream
                fs = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Read);
                // if we don't have permission to read we exit
                if (!fs.CanRead)
                    ret = false;

                byte[] buffer = new byte[4194304];// 4MB of space
                int bytesRead;
                bytesRead = fs.Read(buffer, 0, buffer.Length);
                // creating list of instruction by decoding bytes to string and then deserializing JSON format to object
                instructions = (List<Instruction>)new JavaScriptSerializer().Deserialize(Encoding.ASCII.GetString(buffer, 0, bytesRead), typeof(List<Instruction>));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ret = false;
            }
            finally
            {
                if (fs != null)
                {
                    // closing filestream
                    fs.Flush();
                    fs.Close();
                }
            }
            return ret;
        }

        public static void DoWork()
        {
            foreach (var instruction in instructions)
            {
                switch (instruction.iType)
                {
                    case InstructionType.copy:
                        File.Copy(instruction.src, instruction.dest == "" ? AppDomain.CurrentDomain.BaseDirectory : instruction.dest);
                        break;
                    case InstructionType.delete:
                        File.Delete(instruction.src);
                        break;
                    case InstructionType.create:
                        File.Create(instruction.src);
                        break;
                    case InstructionType.move:
                        File.Move(instruction.src, AppDomain.CurrentDomain.BaseDirectory + instruction.dest);
                        break;
                    case InstructionType.extract:
                        ZipFile.ExtractToDirectory(instruction.src, instruction.dest == "" ? AppDomain.CurrentDomain.BaseDirectory : instruction.dest);
                        break;
                    case InstructionType.createDir:
                        Directory.CreateDirectory(instruction.src);
                        break;
                }
            }
        }

        // this is used to create instruction file
        public static void SaveInstructions()
        {
            instructions.Add(new Instruction(InstructionType.delete, "CryptoWatcher.exe"));
            instructions.Add(new Instruction(InstructionType.move, "tmp/CryptoWatcher.exe", "CryptoWatcher.exe"));
            Save();
        }

        private static void Save()
        {
            // creating filestream that can write a file
            FileStream fs = new FileStream("Save/Instructions.txt", FileMode.Create, FileAccess.Write);
            // if we don't have permission to write we exit function
            if (!fs.CanWrite)
                return;
            // creating JSON format from alerts list
            string str_alerts = new JavaScriptSerializer().Serialize(instructions);
            // converting string to byte array
            byte[] buffer = Encoding.ASCII.GetBytes(str_alerts);
            // writing whole buffer array
            fs.Write(buffer, 0, buffer.Length);
            // closing filestream
            fs.Flush();
            fs.Close();
        }
    }
}
