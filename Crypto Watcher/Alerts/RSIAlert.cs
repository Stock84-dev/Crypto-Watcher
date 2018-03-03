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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using CryptoWatcher.APIs;
using MetroFramework.Controls;

namespace CryptoWatcher.Alerts
{
	class RSIAlert : AbstractAlert
	{
		enum Source { open, high, low, close }; 
		AbsoluteAlert absoluteAlert;
		int length = 14;
		Timeframe timeframe;
		Source source = Source.close;
		public override string Name { get { return "RSI"; } }
		static string[] types = { "Overbought", "Oversold", "Custom" };

		public RSIAlert() { absoluteAlert = new AbsoluteAlert(); }

		public RSIAlert(string data)
		{
			BaseFromLine(ref data);
			absoluteAlert = new AbsoluteAlert(ref data);
			
			int i;
			length = int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			timeframe = (Timeframe)int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			source = (Source)int.Parse(data.Substring(0));

			AbstractAPI.SubscrbeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, length, Test);
		}

		public override string Message {
			get {
				return $"{alertData.BaseName} [{alertData.BaseSymbol}{alertData.QuoteSymbol}] with the current RSI of {absoluteAlert.Current} is {absoluteAlert.ConditionToString().ToLower()} {absoluteAlert.Trigger}.";
			}
		}

		protected override void Test(object[] param)
		{
			//List<float> r = TA.GetRSI((from c in candlesticks select c.close).ToList(), period);
			//System.IO.StreamWriter sw2 = new System.IO.StreamWriter("rsi.txt");
			//for (int i = 0; i < r.Count; i++)
			//{
			//	sw2.WriteLine(r[i]);
			//}
			//sw2.Close();

			switch (source)
			{
				case Source.open:
					absoluteAlert.Current = TA.GetRSI((from c in (List<Candlestick>)param[0] select c.open).ToList(), length).Last();
					break;
				case Source.high:
					absoluteAlert.Current = TA.GetRSI((from c in (List<Candlestick>)param[0] select c.high).ToList(), length).Last();
					break;
				case Source.low:
					absoluteAlert.Current = TA.GetRSI((from c in (List<Candlestick>)param[0] select c.low).ToList(), length).Last();
					break;
				case Source.close:
					absoluteAlert.Current = TA.GetRSI((from c in (List<Candlestick>)param[0] select c.close).ToList(), length).Last();
					break;
			}
			if (absoluteAlert.Test())
				BaseTest(timeframe, length);
		}

		public override void Destroy()
		{
			Unsubscribe();
			BaseDestroy();
		}

		public override string ToLine()
		{
			return $"2;{BaseToLine()};{absoluteAlert};{length};{(int)timeframe};{(int)source}";
		}

		public override string[] ToRow()
		{
			return new string[]{ alertData.BaseName,
				"RSI",
				absoluteAlert.Trigger.ToString(),
				alertData.QuoteSymbol,
				alertData.ExchangeName
			};
		}

		public override bool Create(AlertData alertData, Notification notification, MetroFramework.Controls.MetroPanel panel)
		{
			// custom alert
			if(panel.Controls["cBoxType"].Text == types[2])
			{
				if (!float.TryParse(panel.Controls["txtValue"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out absoluteAlert.Trigger))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in price section.");
					return false;
				}
				if (absoluteAlert.Trigger > 100 || absoluteAlert.Trigger < 0)
				{
					System.Windows.Forms.MessageBox.Show("RSI value must be between 0 and 100");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtLength"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out length))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in length section.");
					return false;
				}
				if(length <= 0)
				{
					System.Windows.Forms.MessageBox.Show("Length cannot be less than 1.");
					return false;
				}
				absoluteAlert.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
				source = StringToSource(panel.Controls["cBoxSource"].Text);
			}
			else if (panel.Controls["cBoxType"].Text == types[0]) // overbought
			{
				absoluteAlert.Trigger = 70;
				absoluteAlert.Type = TriggerType.higherOrEqual;
			}
			else if (panel.Controls["cBoxType"].Text == types[1]) // oversold
			{
				absoluteAlert.Trigger = 30;
				absoluteAlert.Type = TriggerType.lowerOrEqual;
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

			timeframe = AbstractAPI.StringToTimeframe(panel.Controls["cBoxTimeframe"].Text);
			BaseInit(alertData, notification);
			AbstractAPI.SubscrbeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, length, Test);
			return true;
		}

		private static string[] GetSources()
		{
			var values = Enum.GetValues(typeof(Source)).Cast<Source>().ToArray();
			string[] types = new string[values.Count()];

			for (int i = 0; i < types.Length; i++)
			{
				types[i] = SourceToString(values[i]);
			}
			return types;
		}

		private static Source StringToSource(string source)
		{
			switch (source)
			{
				case "Open":
					return Source.open;
				case "High":
					return Source.high;
				case "Low":
					return Source.low;
				case "Close":
					return Source.close;
			}
			throw new ArgumentException();
		}

		private static string SourceToString(Source source)
		{
			switch (source)
			{
				case Source.open:
					return "Open";
				case Source.high:
					return "High";
				case Source.low:
					return "Low";
				case Source.close:
					return "Close";
			}
			throw new ArgumentException();
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

			var lblLength = new MetroFramework.Controls.MetroLabel
			{
				Location = new System.Drawing.Point(249, 10),
				Name = "lblLength",
				Size = new System.Drawing.Size(51, 19),
				Text = "Length:",
				Visible = false
			};

			var txtLength = new MetroFramework.Controls.MetroTextBox
			{
				Location = new System.Drawing.Point(309, 0),
				Name = "txtLength",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "14"
			};

			var lblSource = new MetroFramework.Controls.MetroLabel
			{
				Location = new System.Drawing.Point(249, 49),
				Name = "lblSource",
				Size = new System.Drawing.Size(52, 19),
				Text = "Source:",
				Visible = false
			};

			var cBoxSource = new MetroFramework.Controls.MetroComboBox
			{
				Location = new System.Drawing.Point(309, 39),
				Name = "cBoxSource",
				Size = new System.Drawing.Size(124, 29),
				Visible = false
			};
			cBoxSource.Items.AddRange(GetSources());
			cBoxSource.SelectedIndex = 3;

			var panel = BaseGetOptions(true);
			panel.Controls["txtValue"].Location = new System.Drawing.Point(85, 117);
			panel.Controls["lblValue"].Location = new System.Drawing.Point(32, 127);
			panel.Controls["lblCondition"].Location = new System.Drawing.Point(7, 88);
			panel.Controls["cBoxCondition"].Location = new System.Drawing.Point(85, 78);
			panel.Controls["txtValue"].Visible = false;
			panel.Controls["lblValue"].Visible = false;
			panel.Controls["lblCondition"].Visible = false;
			panel.Controls["cBoxCondition"].Visible = false;
			panel.Controls.Add(lblLength);
			panel.Controls.Add(txtLength);
			panel.Controls.Add(lblSource);
			panel.Controls.Add(cBoxSource);
			// always shown
			object[] controls = GetTimeFrameOptions();
			panel.Controls.Add(((MetroFramework.Controls.MetroComboBox)controls[0]));
			panel.Controls.Add(((MetroFramework.Controls.MetroLabel)controls[1]));
			panel.Controls.Add(cBoxType);
			panel.Controls.Add(lblType);
			return panel;
		}

		protected override void Unsubscribe()
		{
			AbstractAPI.UnsubscribeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, Test);
		}
	}
}
