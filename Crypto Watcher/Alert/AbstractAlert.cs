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
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoWatcher.Utilities;
using System.Reflection;
using System.Windows.Forms;

// TODO: add tooltips on custom controls of alert
namespace CryptoWatcher.Alert
{
    // for template look at priceAlert.cs
	abstract class AbstractAlert
	{
		// where alerts are saved on disk
		private const string SAVE_DIR = "Saves/";
		private const string SAVE_PATH = SAVE_DIR + "Alerts.txt";

		private static IEnumerable<Type> _childClassTypes = null;
		private static Task _allSubscribed;
		/// <summary>
		/// Gets called when user creates alert and there isn't enough data on server.
		/// </summary>
		public static event OnSubscribeFailEventHandler OnSubscribeFail;
		public delegate void OnSubscribeFailEventHandler(object sender, EventArgs e);
		public delegate void OnPriceUpdateEventHandler(object sender, PriceUpdateEventArgs e);
		public static event OnPriceUpdateEventHandler PriceChanged;

		private bool _isNotifying = false;
		private bool _subscribed = false;
		private float _price;
		/// <summary>
		/// Waits for alert to subscribe and then you can unsibscribe.
		/// </summary>
		private Task<bool> _subscribedTask;
		private Notification _notification;
		private DateTime _lastTriggered = new DateTime(1, 1, 1, 1, 1, 1, 1);

		protected Timeframe timeframe;

		public AbstractAlert() { }
		// List of all alerts that are running.
		public static List<AbstractAlert> Alerts { get; } = new List<AbstractAlert>();

		public Notification Notification { get { return _notification; } }
		/// <summary>
		/// Returns: alert id, basename[basesymbol], quotesymbol, exchangename, alertname, condition
		/// </summary>
		public object[] Row
		{
			get {
				if (AlertDisplay.Length != 2)
					throw new Exception($"Alert display lines not equal, for {GetType().Name}, \"AlertDisplay\" getter not implemented properly.");
				return new object[] { Alerts.IndexOf(this), $"{AlertData.BaseName} [{AlertData.BaseSymbol}]", AlertData.ExchangeName, Name, AlertDisplay[0], AbstractAPI.TimeframeToString(timeframe), $"{_price} {AlertData.QuoteSymbol}", AlertDisplay[1]};
			}
		}
		public AlertData AlertData { get; set; }
		public virtual string IndicatorValueToolTip { get { return null; } }
		/// <summary>
		/// Message that user recieves when alert notifies.
		/// </summary>
		public abstract string Message { get; }
		/// <summary>
		/// Name of the alert.
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// Gives condition, current indicator value, current price
		/// </summary>
		protected abstract string[] AlertDisplay { get; }
		protected abstract CandleLength CandlesLength { get; }

		public static AbstractAlert[] CreateChildClasses()
		{
			if (_childClassTypes == null)
				_childClassTypes = Assembly.GetAssembly(typeof(AbstractAlert)).GetTypes().Where(t => t.IsSubclassOf(typeof(AbstractAlert)));

			AbstractAlert[] output = new AbstractAlert[_childClassTypes.Count()];
			for (int i = 0; i < output.Length; i++)
				output[i] = (AbstractAlert)Activator.CreateInstance(_childClassTypes.ElementAt(i).UnderlyingSystemType);

			return output;
		}

		public static AbstractAlert CreateChildClass(Type childClassType)
		{
			if (!_childClassTypes.Any(x => x == childClassType))
				throw new ArgumentException("Type isn't child class of an alert.");
			return (AbstractAlert)Activator.CreateInstance(childClassType.UnderlyingSystemType);
		}

		// creates different alert types from line
		public static AbstractAlert FromLine(string data)
		{
			int id = int.Parse(Utility.GetSubstring(data, ';', 0));
			data = Utility.GetSubstring(data, ';', 1, false);

			AbstractAlert tmp = CreateChildClass(id);
			tmp.BaseFromLine(data);
			return tmp;
		}

