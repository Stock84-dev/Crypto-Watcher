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
using Tulpep.NotificationWindow;
using CryptoWatcher;

namespace Alerts
{
	public class Notification
    {
        SoundPlayer soundPlayer = new SoundPlayer();
		PopupNotifier popup = new PopupNotifier();
        string soundsPath = "Sounds\\AlarmSound.wav";

		SoundType sound;
		bool showWindowsMsg;
		bool showWindow;
		public int Interval { get; set; }
		
		IFormReference mainForm = (IFormReference)Application.OpenForms["MainForm"];

		public Notification(SoundType soundType, bool showWindowsMsg, bool showWindow, int interval = -1)
        {
			Initialize();

			sound = soundType;
			this.showWindowsMsg = showWindowsMsg;
			this.showWindow = showWindow;
			Interval = interval;
		}

		public Notification(ref string data)
		{
			Initialize();

			int i = data.IndexOf(';');
			sound = (SoundType)int.Parse(data.Substring(0, i));
			data = data.Substring(i + 1);
			showWindowsMsg = Convert.ToBoolean(int.Parse(data.Substring(0, (i = data.IndexOf(';')))));
			data = data.Substring(i + 1);
			showWindow = Convert.ToBoolean(int.Parse(data.Substring(0, (i = data.IndexOf(';')))));
			data = data.Substring(i + 1);
			Interval = int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
		}

		public void Notify(int alertIndex)
        {
			if (Settings.Default.playSound)
			{
				switch (sound)
				{
					case SoundType.looping: soundPlayer.PlayLooping();
						break;
					case SoundType.sound:soundPlayer.Play();
						break;
				}
			}

			if (showWindowsMsg)
			{
				popup.ContentText = Alert.AlertList[alertIndex].Message;
				popup.Popup();
			}
			if (showWindow)
			{
				mainForm.ShowWindow();
			}
			mainForm.AddMessage(alertIndex);
		}

		public void StopNotifying(int alertIndex, bool tryRemovingAlert)
		{
			soundPlayer.Stop();
			popup.Hide();
			if(tryRemovingAlert)
				mainForm.TryRemoveAlert(alertIndex);
		}

		public string ToLine()
		{
			return ((int)sound).ToString() + ";" +
				Convert.ToInt32(showWindowsMsg).ToString() + ";" +
				Convert.ToInt32(showWindow).ToString() + ";" +
				Interval.ToString() + ";";
		}

		public enum SoundType { looping, sound, noSound};

		private void Initialize()
		{
			soundPlayer = new SoundPlayer(soundsPath);
			popup.Image = Resources.Info;
			popup.TitleText = "Alert triggered!";
			popup.HeaderColor = Color.FromArgb(255, 0, 174, 219);
			popup.TitleColor = Color.FromArgb(255, 0, 174, 219);
			popup.ContentHoverColor = Color.FromArgb(255, 0, 174, 219);
			popup.ShowGrip = false;
			popup.HeaderHeight = 6;
			popup.Click += mainForm.Popup_Click;
		}
	}
}
