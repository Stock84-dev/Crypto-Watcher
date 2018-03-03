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

using CryptoWatcher.APIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Alerts
{
    // for template look at priceAlert.cs
	abstract class AbstractAlert
	{
        // where alerts are saved on disk
		const string save_path = "Saves/Alerts.txt";

		protected AlertData alertData = new AlertData();

		public Notification notification { get; set; }
		public bool IsMessageShowing { get; set; } = false;
		public DateTime LastTriggered { get; set; } = new DateTime(1, 1, 1, 1, 1, 1, 1);
		public bool IsNotifying { get; set; } = false;
        // List of all alerts that are running.
		public static List<AbstractAlert> alerts = new List<AbstractAlert>();

		public AbstractAlert() { }

		public AbstractAlert(AlertData alertData, Notification notification)
		{
			this.alertData = alertData;
			this.notification = notification;
		}
        // Notifies user.
		public void Notify(DateTime currentTime)
		{
			LastTriggered = currentTime;
			notification.Notify(alerts.IndexOf(this));
			IsNotifying = true;
		}
        // Stops sound and maybe removes alert from list.
		public void StopNotifying(bool tryRemovingAlert = true)
		{
			notification.StopNotifying(alerts.IndexOf(this), tryRemovingAlert);
			IsNotifying = false;
		}

		// creates different alert types from line
		public static AbstractAlert FromLine(string data)
		{
			switch (data[0])
			{
				case '1':
					return new PriceAlert(data.Substring(2));
				case '2':
					return new RSIAlert(data.Substring(2));
				case '3':
					return new StochAlert(data.Substring(2));
				case '4':
					return new MACDAlert(data.Substring(2));
				default:
					return null;
			}
		}

		public static void UnsubscribeAll()
		{
			alerts.ForEach(x => x.Unsubscribe());
		}

        /// <summary>
        /// Creates alert from modified data that user specified when creating alert.
        /// </summary>
		public abstract bool Create(AlertData alertData, Notification notification, MetroFramework.Controls.MetroPanel panel);
        /// <summary>
        /// Converts alert object to csv line.
        /// </summary>
		public abstract string ToLine();
        /// <summary>
        /// Subscribed event, gets called when API downloads data.
        /// </summary>
        /// <param name="param">Downloaded data from API, usually price or candlestick array</param>
		protected abstract void Test(object[] param);
        /// <summary>
        /// Gets string to display in listview control.
        /// </summary>
		public abstract string[] ToRow();
        /// <summary>
        /// Message that user recieves when alert notifies.
        /// </summary>
		public abstract string Message { get; }
        /// <summary>
        /// Name of the alert.
        /// </summary>
		public abstract string Name { get; }
        /// <summary>
        /// Gets panel with controls which is used for creating alert.
        /// </summary>
		public abstract MetroFramework.Controls.MetroPanel GetOptions();
        /// <summary>
        /// Unsubscribes from API and destroys itself.
        /// </summary>
		public abstract void Destroy();
		/// <summary>
		/// Gets called from outside to unsubscribe from API.
		/// </summary>
		protected abstract void Unsubscribe();

		protected void BaseDestroy()
		{
			StopNotifying(false);
			alerts.Remove(this);
			SaveAlertsAsync(alerts);
		}

		protected void BaseInit(AlertData alertData, Notification notification)
		{
			this.alertData = alertData;
			this.notification = notification;
			alerts.Add(this);
			SaveAlertsAsync(alerts);
		}

		// returns alert data to line
		protected string BaseToLine()
		{
			return $"{alertData};{notification};{LastTriggered}";
		}

		// creates alert data from line
		protected void BaseFromLine(ref string data)
		{
			int i;
			alertData = new AlertData(ref data);
			notification = new Notification(ref data);
			LastTriggered = DateTime.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
		}
        // Prevents spamming notifications.
		protected void BaseTest(Timeframe timeframe = Timeframe.NONE, int length = -1)
		{
			DateTime currentTime = DateTime.Now;

			if (LastTriggered + new TimeSpan(0, 0, notification.Interval) <= currentTime && !IsNotifying)
			{
				Notify(currentTime);
				if (timeframe == Timeframe.NONE)
					ResubscribePrice();
				else ResubscribeCandle(timeframe, length);
			}
		}
		/// <summary>
		/// Unsubscribes from api and resubscribes after notification.interval.
		/// </summary>
		protected async void ResubscribeCandle(Timeframe timeframe, int length)
		{
			AbstractAPI.UnsubscribeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, Test);
			if (notification.Interval != -1)
			{
				await Task.Delay(notification.Interval * 1000);
				AbstractAPI.SubscrbeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, length, Test);
			}
		}

		/// <summary>
		/// Unsubscribes from api and resubscribes after notification.interval.
		/// </summary>
		protected async void ResubscribePrice()
		{
			AbstractAPI.UnsubscribePrice(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, Test);
			if (notification.Interval != -1)
			{
				await Task.Delay(notification.Interval * 1000);
				AbstractAPI.SubscrbePrice(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, Test);
			}
		}

		// saves all alerts, automatically managed
		protected static async void SaveAlertsAsync(List<AbstractAlert> data)
		{
			StreamWriter sw = new StreamWriter(save_path);
			foreach (var a in data)
			{
				await sw.WriteLineAsync(a.ToLine());
			}
			sw.Close();
		}

		// loads all alerts
		public static async Task LoadAlertsAsync()
		{
			if (!File.Exists(save_path))
				return;
			StreamReader sr = new StreamReader(save_path);
			string line = string.Empty;
			while ((line = await sr.ReadLineAsync()) != null)
			{
				alerts.Add(FromLine(line));
			}
			sr.Close();
		}

        /// <summary>
        /// Creates panel.
        /// </summary>
        /// <param name="addAbsoluteAlert">Include default controls?</param>
        /// <returns></returns>
		protected MetroFramework.Controls.MetroPanel BaseGetOptions(bool addAbsoluteAlert = false)
		{
			var panel = new MetroFramework.Controls.MetroPanel
			{
				Location = new System.Drawing.Point(278, 7),
				Name = "Panel",
				Size = new System.Drawing.Size(433, 146),
			};

			if (addAbsoluteAlert)
			{
				object[] controls = AbsoluteAlert.GetOptions();

				panel.Controls.Add(((MetroFramework.Controls.MetroTextBox)controls[0]));
				panel.Controls.Add(((MetroFramework.Controls.MetroComboBox)controls[1]));
				panel.Controls.Add(((MetroFramework.Controls.MetroLabel)controls[2]));
				panel.Controls.Add(((MetroFramework.Controls.MetroLabel)controls[3]));
			}

			return panel;
		}
        // gets timeframe controls
		protected object[] GetTimeFrameOptions()
		{
			var cBoxTimeframe = new MetroFramework.Controls.MetroComboBox
			{
				FormattingEnabled = true,
				ItemHeight = 23,
				Location = new System.Drawing.Point(85, 39),
				Name = "cBoxTimeframe",
				Size = new System.Drawing.Size(124, 29),
				TabIndex = 22,
				UseSelectable = true
			};
			cBoxTimeframe.Items.AddRange(AbstractAPI.GetTimeframes());

			var lblTimeframe = new MetroFramework.Controls.MetroLabel
			{
				AutoSize = true,
				Location = new System.Drawing.Point(0, 49),
				Name = "lblTimeframe",
				Size = new System.Drawing.Size(76, 19),
				TabIndex = 21,
				Text = "Timeframe:"
			};

			return new object[] { cBoxTimeframe, lblTimeframe };
		}

		// changing visibillity of custom controls if custom option is selected
		protected void CBoxType_SelectedIndexChanged(object sender, EventArgs e)
		{
			var panel = (MetroFramework.Controls.MetroPanel)((MetroFramework.Controls.MetroComboBox)sender).Parent;
			int nShown = 3;
			if (((MetroFramework.Controls.MetroComboBox)panel.Controls["cBoxType"]).SelectedIndex == 2)
			{
				for (int i = 0; i < panel.Controls.Count - nShown; i++)
				{
					panel.Controls[i].Visible = true;
				}
			}
			else
			{
				for (int i = 0; i < panel.Controls.Count - nShown; i++)
				{
					panel.Controls[i].Visible = false;
				}
			}
			panel.Refresh();
		}
	}
}
