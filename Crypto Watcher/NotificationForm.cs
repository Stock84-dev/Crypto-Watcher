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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoWatcher.Properties;
using System.Media;

namespace Crypto_watcher
{
    public class Notification
    {
        private SoundPlayer soundPlayer;
        string sounds_path = "Sounds\\AlarmSound.wav";

        public Notification()
        {
            soundPlayer = new SoundPlayer(sounds_path);
        }

        public void Notify(string message)
        {
            if (Settings.Default.playSound && System.IO.File.Exists(sounds_path))
                soundPlayer.PlayLooping();
            MessageBox.Show(message);
            soundPlayer.Stop();
        }
        
    }
}
