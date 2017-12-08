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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Management;

namespace Launcher
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
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
