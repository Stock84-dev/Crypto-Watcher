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
using CryptoWatcher.Utilities;

namespace CryptoWatcher.Alert
{
	class StochAlert : AbstractAlert
	{
		private static string[] _types = { "Overbought", "Oversold", "Custom" };
		private AbsoluteAlert _absoluteAlert;
		private int _lengthK = 14, _lengthD = 3, _lengthSmooth = 1;
		private bool _isDTrigger = true;
		private int _type;
		private float _D, _K;

		public StochAlert() { _absoluteAlert = new AbsoluteAlert(); }

		protected override void Init(string data)
		{
			_absoluteAlert = new AbsoluteAlert(ref data);
			_lengthK = int.Parse(Utility.GetSubstring(data, ';', 0));
			_lengthD = int.Parse(Utility.GetSubstring(data, ';', 1));
			_lengthSmooth = int.Parse(Utility.GetSubstring(data, ';', 2));
			_isDTrigger = Convert.ToBoolean(int.Parse(Utility.GetSubstring(data, ';', 3)));
			_type = int.Parse(Utility.GetSubstring(data, ';', 4));
		}

		public override string IndicatorValueToolTip { get { return "K-fast, D-slow"; } }
		public override string Name { get { return "Stoch"; } }
		public override string Message {
			get {
				return $"{AlertData.BaseName} [{AlertData.ExchangeName}:{AlertData.BaseSymbol}{AlertData.QuoteSymbol}] with the current Stoch of {_absoluteAlert.Current} is {_absoluteAlert.ConditionToString().ToLower()} {_absoluteAlert.Trigger}.";
			}
		}

		public override MetroPanel GetFilledUI()
		{
			MetroPanel panel = GetUI();
			((MetroComboBox)(panel.Controls["cBoxType"])).SelectedItem = _types[_type];
			panel.Controls["txtK"].Text = _lengthK.ToString();
			panel.Controls["txtD"].Text = _lengthD.ToString();
			//panel.Controls["txtSmooth"].Text = _lengthSmooth.ToString();
			panel.Controls["txtValue"].Text = _absoluteAlert.Trigger.ToString();
			((MetroCheckBox)(panel.Controls["chcBoxTriggerType"])).Checked = _isDTrigger;
			((MetroComboBox)(panel.Controls["cBoxTimeframe"])).SelectedItem = AbstractAPI.TimeframeToString(timeframe);

			return panel;
		}

		protected override bool Create(MetroPanel panel)
		{
			_absoluteAlert = new AbsoluteAlert();
			// custom alert
			if (panel.Controls["cBoxType"].Text == _types[2])
			{
				if (!float.TryParse(panel.Controls["txtValue"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _absoluteAlert.Trigger))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in price section.");
					return false;
				}
				if (_absoluteAlert.Trigger > 100 || _absoluteAlert.Trigger < 0)
				{
					System.Windows.Forms.MessageBox.Show("Stoch value must be between 0 and 100");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtK"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _lengthK))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in length section.");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtD"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _lengthD))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in length section.");
					return false;
				}
				//if (!int.TryParse(panel.Controls["txtSmooth"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _lengthSmooth))
				//{
				//	System.Windows.Forms.MessageBox.Show("Something is wrong in length section.");
				//	return false;
				//}
				if (_lengthD < 1 || _lengthK < 1 || _lengthSmooth < 1)
				{
					System.Windows.Forms.MessageBox.Show("Custom values must be positive.");
					return false;
				}
				_absoluteAlert.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
				_type = 2;
				_isDTrigger = ((MetroCheckBox)(panel.Controls["chcBoxTriggerType"])).Checked;
			}
			else if (panel.Controls["cBoxType"].Text == _types[0]) // overbought
			{
				_absoluteAlert.Trigger = 80;
				_absoluteAlert.Type = TriggerType.higherOrEqual;
				_type = 0;
			}
			else if (panel.Controls["cBoxType"].Text == _types[1]) // oversold
			{
				_absoluteAlert.Trigger = 20;
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

			_isDTrigger = ((MetroCheckBox)panel.Controls["chcBoxTriggerType"]).Checked;
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

			var lblK = new MetroLabel
			{
				Location = new System.Drawing.Point(270, 49),
				Name = "lblK",
				Size = new System.Drawing.Size(30, 19),
				Text = "%K:",
				Visible = false
			};

			var txtK = new MetroTextBox
			{
				Location = new System.Drawing.Point(309, 39),
				Name = "txtK",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "14"
			};

			var txtD = new MetroTextBox
			{
				Location = new System.Drawing.Point(309, 78),
				Name = "txtD",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "3"
			};

			var lblD = new MetroLabel
			{
				Location = new System.Drawing.Point(268, 88),
				Name = "lblD",
				Size = new System.Drawing.Size(32, 19),
				Text = "%D:",
				Visible = false
			};

			var txtSmooth = new MetroTextBox
			{
				Location = new System.Drawing.Point(309, 117),
				Name = "txtSmooth",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "1"
			};

			var lblSmooth = new MetroLabel
			{
				Location = new System.Drawing.Point(249, 127),
				Name = "lblSmooth",
				Size = new System.Drawing.Size(58, 19),
				Text = "Smooth:",
				Visible = false
			};

			var chcBoxTriggerType = new MetroCheckBox
			{
				Checked = true,
				Location = new System.Drawing.Point(308, 14),
				Name = "chcBoxTriggerType",
				Size = new System.Drawing.Size(122, 15),
				Text = "Trigger on smooth",
				Visible = false
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
			panel.Controls.Add(lblD);
			panel.Controls.Add(txtD);
			panel.Controls.Add(lblK);
			panel.Controls.Add(txtK);
			//panel.Controls.Add(lblSmooth);
			//panel.Controls.Add(txtSmooth);
			panel.Controls.Add(chcBoxTriggerType);
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
				return new string[] { _types[_type], $"K:{_K}, D:{_D}" };
			}
		}

		protected override CandleLength CandlesLength {
			get {
				return new CandleLength(_lengthK + _lengthD);
			}
		}

		protected override string ToLine()
		{
			return $"{_absoluteAlert};{_lengthK};{_lengthD};{_lengthSmooth};{Convert.ToInt32(_isDTrigger)};{_type}";
		}

		protected override void Test(object param)
		{
			TA.Stoch[] stochs = TA.GetStoch(((List<Candlestick>)param).ToArray(), _lengthK, _lengthD, _lengthSmooth);
			//TA.MACD macd = TA.GetMACD(((List<Candlestick>)param[0]).Select(x => x.close).ToList(), 12, 26);
			//System.IO.StreamWriter sw = new System.IO.StreamWriter("macd.txt");
			//for (int i = 0; i < macd.Macd.Count; i++)
			//{
			//	sw.WriteLine($"{macd.Macd[i]};{macd.Signal[i]};{macd.Histogram[i]}");
			//}
			//sw.Close();
			_D = stochs.Last().stoch_ma;
			_K = stochs.Last().stoch;
			if (_isDTrigger)
				_absoluteAlert.Current = stochs.Last().stoch_ma;
			else _absoluteAlert.Current = stochs.Last().stoch;

			if (_absoluteAlert.Test())
				BaseTest(timeframe, _lengthK + _lengthD);

			// used for debuging, always triggers alert
			//_absoluteAlert.Test();
			//BaseTest(timeframe, _lengthK + _lengthD);
		}
	}
}
