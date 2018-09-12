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
using System.Globalization;
using CryptoWatcher.APIs;
using MetroFramework.Controls;
using CryptoWatcher.Utilities;
using System.Threading.Tasks;
// TODO: Use databeses.
namespace CryptoWatcher.Alert
{
	class RSIAlert : AbstractAlert
	{
		private AbsoluteAlert _absoluteAlert;
		private int _length = 14;
		private Source _source = Source.close;
		private int _type;

		public RSIAlert() { _absoluteAlert = new AbsoluteAlert(); }

		protected override void Init(string data)
		{
			_absoluteAlert = new AbsoluteAlert(ref data);
			_length = int.Parse(Utility.GetSubstring(data, ';', 0));
			_source = (Source)int.Parse(Utility.GetSubstring(data, ';', 1));
			_type = int.Parse(Utility.GetSubstring(data, ';', 2));
		}

		private enum Source { open, high, low, close };

		public static string[] Types { get { return new string[] { "Overbought", "Oversold", "Custom" }; } }
		public override string Name { get { return "RSI"; } }
		public override string Message {
			get {
				return $"{AlertData.BaseName} [{AlertData.ExchangeName}:{AlertData.BaseSymbol}{AlertData.QuoteSymbol}] with the current RSI of {_absoluteAlert.Current} is {_absoluteAlert.ConditionToString().ToLower()} {_absoluteAlert.Trigger}.";
			}
		}

		public override MetroPanel GetFilledUI()
		{
			MetroPanel panel = GetUI();
			((MetroComboBox)(panel.Controls["cBoxType"])).SelectedItem = Types[_type];
			panel.Controls["txtValue"].Text = _absoluteAlert.Trigger.ToString();
			panel.Controls["txtLength"].Text = _length.ToString();
			((MetroComboBox)panel.Controls["cBoxSource"]).SelectedItem = SourceToString(_source);
			((MetroComboBox)(panel.Controls["cBoxTimeframe"])).SelectedItem = AbstractAPI.TimeframeToString(timeframe);
			return panel;
		}

		protected override bool Create(MetroPanel panel)
		{
			_absoluteAlert = new AbsoluteAlert();
			// custom alert
			if (panel.Controls["cBoxType"].Text == Types[2])
			{
				if (!float.TryParse(panel.Controls["txtValue"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _absoluteAlert.Trigger))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in price section.");
					return false;
				}
				if (_absoluteAlert.Trigger > 100 || _absoluteAlert.Trigger < 0)
				{
					System.Windows.Forms.MessageBox.Show("RSI value must be between 0 and 100");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtLength"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _length))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in length section.");
					return false;
				}
				if (_length <= 0)
				{
					System.Windows.Forms.MessageBox.Show("Length cannot be less than 1.");
					return false;
				}
				_type = 2;
				_absoluteAlert.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
				_source = StringToSource(panel.Controls["cBoxSource"].Text);
			}
			else if (panel.Controls["cBoxType"].Text == Types[0]) // overbought
			{
				_absoluteAlert.Trigger = 70;
				_absoluteAlert.Type = TriggerType.higherOrEqual;
				_type = 0;
			}
			else if (panel.Controls["cBoxType"].Text == Types[1]) // oversold
			{
				_absoluteAlert.Trigger = 30;
				_absoluteAlert.Type = TriggerType.lowerOrEqual;
				_type = 1;
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
			return true;
		}

		public override MetroPanel GetUI()
		{
			var cBoxType = new MetroComboBox
			{
				Location = new System.Drawing.Point(85, 0),
				Name = "cBoxType",
				Size = new System.Drawing.Size(124, 29),
			};
			cBoxType.Items.AddRange(Types);
			cBoxType.SelectedIndex = 0;
			cBoxType.SelectedIndexChanged += CBoxType_SelectedIndexChanged;

			var lblType = new MetroLabel
			{
				Location = new System.Drawing.Point(36, 10),
				Name = "lblType",
				Size = new System.Drawing.Size(40, 19),
				Text = "Type:"
			};

			var lblLength = new MetroLabel
			{
				Location = new System.Drawing.Point(249, 10),
				Name = "lblLength",
				Size = new System.Drawing.Size(51, 19),
				Text = "Length:",
				Visible = false
			};

			var txtLength = new MetroTextBox
			{
				Location = new System.Drawing.Point(309, 0),
				Name = "txtLength",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "14"
			};

			var lblSource = new MetroLabel
			{
				Location = new System.Drawing.Point(249, 49),
				Name = "lblSource",
				Size = new System.Drawing.Size(52, 19),
				Text = "Source:",
				Visible = false
			};

			var cBoxSource = new MetroComboBox
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
			panel.Controls.Add(((MetroLabel)controls[1]));
			panel.Controls.Add(((MetroComboBox)controls[0]));
			panel.Controls.Add(cBoxType);
			panel.Controls.Add(lblType);
			return panel;
		}

		protected override string[] AlertDisplay {
			get {
				return new string[] { Types[_type], _absoluteAlert.Current.ToString()};
			}
		}

		protected override CandleLength CandlesLength {
			get {
				return new CandleLength(_length, 180);
			}
		}

		protected override string ToLine()
		{
			return $"{_absoluteAlert};{_length};{(int)_source};{_type}";
		}

		protected override void Test(object param)
		{
			//System.IO.StreamWriter sw2 = new System.IO.StreamWriter("rsi.txt");
			//foreach (var c in (List<Candlestick>)param)
			//{
			//	sw2.WriteLine(c.close);
			//}
			//sw2.Close();
			//List<float> r = TA.GetRSI((from c in (List<Candlestick>)param select c.close).ToList(), _length);
			//for (int i = 0; i < r.Count; i++)
			//{
			//}

			switch (_source)
			{
				case Source.open:
					_absoluteAlert.Current = TA.GetRSI((from c in (List<Candlestick>)param select c.open).ToList(), _length).Last();
					break;
				case Source.high:
					_absoluteAlert.Current = TA.GetRSI((from c in (List<Candlestick>)param select c.high).ToList(), _length).Last();
					break;
				case Source.low:
					_absoluteAlert.Current = TA.GetRSI((from c in (List<Candlestick>)param select c.low).ToList(), _length).Last();
					break;
				case Source.close:
					_absoluteAlert.Current = TA.GetRSI((from c in (List<Candlestick>)param select c.close).ToList(), _length).Last();
					break;
			}
			if (_absoluteAlert.Test())
				BaseTest(timeframe, _length);
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
	}
}
