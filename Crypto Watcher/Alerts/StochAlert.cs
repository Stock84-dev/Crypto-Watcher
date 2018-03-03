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
	class StochAlert : AbstractAlert
	{
		AbsoluteAlert absoluteAlert;
		int lengthK = 14, lengthD = 3, lengthSmooth = 1;
		bool isDTrigger = true;
		Timeframe timeframe;
		public override string Name { get { return "Stoch"; } }
		static string[] types = { "Overbought", "Oversold", "Custom" };

		public StochAlert() { absoluteAlert = new AbsoluteAlert(); }

		public StochAlert(string data)
		{
			BaseFromLine(ref data);
			absoluteAlert = new AbsoluteAlert(ref data);

			int i;
			lengthK = int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			lengthD = int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			lengthSmooth = int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			timeframe = (Timeframe)int.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			isDTrigger = bool.Parse(data.Substring(0));

			AbstractAPI.SubscrbeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, lengthK+lengthD, Test);
		}

		public override string Message {
			get {
				return $"{alertData.BaseName} [{alertData.BaseSymbol}{alertData.QuoteSymbol}] with the current Stoch of {absoluteAlert.Current} is {absoluteAlert.ConditionToString().ToLower()} {absoluteAlert.Trigger}.";
			}
		}

		protected override void Test(object[] param)
		{
			TA.Stoch[] stochs = TA.GetStoch(((List<Candlestick>)param[0]).ToArray(), lengthK, lengthD, lengthSmooth);
			//TA.MACD macd = TA.GetMACD(((List<Candlestick>)param[0]).Select(x => x.close).ToList(), 12, 26);
			//System.IO.StreamWriter sw = new System.IO.StreamWriter("macd.txt");
			//for (int i = 0; i < macd.Macd.Count; i++)
			//{
			//	sw.WriteLine($"{macd.Macd[i]};{macd.Signal[i]};{macd.Histogram[i]}");
			//}
			//sw.Close();
			if (isDTrigger)
				absoluteAlert.Current = stochs.Last().stoch_ma;
			else absoluteAlert.Current = stochs.Last().stoch;

			if (absoluteAlert.Test())
				BaseTest(timeframe, lengthK + lengthD);
		}

		public override void Destroy()
		{
			Unsubscribe();
			BaseDestroy();
		}

		public override string ToLine()
		{
			return $"3;{BaseToLine()};{absoluteAlert};{lengthK};{lengthD};{lengthSmooth};{(int)timeframe};{isDTrigger}";
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
			if (panel.Controls["cBoxType"].Text == types[2])
			{
				if (!float.TryParse(panel.Controls["txtValue"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out absoluteAlert.Trigger))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in price section.");
					return false;
				}
				if (absoluteAlert.Trigger > 100 || absoluteAlert.Trigger < 0)
				{
					System.Windows.Forms.MessageBox.Show("Stoch value must be between 0 and 100");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtK"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out lengthK))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in length section.");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtD"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out lengthD))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in length section.");
					return false;
				}
				if (!int.TryParse(panel.Controls["txtSmooth"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out lengthSmooth))
				{
					System.Windows.Forms.MessageBox.Show("Something is wrong in length section.");
					return false;
				}
				if(lengthD < 1 || lengthK < 1 || lengthSmooth < 1)
				{
					System.Windows.Forms.MessageBox.Show("Custom values must be positive.");
					return false;
				}
				absoluteAlert.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
			}
			else if (panel.Controls["cBoxType"].Text == types[0]) // overbought
			{
				absoluteAlert.Trigger = 80;
				absoluteAlert.Type = TriggerType.higherOrEqual;
			}
			else if (panel.Controls["cBoxType"].Text == types[1]) // oversold
			{
				absoluteAlert.Trigger = 20;
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

			isDTrigger = ((MetroFramework.Controls.MetroCheckBox)panel.Controls["chcBoxTriggerType"]).Checked;
			timeframe = AbstractAPI.StringToTimeframe(panel.Controls["cBoxTimeframe"].Text);
			BaseInit(alertData, notification);
			AbstractAPI.SubscrbeCandle(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, timeframe, lengthK+lengthD, Test);
			return true;
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

			var lblK = new MetroFramework.Controls.MetroLabel
			{
				Location = new System.Drawing.Point(270, 49),
				Name = "lblK",
				Size = new System.Drawing.Size(30, 19),
				Text = "%K:",
				Visible = false
			};

			var txtK = new MetroFramework.Controls.MetroTextBox
			{
				Location = new System.Drawing.Point(309, 39),
				Name = "txtK",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "14"
			};

			var txtD = new MetroFramework.Controls.MetroTextBox
			{
				Location = new System.Drawing.Point(309, 78),
				Name = "txtD",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "3"
			};

			var lblD = new MetroFramework.Controls.MetroLabel
			{
				Location = new System.Drawing.Point(268, 88),
				Name = "lblD",
				Size = new System.Drawing.Size(32, 19),
				Text = "%D:",
				Visible = false
			};

			var txtSmooth = new MetroFramework.Controls.MetroTextBox
			{
				Name = "txtSmooth",
				Size = new System.Drawing.Size(124, 29),
				Visible = false,
				Text = "1"
			};

			var lblSmooth = new MetroFramework.Controls.MetroLabel
			{
				Location = new System.Drawing.Point(249, 127),
				Name = "lblSmooth",
				Size = new System.Drawing.Size(58, 19),
				Text = "Smooth:",
				Visible = false
			};

			var chcBoxTriggerType = new MetroFramework.Controls.MetroCheckBox
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
			panel.Controls.Add(lblSmooth);
			panel.Controls.Add(txtSmooth);
			panel.Controls.Add(chcBoxTriggerType);
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