		/// <summary>
		/// Unsubscribes all alerts that are subscribed to API.
		/// </summary>
		public static void UnsubscribeAll()
		{
			foreach (var alert in Alerts)
			{
				alert.Unsubscribe();
			}
		}

		// loads all alerts
		public static async Task LoadAlertsAsync()
		{
			if (!Directory.Exists(SAVE_DIR))
				Directory.CreateDirectory(SAVE_DIR);
			if (!File.Exists(SAVE_PATH))
				File.Create(SAVE_PATH);
			else
			{
				IOManager ioManager = new IOManager(SAVE_PATH);
				await ioManager.LoadByLineAsync(line =>
				{
					Alerts.Add(FromLine(line));
				});
			}
			
			_allSubscribed = SubscribeAll();
		}

		// Notifies user.
		public void Notify(DateTime currentTime)
		{
			_lastTriggered = currentTime;
			_notification.Notify(Alerts.IndexOf(this));
			_isNotifying = true;
		}
        // Stops sound and maybe removes alert from list.
		public void StopNotifying(bool tryRemovingAlert = true, bool resetLastTriggered = false)
		{
			if(resetLastTriggered)
				_lastTriggered = new DateTime(1, 1, 1, 1, 1, 1, 1);
			_notification.StopNotifying(Alerts.IndexOf(this), tryRemovingAlert);
			_isNotifying = false;
		}

