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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Alerts
{
	class MACDAlert : AbstractAlert
	{
		enum OnType { MACD, Signal, Histogram, MACD_Signal, MACD_Histogram, Signal_Histogram }
		AbsoluteAlert absoluteAlert;
		AbsoluteAlert absoluteAlert2;

		public override string Name { get { return "MACD"; } }
		static string[] types = { "Buy signal (divergence)", "Sell signal (divergence)", "Positive", "Negative", "Custom" };
		int slowLength, fastLength;
		Timeframe timeframe;
		OnType onType;

		public MACDAlert()
		{
			absoluteAlert = new AbsoluteAlert();
			absoluteAlert2 = new AbsoluteAlert();
		}

		// creates alert from line (the one that is in file)
		public MACDAlert(string data)
		{
			BaseFromLine(ref data);
			absoluteAlert = new AbsoluteAlert(ref data);
			absoluteAlert2 = new AbsoluteAlert(ref data);

			int i;
			slowLength = int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			fastLength = int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			timeframe = (Timeframe)int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			onType = (OnType)int.Parse(data.Substring(0));

			AbstractAPI.SubscrbeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, fastLength > slowLength ? fastLength + 10 : slowLength + 10, Test);
		}

		public override string ToLine()
		{
			return $"1;{BaseToLine()};{absoluteAlert};{absoluteAlert2};{slowLength};{fastLength};{(int)timeframe};{(int)onType}";
		}

		protected override void Test(object[] param)
		{
			TA.MACD macd = TA.GetMACD((from c in (List<Candlestick>)param[0] select c.close).ToList(), fastLength, slowLength);
			switch (onType)
			{
				case OnType.MACD:absoluteAlert.Current = macd.Macd.Last();
					break;
				case OnType.Signal:absoluteAlert.Current = macd.Signal.Last();
					break;
				case OnType.Histogram:absoluteAlert.Current = macd.Histogram.Last();
					break;
				case OnType.MACD_Signal:
					absoluteAlert.Current = macd.Macd.Last();
					absoluteAlert2.Current = macd.Signal.Last();
					break;
				case OnType.MACD_Histogram:
					absoluteAlert.Current = macd.Macd.Last();
					absoluteAlert2.Current = macd.Histogram.Last();
					break;
				case OnType.Signal_Histogram:
					absoluteAlert.Current = macd.Signal.Last();
					absoluteAlert2.Current = macd.Histogram.Last();
					break;
			}

			if ((int)onType > 2 && absoluteAlert.Type == TriggerType.crossing)
			{
				if(absoluteAlert.Test(absoluteAlert2))
					BaseTest();
			}
			else if ((int)onType > 2)
			{
				if (absoluteAlert.Test() && absoluteAlert2.Test())
					BaseTest();
			}
			else
			{
				if (absoluteAlert.Test())
					BaseTest();
			}
		}

		// creates a string that is displayed in form
		public override string[] ToRow()
		{
			return new string[]{ alertData.BaseName,
				absoluteAlert.ConditionToString(),
				absoluteAlert.Trigger.ToString(),
				alertData.QuoteSymbol,
				alertData.ExchangeName
			};
		}

		public override string Message {
			get {
				if ((int)onType > 2)
				{
					if (absoluteAlert.Type == TriggerType.crossing && absoluteAlert.Trigger == float.MinValue)
						return $"{alertData.BaseName} [{alertData.BaseSymbol}] {OnTypeToString(onType)} is {absoluteAlert.ConditionToString().ToLower()}, current value ({absoluteAlert.Current},{absoluteAlert2.Current})";
					else
						return $"{alertData.BaseName} [{alertData.BaseSymbol}] {OnTypeToString(onType)} is {absoluteAlert.ConditionToString().ToLower()} {absoluteAlert.Trigger}, current value ({absoluteAlert.Current},{absoluteAlert2.Current})";

				}
				return $"{alertData.BaseName} [{alertData.BaseSymbol}] {OnTypeToString(onType)} is {absoluteAlert.ConditionToString().ToLower()} {absoluteAlert.Trigger}, with the current value of {absoluteAlert.Current}";
			}
		}

		private static string OnTypeToString(OnType type)
		{
			switch (type)
			{
				case OnType.MACD:
					return "MACD";
				case OnType.Signal:
					return "Signal";
				case OnType.Histogram:
					return "Histogram";
				case OnType.MACD_Signal:
					return "MACD & Signal";
				case OnType.MACD_Histogram:
					return "MACD & Histogram";
				case OnType.Signal_Histogram:
					return "Signal & Histogram";
			}
			throw new ArgumentException();
		}

		private static OnType StringToOnType(string type)
		{
			switch (type)
			{
				case "MACD": return OnType.MACD;
				case "HSignal": return OnType.Signal;
				case "Histogram": return OnType.Histogram;
				case "MACD & Signal": return OnType.MACD_Signal;
				case "MACD & Histogram": return OnType.MACD_Histogram;
				case "Signal & Histogram": return OnType.Signal_Histogram;
			}
			throw new ArgumentException();
		}
		
		private static string[] GetOnTypes()
		{
			var values = Enum.GetValues(typeof(OnType)).Cast<OnType>().ToArray();
			string[] types = new string[values.Count()];

			for (int i = 0; i < types.Length; i++)
			{
				types[i] = OnTypeToString(values[i]);
			}
			return types;
		}

		public override MetroFramework.Controls.MetroPanel GetOptions()
		{
			var cBoxType = new MetroFramework.Controls.MetroComboBox
			{
				Location = new System.Drawing.Point(85, 0),
				Name = "cBoxType",
				Size = new System.Drawing.Size(124, 29),
			};
			cBoxType.Items.AddRange(types);
			cBoxType.SelectedIndex = 0;
			cBoxType.SelectedIndexChanged += CBoxType_SelectedIndexChanged;

			var lblType = new MetroFramework.Controls.MetroLabel
			{
				Location = new System.Drawing.Point(36, 10),
				Name = "lblType",
				Size = new System.Drawing.Size(40, 19),
				Text = "Type:"
			};

			var cBoxOn = new MetroComboBox
			{
				Location = new System.Drawing.Point(309, 0),
				Size = new System.Drawing.Size(124, 29),
				Name = "cBoxOn",
				Visible = false
			};
			cBoxOn.Items.AddRange(new object[] { "Fast", "Slow", "Histogram", "Fast & Slow", "Fast & Histogram", "Slow & Histogram" });

			var lblOn = new MetroLabel
			{
				Location = new System.Drawing.Point(270, 10),
				Name = "lblOn",
				Size = new System.Drawing.Size(30, 19),
				Text = "On:",
				Visible = false
			};

			var txtSlowLength = new MetroTextBox
			{
				Location = new System.Drawing.Point(309, 78),
				Name = "txtSlowLength",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "26"
			};

			var lblSlowLength = new MetroLabel
			{
				Location = new System.Drawing.Point(221, 88),
				Name = "lblSlowLength",
				Size = new System.Drawing.Size(79, 19),
				Text = "Slow length:",
				Visible = false,
			};

			var txtFastLength = new MetroTextBox
			{
				Location = new System.Drawing.Point(309, 39),
				Name = "txtFastLength",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "12"
			};

			var lblFastLength = new MetroLabel
			{
				Location = new System.Drawing.Point(225, 49),
				Name = "lblFastLength",
				Size = new System.Drawing.Size(75, 19),
				Text = "Fast length:",
				Visible = false,
			};

			var panel = BaseGetOptions(true);
			panel.Controls["txtValue"].Location = new System.Drawing.Point(85, 117);
			panel.Controls["lblValue"].Location = new System.Drawing.Point(32, 127);
			panel.Controls["lblCondition"].Location = new System.Drawing.Point(7, 88);
			panel.Controls["cBoxCondition"].Location = new System.Drawing.Point(85, 78);
			panel.Controls["txtValue"].Visible = false;
			panel.Controls["lblValue"].Visible = false;
			panel.Controls["lblCondition"].Visible = false;
			panel.Controls["cBoxCondition"].Visible = false;
			panel.Controls.Add(cBoxOn);
			panel.Controls.Add(lblOn);
			panel.Controls.Add(txtSlowLength);
			panel.Controls.Add(lblSlowLength);
			panel.Controls.Add(txtFastLength);
			panel.Controls.Add(lblFastLength);
			// always shown
			object[] controls = GetTimeFrameOptions();
			panel.Controls.Add(((MetroFramework.Controls.MetroComboBox)controls[0]));
			panel.Controls.Add(((MetroFramework.Controls.MetroLabel)controls[1]));
			panel.Controls.Add(cBoxType);
			panel.Controls.Add(lblType);
			return panel;
		}

		public override bool Create(AlertData alertData, Notification notification, MetroFramework.Controls.MetroPanel panel)
		{
			if (panel.Controls["cBoxType"].Text == types[4])
			{
				if (!int.TryParse(panel.Controls["txtSlowLength"].Text, out slowLength))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in slow length section.");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtFastLength"].Text, out fastLength))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in fast length section.");
					return false;
				}
				if (slowLength < 1 || fastLength < 1)
				{
					System.Windows.Forms.MessageBox.Show("Length cannot be less than 1.");
					return false;
				}
				absoluteAlert.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
				absoluteAlert2.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
				if(((MetroComboBox)(panel.Controls["cBoxOn"])).SelectedIndex == -1)
				{
					System.Windows.Forms.MessageBox.Show("Please select on what to trigger.");
					return false;
				}
				onType = StringToOnType(panel.Controls["cBoxOn"].Text);
				if (!float.TryParse(panel.Controls["txtValue"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out absoluteAlert.Trigger))
				{
					if(absoluteAlert.Type!=TriggerType.crossing && (int)onType > 2)
					{
						System.Windows.Forms.MessageBox.Show("Something is wrong in value section.");
						return false;
					}
					absoluteAlert.Trigger = float.MinValue;
				}
				absoluteAlert2.Trigger = absoluteAlert.Trigger;
			}
			else if(panel.Controls["cBoxType"].Text == "Buy signal (divergence)")
			{
				absoluteAlert.Type = TriggerType.crossing;
				onType = OnType.MACD_Signal;
			}
			else if (panel.Controls["cBoxType"].Text == "Sell signal (divergence)")
			{
				absoluteAlert.Type = TriggerType.crossing;
				onType = OnType.MACD_Signal;
			}
			else if (panel.Controls["cBoxType"].Text == "Positive")
			{
				absoluteAlert.Type = TriggerType.higher;
				absoluteAlert.Trigger = 0;
				onType = OnType.MACD;
			}
			else if (panel.Controls["cBoxType"].Text == "Negative")
			{
				absoluteAlert.Type = TriggerType.lower;
				absoluteAlert.Trigger = 0;
				onType = OnType.MACD;
			}
			else
			{
				// combobox has different text other than expected
				throw new ArgumentException();
			}
			if (((MetroComboBox)panel.Controls["cBoxTimeframe"]).SelectedIndex == -1)
			{
				System.Windows.Forms.MessageBox.Show("Please select timeframe.");
				return false;
			}
			slowLength = int.Parse(panel.Controls["txtSlowLength"].Text);
			fastLength = int.Parse(panel.Controls["txtFastLength"].Text);
			timeframe = AbstractAPI.StringToTimeframe(panel.Controls["cBoxTimeframe"].Text);
			BaseInit(alertData, notification);
			AbstractAPI.SubscrbeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, fastLength > slowLength ? fastLength + 10 : slowLength + 10, Test);
			return true;
		}
		public override void Destroy()
		{
			Unsubscribe();
			BaseDestroy();
		}

		protected override void Unsubscribe()
		{
			AbstractAPI.UnsubscribeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, Test);
		}
	}
}
