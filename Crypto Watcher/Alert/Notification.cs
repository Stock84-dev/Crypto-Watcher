/******************************************************************************
 * CRYPTO WATCHER - cryptocurrency alert system that notifies you when certain 
 * cryptocurrency fulfills your condition.
 * Copyright (c) 2017-2018 Stock84-dev
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
using System.Drawing;
using CryptoWatcher.Properties;
using System.Media;
using Tulpep.NotificationWindow;
using System.Diagnostics;
using System.Threading;
using CryptoWatcher.Utilities;
// TODO: stop sound when user selects option to not play sound
namespace CryptoWatcher.Alert
{
	public class Notification
	{
		private string SOUND_PATH = "Sounds\\AlarmSound.wav";

		private SoundPlayer _soundPlayer = new SoundPlayer();
		private PopupNotifier _popup = new PopupNotifier();

		public Notification(SoundType soundType, bool showWindowsMsg, bool showWindow, int interval = -1)
		{
			Initialize();
			Debug.WriteLine("Created popup: " + Thread.CurrentThread.ManagedThreadId);
			Sound = soundType;
			this.ShowWindowsMsg = showWindowsMsg;
			this.ShowWindow = showWindow;
			Interval = interval;
		}

		public Notification(ref string data)
		{
			Initialize();

			Sound = (SoundType)int.Parse(Utility.GetSubstring(data, ';', 0));
			ShowWindowsMsg = Convert.ToBoolean(int.Parse(Utility.GetSubstring(data, ';', 1)));
			ShowWindow = Convert.ToBoolean(int.Parse(Utility.GetSubstring(data, ';', 2)));
			Interval = int.Parse(Utility.GetSubstring(data, ';', 3));
			data = Utility.GetSubstring(data, ';', 4, false);
		}

		public enum SoundType { looping, sound, noSound };

		// reference to main form to display notifications
		public static MainForm MainForm { get; set; }
		// wait period after alert is triggered
		public int Interval { get; set; }
		public SoundType Sound { get; }
		public bool ShowWindowsMsg { get; }
		public bool ShowWindow { get; }


		public static string SoundTypeToString(SoundType soundType)
		{
			switch (soundType)
			{
				case SoundType.looping: return "Loop Sound";
				case SoundType.sound: return "Play Sound";
				case SoundType.noSound: return "No Sound";
			}
			throw new ArgumentException();
		}

		// notifies user based on alert settings
		public void Notify(int alertIndex)
        {
			if (MainForm.InvokeRequired)
			{
				MainForm.BeginInvoke(new Action(() => Notify(alertIndex)));
			}
			else
			{
				if (Settings.Default.PlaySound)
				{
					switch (Sound)
					{
						case SoundType.looping:
							_soundPlayer.PlayLooping();
							break;
						case SoundType.sound:
							_soundPlayer.Play();
							break;
					}
				}

				if (ShowWindowsMsg)
				{
					_popup.ContentText = AbstractAlert.Alerts[alertIndex].Message;
					Debug.WriteLine("Called popup: " + Thread.CurrentThread.ManagedThreadId);
					_popup.Popup();
				}
				if (ShowWindow)
				{
					MainForm.ShowWindow();
				}
				MainForm.AddMessage(alertIndex);
			}
		}

		public void StopNotifying(int alertIndex, bool tryRemovingAlert)
		{
			_soundPlayer.Stop();
			_popup.Hide();
			if(tryRemovingAlert)
				MainForm.TryRemoveAlert(alertIndex);
		}

		public override string ToString()
		{
			return $"{(int)Sound};{Convert.ToInt32(ShowWindowsMsg)};{Convert.ToInt32(ShowWindow)};{Interval}";
		}

		private void Initialize()
		{
			_soundPlayer = new SoundPlayer(SOUND_PATH);
			_popup.Image = Resources.Info;
			_popup.TitleText = "Alert triggered!";
			_popup.HeaderColor = Color.FromArgb(255, 0, 174, 219);
			_popup.TitleColor = Color.FromArgb(255, 0, 174, 219);
			_popup.ContentHoverColor = Color.FromArgb(255, 0, 174, 219);
			_popup.ShowGrip = false;
			_popup.HeaderHeight = 6;
			_popup.Click += MainForm.Popup_Click;
		}
	}
}
