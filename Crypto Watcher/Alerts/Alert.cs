/******************************************************************************
 * CRYPTO WATCHER - cryptocurrency alert system that notifies you when certain 
 * cryptocurrency fulfills your condition.
 * Copyright (c) 2017 Stock84-dev
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
using System.Web.Script.Serialization;
using System.IO;
using APIs;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace Alerts
{
	class Alert
	{
		static string save_path = "Saves/Alerts.txt";
		public static List<AbstractAlert> AlertList { get; set; } = new List<AbstractAlert>();
		public static List<Assets> AssetList { get; set; } = null;
		public static List<Ticker> TickerList { get; set; } = null;
		public static long CurrentCosts { get; set; }
		private static readonly object door = new object();
		
		// saves all alerts
		public static void SaveAlerts()
		{
			StreamWriter sw = new StreamWriter(save_path);
			foreach (var alert in AlertList)
			{
				sw.WriteLine(alert.ToLine());
			}
			sw.Close();
		}
		// loads all alerts
		public static void LoadAlerts()
		{
			CryptowatchAPI.AllowanceChanged += CryptowatchAPI_AllowanceChanged;
			if (!File.Exists(save_path))
				return;
			StreamReader sr = new StreamReader(save_path);
			string line = string.Empty;
			while ((line = sr.ReadLine()) != null)
			{
				AlertList.Add(AbstractAlert.FromLine(line));
			}
		}

		private static void CryptowatchAPI_AllowanceChanged(object sender, AllowanceEventArgs e)
		{
			CurrentCosts += e.allowance.cost;
		}

		// gets average cost (allowance API) for all alerts
		public static long GetMaxCosts()
		{
			long maxCosts = 0;

			foreach (var alert in AlertList)
			{
				maxCosts += alert.GetMaxCosts();
			}

			return maxCosts;
		}

		public async static Task UpdateTickerList()
		{
			Console.WriteLine($"Updating ticker list: {DateTime.Now}");
			List<Ticker> tickers = await CoinMarketCapAPI.GetTickers(-1, -1);
			while ((tickers = await CoinMarketCapAPI.GetTickers(-1, -1)) == null)
				;
			lock (door)
			{
				TickerList = tickers;
			}
			Console.WriteLine($"Done updating ticker list: {DateTime.Now}");
		}

		public static Task IsTickerListDone()
		{
			return Task.Factory.StartNew(() =>
			{
				while (true)
				{
					lock (door)
					{
						if (TickerList != null)
							break;
					}
					Thread.Sleep(10);
				}
			});
		}

	}

	class AbsoluteAlert : AbstractAlert
	{
		public enum Type { higher, higherOrEqual, lower, lowerOrEqual};
		float trigger_price, current_price = -1;
		Type type;
		// market route to convert price to another currency
		string end_market_route;

		public AbsoluteAlert(AlertData alertData, Notification notification, float trigger_price, Type type, string end_market_route) : base(alertData, notification)
		{
			this.trigger_price = trigger_price;
			this.type = type;
			this.end_market_route = end_market_route;
		}

		// creates alert from line (the one that is in file)
		public AbsoluteAlert(string data)
		{
			BaseFromLine(ref data);
			int i = data.IndexOf(';');
			type = (Type)int.Parse(data.Substring(0, i));
			data = data.Substring(i + 1);
			trigger_price = float.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
			end_market_route = data.Substring(0);
		}

		public override string ToLine()
		{
			return "1;" + BaseToLine() + 
				((int)type).ToString() + ";" +
				trigger_price.ToString() + ";" +
				end_market_route;
		}

		// downloads price and compares, if condition is fulfilled returns true
		public async override Task<bool> Test()
		{
			if (end_market_route != "")
			{
				float middle_price = await CryptowatchAPI.GetPrice(alertData.MarketRoute + "/price");
				current_price = middle_price * await CryptowatchAPI.GetPrice(end_market_route + "/price");
			}
			else if (alertData.MarketRoute != "")
				current_price = await CryptowatchAPI.GetPrice(alertData.MarketRoute + "/price");
			else
			{
				foreach (var ticker in Alert.TickerList)
				{
					if(ticker.symbol == alertData.BaseSymbol)
					{
						if (alertData.QuoteSymbol == "BTC")
							current_price = float.Parse(ticker.price_btc, CultureInfo.InvariantCulture);
						else if (alertData.QuoteSymbol == "USD")
							current_price =  float.Parse(ticker.price_usd, CultureInfo.InvariantCulture);
						break;
					}
				}
			}

			switch (type)
			{
				case Type.higher:
					if (current_price > trigger_price)
						return true;
					break;
				case Type.higherOrEqual:
					if (current_price >= trigger_price)
						return true;
					break;
				case Type.lower:
					if (current_price < trigger_price)
						return true;
					break;
				case Type.lowerOrEqual:
					if (current_price <= trigger_price)
						return true;
					break;
			}
			return false;
		}

		// creates a string that is displayed in form
		public override string[] ToRow()
		{
			return new string[]{ alertData.BaseName,
				ConditionToString(),
				trigger_price.ToString(),
				alertData.QuoteSymbol,
				alertData.ExchangeName
			};
		}

		public override string Message {
			get {
				return $"{alertData.BaseName} [{alertData.BaseSymbol}] with the current price of {current_price.ToString()} {alertData.QuoteSymbol} is {ConditionToString().ToLower()} {trigger_price.ToString()} {alertData.QuoteSymbol}.";
			}
		}

		// sums all average costs (allowance API) of API functions that this alert is using 
		public override long GetMaxCosts()
		{
			return CryptowatchAPI.MaximumCost.GetPrice;
		}

		string ConditionToString()
		{
			switch (type)
			{
				case Type.higher:
					return "Higher than";
				case Type.higherOrEqual:
					return "Higher or equal than";
				case Type.lower:
					return "Lower than";
				case Type.lowerOrEqual:
					return "Lower or equal than";
			}
			return "NO CONDITION";
		}
	}
}

	 