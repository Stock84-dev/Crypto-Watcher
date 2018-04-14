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

namespace CryptoWatcher.Alert
{
	class PriceAlert : AbstractAlert
	{
		private AbsoluteAlert _absoluteAlert;

		public PriceAlert() { _absoluteAlert = new AbsoluteAlert(); }

		// creates alert from line (the one that is in file)
		protected override void Init(string data)
		{
			_absoluteAlert = new AbsoluteAlert(ref data);
		}

		public override string Name { get { return "Price"; } }
		public override string Message {
			get {
				string output = $"{AlertData.BaseName} [{AlertData.ExchangeName}:{AlertData.BaseSymbol}{AlertData.QuoteSymbol}] with the current price of {_absoluteAlert.Current} {AlertData.QuoteSymbol} is {_absoluteAlert.ConditionToString().ToLower()}";
				if (_absoluteAlert.Type != TriggerType.crossing)
					output += " than ";
				output += $"{_absoluteAlert.Trigger} {AlertData.QuoteSymbol}.";
				return output;
			}
		}

		public override MetroPanel GetUI()
		{
			return BaseGetOptions(true);
		}

		public override MetroPanel GetFilledUI()
		{
			MetroPanel panel = BaseGetOptions(true);

			panel.Controls["txtValue"].Text = _absoluteAlert.Trigger.ToString();
			((MetroComboBox)(panel.Controls["cBoxCondition"])).SelectedItem = AbsoluteAlert.ConditionToString(_absoluteAlert.Type);

			return panel;
		}

		protected override bool Create(MetroPanel panel)
		{
			_absoluteAlert = new AbsoluteAlert(); // when editing alert last price is saved from previous one
			if (!float.TryParse(panel.Controls["txtValue"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _absoluteAlert.Trigger))
			{
				System.Windows.Forms.MessageBox.Show("Something is wrong in value section.");
				return false;
			}
			_absoluteAlert.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
			return true;
		}

		protected override string[] AlertDisplay {
			get {
				return new string[] { _absoluteAlert.ConditionToString() + " " + _absoluteAlert.Trigger.ToString(), "N/A" };
			}
		}

		protected override CandleLength CandlesLength {
			get {
				return new CandleLength(0);
			}
		}

		protected override string ToLine()
		{
			return $"{_absoluteAlert}";
		}

		protected override void Test(object param)
		{
			_absoluteAlert.Current = Convert.ToSingle(param);

			if (_absoluteAlert.Test())
				BaseTest();
		}
	}
}