		/// <summary>
		/// Creates alert from modified data that user specified when creating alert.
		/// </summary>
		/// <param name="panel">Panel with filled controls.</param>
		/// <param name="edited">If alert is edited don't add it to list (already in it).</param>
		/// <returns></returns>
		public bool CreateAlert(AlertData alertData, Notification notification, MetroPanel panel, bool edited = false, AbstractAlert newAlert = null)
		{
			// Alert type is changed
			if (newAlert != null)
			{
				newAlert.AlertData = alertData;
				newAlert._notification = notification;
				if (newAlert.Create(panel))
				{
					Alerts[Alerts.IndexOf(this)] = newAlert;
					newAlert.Subscribe(newAlert.CandlesLength.MinLength, newAlert.CandlesLength.MaxLength);
					SaveAlertsAsync();
					return true;
				}
				
			}

			AlertData = alertData;
			_notification = notification;
			if(Create(panel))
			{
				// initializing alert
				if (!edited)
				{
					Alerts.Add(this);
				}
				Subscribe(CandlesLength.MinLength, CandlesLength.MaxLength);
				SaveAlertsAsync();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Unsubscribes from API and destroys itself.
		/// </summary>
		public void Destroy()
		{
			Unsubscribe();
			StopNotifying(false);
			Alerts.Remove(this);
			SaveAlertsAsync();
		}
		/// <summary>
		/// Gets called from outside to unsubscribe from API.
		/// </summary>
		public async void Unsubscribe()
		{
			if (!_subscribed)
				return;
			if (timeframe == Timeframe.NONE)
				AbstractAPI.CryptoCompareAPI.PriceUnsubscribe(AlertData.ExchangeName, AlertData.BaseSymbol, AlertData.QuoteSymbol, Tester);
			else
			{
				await _subscribedTask;
				AbstractAPI.CryptoCompareAPI.CandleUnsubscribe(AlertData.ExchangeName, AlertData.BaseSymbol, AlertData.QuoteSymbol, timeframe, Tester);
			}
			_subscribed = false;
		}

		public override string ToString() { return Name; }

		/// <summary>
		/// Gets panel with controls which is used for creating alert.
		/// </summary>
		public abstract MetroPanel GetUI();
		/// <summary>
		/// Gets panel with controls that are already filled, i.e. edit alert.
		/// </summary>
		public abstract MetroPanel GetFilledUI();
		/// <summary>
		/// Creates alert from CSV string.
		/// </summary>
		protected abstract void Init(string data);
		/// <summary>
		/// Subscribed event, gets called when API downloads data.
		/// </summary>
		/// <param name="param">Downloaded data from API, usually price or candlestick array</param>
		protected abstract void Test(object param);
		/// <summary>
		/// Creates alert from modified data that user specified when creating alert.
		/// </summary>
		/// <param name="panel">Panel with filled controls.</param>
		/// <param name="edited">If alert is edited don't add it to list (already in it).</param>
		/// <returns></returns>
		protected abstract bool Create(MetroPanel panel);
		/// <summary>
		/// Converts alert object to csv line.
		/// </summary>
		protected abstract string ToLine();
		/// <summary>
		/// Use this as subscription action.
		/// </summary>
		protected async void Tester(PriceUpdate priceUpdate)
		{
			if (priceUpdate.Price != -1)
			{
				Test(priceUpdate.Price);
				_price = priceUpdate.Price;
			}
			else
			{
				await _allSubscribed;
				_price = priceUpdate.Candlesticks.Last().close;
				Test(priceUpdate.Candlesticks);
			}
			OnPriceUpdate();
		}

		public void Subscribe()
		{
			Subscribe(CandlesLength.MinLength, CandlesLength.MaxLength);
		}

		/// <param name="minLength">Set to -1 to get all data that is currently cached on server or to 0 if it only needs price.</param>
		/// <param name="maxLength">If there is data on server then downloads candles with maxLength.</param>
		protected async void Subscribe(int minLength = 0, int maxLength = -1, bool autocall = true)
		{
			if (minLength > 0)
			{
				_subscribedTask = AbstractAPI.CryptoCompareAPI.CandleSubscribe(AlertData.ExchangeName, AlertData.BaseSymbol, AlertData.QuoteSymbol, timeframe, minLength, Tester, maxLength, autocall);
				await _subscribedTask;
				if (!_subscribedTask.Result)
					OnSubscribeFail?.Invoke(this, new EventArgs());
			}
			else
				AbstractAPI.CryptoCompareAPI.PriceSubscribe(AlertData.ExchangeName, AlertData.BaseSymbol, AlertData.QuoteSymbol, Tester);
			_subscribed = true;
		}

		// Prevents spamming notifications.
		protected void BaseTest(Timeframe timeframe = Timeframe.NONE, int minLength = -1, int maxLength = -1)
		{
			DateTime currentTime = DateTime.Now;
			
			if (_lastTriggered + new TimeSpan(0, 0, _notification.Interval) <= currentTime && !_isNotifying)
			{
				Notify(currentTime);
				Resubscribe(minLength, maxLength);
			}
		}

        /// <summary>
        /// Creates panel.
        /// </summary>
        /// <param name="addAbsoluteAlert">Include default controls?</param>
        /// <returns></returns>
		protected MetroPanel BaseGetOptions(bool addAbsoluteAlert = false)
		{
			var panel = new MetroPanel
			{
				Location = new System.Drawing.Point(278, 7),
				Name = "Panel",
				Size = new System.Drawing.Size(433, 146),
			};

			if (addAbsoluteAlert)
			{
				object[] controls = AbsoluteAlert.GetOptions();

				panel.Controls.Add(((MetroTextBox)controls[0]));
				panel.Controls.Add(((MetroComboBox)controls[1]));
				panel.Controls.Add(((MetroLabel)controls[2]));
				panel.Controls.Add(((MetroLabel)controls[3]));
			}

			return panel;
		}
        // gets timeframe controls
		protected object[] GetTimeFrameOptions()
		{
			var cBoxTimeframe = new MetroComboBox
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
			var panel = (MetroPanel)((MetroComboBox)sender).Parent;
			int nShown = 4;
			if ((string)((MetroComboBox)panel.Controls["cBoxType"]).SelectedItem == "Custom")
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

		// saves all alerts, automatically managed
		private static void SaveAlertsAsync()
		{
			IOManager iOManager = new IOManager(SAVE_PATH);
			iOManager.SaveByLineAsync(Alerts.ToList().Select(x => x.BaseToLine()));
		}

		private async static Task SubscribeAll()
		{
			Dictionary<string, CandleLength> alertLengths = new Dictionary<string, CandleLength>();

			foreach (var alert in Alerts)
			{
				if (alert.CandlesLength.MinLength == 0)
				{
					alert.Subscribe();
					continue;
				}
				string key = $"{alert.AlertData.ExchangeName};{alert.AlertData.BaseSymbol};{alert.AlertData.QuoteSymbol};{alert.timeframe}";

				if (!alertLengths.ContainsKey(key))
					alertLengths.Add(key, alert.CandlesLength);
				if (alertLengths[key].MinLength < alert.CandlesLength.MinLength)
					alertLengths[key].MinLength = alert.CandlesLength.MinLength;
				if (alertLengths[key].MaxLength < alert.CandlesLength.MaxLength)
					alertLengths[key].MaxLength = alert.CandlesLength.MaxLength;
			}

			foreach (var key in alertLengths.Keys)
			{
				foreach (var alert in Alerts)
				{
					string alertKey = $"{alert.AlertData.ExchangeName};{alert.AlertData.BaseSymbol};{alert.AlertData.QuoteSymbol};{alert.timeframe}";
					if (key == alertKey)
					{
						alert.Subscribe(alertLengths[key].MinLength, alertLengths[key].MaxLength, false);
						await alert._subscribedTask;
					}
				}
			}

			//foreach (var alert in Alerts)
			//{
			//	string key = $"{alert.AlertData.ExchangeName};{alert.AlertData.BaseSymbol};{alert.AlertData.QuoteSymbol};{alert.timeframe}";
			//	if (!alertLengths.ContainsKey(key))
			//		alert.Subscribe();
			//	alert.Subscribe(alertLengths[key].MinLength, alertLengths[key].MaxLength, false);
			//	await alert._subscribedTask;
			//}
		}

		/// <param name="id">Child class order in reflection.</param>
		private static AbstractAlert CreateChildClass(int id)
		{
			if (_childClassTypes == null)
				_childClassTypes = Assembly.GetAssembly(typeof(AbstractAlert)).GetTypes().Where(t => t.IsSubclassOf(typeof(AbstractAlert)));

			return (AbstractAlert)Activator.CreateInstance(_childClassTypes.ElementAt(id).UnderlyingSystemType);
		}

		// creates alert data from line
		private void BaseFromLine(string data)
		{
			AlertData = new AlertData(ref data);
			_notification = new Notification(ref data);
			_lastTriggered = DateTime.Parse(Utility.GetSubstring(data, ';', 0));
			timeframe = (Timeframe)int.Parse(Utility.GetSubstring(data, ';', 1));
			data = Utility.GetSubstring(data, ';', 2, false);

			Init(data);
		}

		/// <summary>
		/// Unsubscribes from api and resubscribes after notification.interval.
		/// </summary>
		private async void Resubscribe(int minLength, int maxLength)
		{
			Unsubscribe();
			if (_notification.Interval != -1)
			{
				await Task.Delay(_notification.Interval * 1000);
				Subscribe(minLength, maxLength);
			}
		}

		private int GetAlertTypeId()
		{
			for (int i = 0; i < CreateChildClasses().Length; i++)
			{
				if (_childClassTypes.ElementAt(i) == GetType())
					return i;
			}
			throw new Exception("Type doesn't exist in a collection.");
		}

		// returns alert data to line
		private string BaseToLine()
		{
			return $"{GetAlertTypeId()};{AlertData};{_notification};{_lastTriggered};{(int)timeframe};{ToLine()}";
		}

		private void OnPriceUpdate()
		{
			PriceChanged?.Invoke(this, new PriceUpdateEventArgs() { Row = Row, AlertId = Alerts.IndexOf(this) });
		}

		protected class CandleLength
		{
			public CandleLength(int minLength, int maxLength = -1)
			{
				MinLength = minLength;
				MaxLength = maxLength;
			}

			public int MinLength { get; set; }
			public int MaxLength { get; set; }

		}
	}

	public class PriceUpdateEventArgs : EventArgs
	{
		public object[] Row { get; set; }
		public int AlertId { get; set; }
	}
}
