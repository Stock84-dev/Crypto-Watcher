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
using CryptoWatcher.Utilities;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoWatcher.Alert
{
	class MACDAlert : AbstractAlert
	{
		private readonly string[] _types = new string[]{ "Buy signal", "Sell signal", "Positive", "Negative", "Custom" };
		private AbsoluteAlert _absoluteAlert1;
		private AbsoluteAlert _absoluteAlert2;
		private int _slowLength, _fastLength;
		private OnType _onType;
		/// <summary>
		/// Is it a custom type or predefined one.
		/// </summary>
		private int _type;
		private float _MACD, _signal, _histogram;

		public MACDAlert() : base()
		{
			_absoluteAlert1 = new AbsoluteAlert();
			_absoluteAlert2 = new AbsoluteAlert();
		}

		// creates alert from line (the one that is in file)
		protected override void Init(string data)
		{
			_absoluteAlert1 = new AbsoluteAlert(ref data);
			_absoluteAlert2 = new AbsoluteAlert(ref data);
			_slowLength = int.Parse(Utility.GetSubstring(data, ';', 0));
			_fastLength = int.Parse(Utility.GetSubstring(data, ';', 1));
			_onType = (OnType)int.Parse(Utility.GetSubstring(data, ';', 2));
			_type = int.Parse(Utility.GetSubstring(data, ';', 3));
		}

		private enum OnType { MACD, Signal, Histogram, MACD_Signal, MACD_Histogram, Signal_Histogram }

		public override string IndicatorValueToolTip { get { return "H-Histogram, M-MACD (slow), S-Signal (fast)"; } }
		public override string Name { get { return "MACD"; } }
		public override string Message {
			get {
				string output = $"{AlertData.BaseName} [{AlertData.ExchangeName}:{AlertData.BaseSymbol}{AlertData.QuoteSymbol}] {OnTypeToString(_onType)}";
				// two indicator values are crossing
				if ((int)_onType > 2 && _absoluteAlert1.Type == TriggerType.crossing && _absoluteAlert1.Trigger == float.MinValue)
					output += $" are {_absoluteAlert1.ConditionToString().ToLower()}";
				// one indicator value is crossing trigger
				else if (_absoluteAlert1.Type != TriggerType.crossing)
					output += $" is {_absoluteAlert1.ConditionToString().ToLower()} than {_absoluteAlert1.Trigger}";
				else output += $" is {_absoluteAlert1.ConditionToString().ToLower()} {_absoluteAlert1.Trigger}";
				output += $", current value (M:{_MACD}, S:{_signal}, H:{_histogram})";
				return output;
			}
		}

		public override MetroPanel GetFilledUI()
		{
			MetroPanel panel = GetUI();
			((MetroComboBox)(panel.Controls["cBoxType"])).SelectedItem = _types[_type];
			((MetroComboBox)(panel.Controls["cBoxOn"])).SelectedItem = OnTypeToString(_onType);
			panel.Controls["txtSlowLength"].Text = _slowLength.ToString();
			panel.Controls["txtFastLength"].Text = _slowLength.ToString();
			((MetroComboBox)(panel.Controls["cBoxTimeframe"])).SelectedItem = AbstractAPI.TimeframeToString(timeframe);
			return panel;
		}

		public override MetroPanel GetUI()
		{
			var cBoxType = new MetroComboBox
			{
				Location = new System.Drawing.Point(85, 0),
				Name = "cBoxType",
				Size = new System.Drawing.Size(124, 29),
			};
			cBoxType.Items.AddRange(_types);
			cBoxType.SelectedIndex = 0;
			cBoxType.SelectedIndexChanged += CBoxType_SelectedIndexChanged;

			var lblType = new MetroLabel
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
			cBoxOn.Items.AddRange(new object[] { Enum.GetValues(typeof(OnType)) });

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
			panel.Controls.Add(((MetroLabel)controls[1]));
			panel.Controls.Add(((MetroComboBox)controls[0]));
			panel.Controls.Add(cBoxType);
			panel.Controls.Add(lblType);
			return panel;
		}

		protected override bool Create(MetroPanel panel)
		{
			_absoluteAlert1 = new AbsoluteAlert();
			_absoluteAlert2 = new AbsoluteAlert();
			if (panel.Controls["cBoxType"].Text == _types[4])
			{
				if (!int.TryParse(panel.Controls["txtSlowLength"].Text, out _slowLength))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in slow length section.");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtFastLength"].Text, out _fastLength))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in fast length section.");
					return false;
				}
				if (_slowLength < 1 || _fastLength < 1)
				{
					System.Windows.Forms.MessageBox.Show("Length cannot be less than 1.");
					return false;
				}
				_absoluteAlert1.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
				_absoluteAlert2.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
				if (((MetroComboBox)(panel.Controls["cBoxOn"])).SelectedIndex == -1)
				{
					System.Windows.Forms.MessageBox.Show("Please select on what to trigger.");
					return false;
				}
				_onType = StringToOnType(panel.Controls["cBoxOn"].Text);
				if (!float.TryParse(panel.Controls["txtValue"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _absoluteAlert1.Trigger))
				{
					if (_absoluteAlert1.Type != TriggerType.crossing && (int)_onType > 2)
					{
						System.Windows.Forms.MessageBox.Show("Something is wrong in value section.");
						return false;
					}
					_absoluteAlert1.Trigger = float.MinValue;
				}
				_type = 4;
				_absoluteAlert2.Trigger = _absoluteAlert1.Trigger;
			}
			else if (panel.Controls["cBoxType"].Text == "Buy signal")
			{
				_absoluteAlert1.Type = TriggerType.crossing;
				_absoluteAlert1.Trigger = float.MinValue;
				_onType = OnType.MACD_Signal;
				_type = 0;
			}
			else if (panel.Controls["cBoxType"].Text == "Sell signal")
			{
				_absoluteAlert1.Type = TriggerType.crossing;
				_absoluteAlert1.Trigger = float.MinValue;
				_onType = OnType.MACD_Signal;
				_type = 1;
			}
			else if (panel.Controls["cBoxType"].Text == "Positive")
			{
				_absoluteAlert1.Type = TriggerType.higher;
				_absoluteAlert1.Trigger = 0;
				_onType = OnType.MACD;
				_type = 2;
			}
			else if (panel.Controls["cBoxType"].Text == "Negative")
			{
				_absoluteAlert1.Type = TriggerType.lower;
				_absoluteAlert1.Trigger = 0;
				_onType = OnType.MACD;
				_type = 3;
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
			_slowLength = int.Parse(panel.Controls["txtSlowLength"].Text);
			_fastLength = int.Parse(panel.Controls["txtFastLength"].Text);
			timeframe = AbstractAPI.StringToTimeframe(panel.Controls["cBoxTimeframe"].Text);
			return true;
		}

		protected override string[] AlertDisplay {
			get {
				return new string[] { _types[_type], $"M:{_MACD},S:{_signal},H:{_histogram}" };
			}
		}

		protected override CandleLength CandlesLength {
			get {
				return new CandleLength(_fastLength > _slowLength ? _fastLength + 10 : _slowLength + 10);
			}
		}

		protected override string ToLine()
		{
			return $"{_absoluteAlert1};{_absoluteAlert2};{_slowLength};{_fastLength};{(int)_onType};{_type}";
		}

		protected override void Test(object param)
		{
			TA.MACD macd = TA.GetMACD((from c in (List<Candlestick>)param select c.close).ToList(), _fastLength, _slowLength);
			_MACD = macd.Macd.Last();
			_signal = macd.Signal.Last();
			_histogram = macd.Histogram.Last();
			switch (_onType)
			{
				case OnType.MACD:_absoluteAlert1.Current = macd.Macd.Last();
					break;
				case OnType.Signal:_absoluteAlert1.Current = macd.Signal.Last();
					break;
				case OnType.Histogram:_absoluteAlert1.Current = macd.Histogram.Last();
					break;
				case OnType.MACD_Signal:
					_absoluteAlert1.Current = macd.Macd.Last();
					_absoluteAlert2.Current = macd.Signal.Last();
					break;
				case OnType.MACD_Histogram:
					_absoluteAlert1.Current = macd.Macd.Last();
					_absoluteAlert2.Current = macd.Histogram.Last();
					break;
				case OnType.Signal_Histogram:
					_absoluteAlert1.Current = macd.Signal.Last();
					_absoluteAlert2.Current = macd.Histogram.Last();
					break;
			}
			//BaseTest(timeframe, _fastLength > _slowLength ? _fastLength + 10 : _slowLength + 10);//////////////////////////////////////////////
			if ((int)_onType > 2 && _absoluteAlert1.Type == TriggerType.crossing)
			{
				if (_absoluteAlert1.Test(_absoluteAlert2))
					BaseTest(timeframe, _fastLength > _slowLength ? _fastLength + 10 : _slowLength + 10);
			}
			else if ((int)_onType > 2)
			{
				if (_absoluteAlert1.Test() && _absoluteAlert2.Test())
					BaseTest(timeframe, _fastLength > _slowLength ? _fastLength + 10 : _slowLength + 10);
			}
			else
			{
				if (_absoluteAlert1.Test())
					BaseTest(timeframe, _fastLength > _slowLength ? _fastLength + 10 : _slowLength + 10);
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
				case "Signal": return OnType.Signal;
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
	}
}
