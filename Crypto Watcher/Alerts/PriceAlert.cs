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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Alerts
{
	class PriceAlert : AbstractAlert
	{
		AbsoluteAlert absoluteAlert;

		public override string Name { get { return "Price"; } }

		public PriceAlert() { absoluteAlert = new AbsoluteAlert(); }

		// creates alert from line (the one that is in file)
		public PriceAlert(string data)
		{
			BaseFromLine(ref data);
			absoluteAlert = new AbsoluteAlert(ref data);
			AbstractAPI.SubscrbePrice(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, Test);

		}

		public override string ToLine()
		{
			return $"1;{BaseToLine()};{absoluteAlert}";
		}

		protected override void Test(object[] param)
		{
			absoluteAlert.Current = Convert.ToSingle(param[0]);

			if(absoluteAlert.Test())
				BaseTest();
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
				return $"{alertData.BaseName} [{alertData.BaseSymbol}] with the current price of {absoluteAlert.Current} {alertData.QuoteSymbol} is {absoluteAlert.ConditionToString().ToLower()} {absoluteAlert.Trigger} {alertData.QuoteSymbol}.";
			}
		}

		public override MetroFramework.Controls.MetroPanel GetOptions()
		{
			return BaseGetOptions(true);
		}

		public override bool Create(AlertData alertData, Notification notification, MetroFramework.Controls.MetroPanel panel)
		{
			if (!float.TryParse(panel.Controls["txtValue"].Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out absoluteAlert.Trigger))
			{
				System.Windows.Forms.MessageBox.Show("Something is wrong in value section.");
				return false;
			}
			absoluteAlert.Type = AbsoluteAlert.StringToCondition(panel.Controls["cBoxCondition"].Text);
			BaseInit(alertData, notification);
			AbstractAPI.SubscrbePrice(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, Test);
			return true;
		}

		public override void Destroy()
		{
			Unsubscribe();
			BaseDestroy();
		}

		protected override void Unsubscribe()
		{
			AbstractAPI.UnsubscribePrice(alertData.ExchangeName, alertData.BaseSymbol, alertData.QuoteSymbol, Test);
		}
	}
}
