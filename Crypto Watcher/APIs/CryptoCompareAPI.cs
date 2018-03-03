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

using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoWatcher.Utilities;

// for hour 
//{"Message":"Total Rate limit hour stats","CallsMade":{"Histo":0,"Price":0,"News":0},"CallsLeft":{"Histo":8000,"Price":150000,"News":3000}}
// for second
//{"Message":"Total Rate limit second stats","CallsMade":{"Histo":0,"Price":0,"News":0},"CallsLeft":{"Histo":15,"Price":50,"News":5}}

// TODO: add max limit of alerts only if you are using get function not websocket
// TODO: consider putting max length when user is creating alert (max converted length = 2000)
// TODO: having multiple canlde subscribtions that are the same, deleting one of them will delete all
// TODO: make thread safe calls
namespace CryptoWatcher.APIs
{
	class CryptoCompareAPI : AbstractAPI
	{
		protected enum RateLimitType { price, history, news }
		const string minURL = "https://min-api.cryptocompare.com/";
		const string mainURL = "https://www.cryptocompare.com/";
		public const string Name = "CryptoCompare";
		Socket socket = IO.Socket("wss://streamer.cryptocompare.com");
		static RateLimit callsLeftHour = new RateLimit(8000, 150000, 3000);
		static RateLimit callsLeftSecond = new RateLimit(15, 50, 5);
		static RateLimit rateLimitHour = new RateLimit(8000, 150000, 3000);
		static RateLimit rateLimitSecond = new RateLimit(15, 50, 5);
		System.Windows.Forms.Timer timerSecond = new System.Windows.Forms.Timer();
		System.Windows.Forms.Timer timerHour = new System.Windows.Forms.Timer();
		System.Windows.Forms.Timer startingTimer = new System.Windows.Forms.Timer();
		DateTime nextUpdateSecond;
		DateTime nextUpdateHour;
		private static readonly object clsDoor = new object();
		private static readonly object clhDoor = new object();
		//private Dictionary<string, Dictionary<string, float>> prices = new Dictionary<string, Dictionary<string, float>>();
		//private Dictionary<string, List<Candlestick>> candles = new Dictionary<string, List<Candlestick>>();
		//private Dictionary<string, int> nSubs = new Dictionary<string, int>();
		private Dictionary<string, Data> subs = new Dictionary<string, Data>();


		public override bool SupportsCandles {
			get {
				return true;
			}
		}

		public CryptoCompareAPI()
		{
			Console.WriteLine("Establishing connection...");
			socket.Connect();
			socket.Open();
			socket.On("m", data => OnMessage(data));
			UpdateLimitsAsync();
			DateTime currentTime = DateTime.Now;
			nextUpdateHour = currentTime + new TimeSpan(1, 0, 0);
			nextUpdateSecond = currentTime + new TimeSpan(0, 0, 1);
			startingTimer.Interval = 1;
			startingTimer.Tick += StartingTimer_Tick;
			startingTimer.Enabled = true;
		}

		// puts thread to sleep if limit is exceeded and revives it when limit renews
		private async Task Limit(RateLimitType type)
		{
			await Task.Factory.StartNew(() =>
			{
				if (RateLimitType.price == type)
				{
					lock (clsDoor)
					{
						if (callsLeftSecond.Price == 0)
							Thread.Sleep(nextUpdateSecond - DateTime.Now);
						callsLeftSecond.Price--;
					}
					lock (clhDoor)
					{
						if (callsLeftHour.Price == 0)
							Thread.Sleep(nextUpdateHour - DateTime.Now);
						callsLeftHour.Price--;
					}
				}
				else if (RateLimitType.history == type)
				{
					lock (clsDoor)
					{
						if (callsLeftSecond.Histo == 0)
							Thread.Sleep(nextUpdateSecond - DateTime.Now);
						callsLeftSecond.Histo--;
					}
					lock (clhDoor)
					{
						if (callsLeftHour.Histo == 0)
							Thread.Sleep(nextUpdateHour - DateTime.Now);
						callsLeftHour.Histo--;
					}
				}
				else
				{
					lock (clsDoor)
					{
						if (callsLeftSecond.News == 0)
							Thread.Sleep(nextUpdateSecond - DateTime.Now);
						callsLeftSecond.News--;
					}
					lock (clhDoor)
					{
						if (callsLeftHour.News == 0)
							Thread.Sleep(nextUpdateHour - DateTime.Now);
						callsLeftHour.News--;
					}
				}
			});
		}

		protected override void PriceSubscribe(string baseSymbol, string quoteSymbol, Action<object[]> action)
		{
			string key = $"{Name};{baseSymbol};{quoteSymbol}";
			if (!subs.ContainsKey(key))
			{
				priceSubscription.CreateEventForKey(key);
				priceSubscription.EventForKey(key).Add(action);
				subs.Add(key, new Data());
				WsSubscribe(baseSymbol, quoteSymbol);
			}
			else
			{
				priceSubscription.EventForKey(key).Add(action);
				subs[key].Subs++;
			}

		}
		/// <summary>
		/// Subscribes to specific pair for websocket.
		/// </summary>
		private void WsSubscribe(string baseSymbol, string quoteSymbol)
		{
			var obj = JObject.FromObject(new { subs = new[] { $"5~CCCAGG~{baseSymbol}~{quoteSymbol}" } });
			Console.WriteLine("Subscribing...");
			socket.Emit("SubAdd", obj);
		}

		protected override void PriceUnsubscribe(string baseSymbol, string quoteSymbol, Action<object[]> action)
		{
			string key = $"{Name};{baseSymbol};{quoteSymbol}";

			priceSubscription.RemoveEvent(key, action);
			subs[key].Subs--;
			if (subs[key].Subs == 0)
			{
				priceSubscription.Remove(key);
				subs.Remove(key);
				WsUnsubscribe(baseSymbol, quoteSymbol);
			}
		}
		/// <summary>
		/// Removes price from tracking (prices dictionary).
		/// </summary>
		private void WsUnsubscribe(string baseSymbol, string quoteSymbol)
		{
			var obj = JObject.FromObject(new { subs = new[] { $"5~CCCAGG~{baseSymbol}~{quoteSymbol}" } });
			Console.WriteLine("Removing...");
			socket.Emit("SubRemove", obj);
		}
		// TODO: update candle subscription code

		protected async override void CandleSubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, int length, Action<object[]> action)
		{
			string key = $"{Name};{baseSymbol};{quoteSymbol}";
			if (!subs.ContainsKey(key))
			{
				candleSubscription.CreateEventForKey(key + ";" + timeframe);
				candleSubscription.EventForKey(key + ";" + timeframe).Add(action);
				subs.Add(key, new Data(timeframe, (await GetCandlesticks(baseSymbol, quoteSymbol, timeframe, length)).ToList()));
				WsSubscribe(baseSymbol, quoteSymbol);
			}
			else if (!candleSubscription.ContainsKey(key + ";" + timeframe))
			{
				candleSubscription.CreateEventForKey(key + ";" + timeframe);
				candleSubscription.EventForKey(key + ";" + timeframe).Add(action);
				subs[key].Candles.Add(timeframe, (await GetCandlesticks(baseSymbol, quoteSymbol, timeframe, length)).ToList());
			}
			else
			{
				candleSubscription.EventForKey(key + ";" + timeframe).Add(action);
				subs[key].Subs++;

				if (length > subs[key].Candles[timeframe].Count)
					subs[key].Candles[timeframe] = (await GetCandlesticks(baseSymbol, quoteSymbol, timeframe, length)).ToList();
			}
		}

		protected override void CandleUnsubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, Action<object[]> action)
		{
			string key = $"{Name};{baseSymbol};{quoteSymbol}";

			candleSubscription.RemoveEvent(key + ";" + timeframe, action);
			if(candleSubscription.Count(key + ";" + timeframe) == 0)
				candleSubscription.Remove(key + ";" + timeframe);
			subs[key].Subs--;
			if (subs[key].Subs == 0)
			{
				subs.Remove(key);
				WsUnsubscribe(baseSymbol, quoteSymbol);
			}
		}

		// TODO: streamer unpacking: https://github.com/cryptoqween/cryptoqween.github.io/blob/master/streamer/ccc-streamer-utilities.js#L353
		private string[] wsPrice =  { "fe01", "40fe1" ,"f01", "58fe1", "38f01", "fe1", "20f89", "8f01", "7ffe9", "fe9", "40fe9", "10fe9", "8fe9", "20fe9", "70fe9", "50fe9", "8fe1", "10f01", "30f01", "60fe9", "f89", "f81", "20f01", "fc1", "30fe9" };
		private string[] wsCandle = { "fe01", "40fe1", "58fe1", "fe1", "8fe1" };
		// timestamp is between price and amount
		private string[] wsPTmsC =   { "7ffe9", "fc9", "10fe9", "8fe9", "fe9", "40fe9", "8fe9", "20fe9", "70fe9", "50fe9", "60fe9", "30fe9" };//maybe 10fe9
		// no price but timestamp and amount
		private string[] wsTmsC = { "fe8", "40fe8", "8fe8", "20fe8", "10fe8", "50fe8", "28fe8" };
		// not useful
		private string[] notUseful = { "f00", "f88", "20f00", "f80", "10f00", "c00" };
		// amount
		private string[] amt = { "fe0", "40fe0", "60fe8", "68fe0", "60fe0", "10fe0", "38fe0", "20fe0" };

		// VolumeFrom = BTC, VolumeTo = USD
		/// <summary>
		/// Constants to find data on specific place of a message.
		/// </summary>
		private enum MsgType
		{
			/// <summary>
			/// Price is in first section.
			/// </summary>
			p,
			/// <summary>
			/// {Price}~{VolumeFrom}~{VolumeTo}...
			/// </summary>
			pAmt,
			/// <summary>
			/// {Price}~{Timestamp}~{VolumeFrom}~{VolumeTo}...
			/// </summary>
			pTmsAmt,
			/// <summary>
			/// {Timestamp}~{VolumeFrom}~{VolumeTo}...
			/// </summary>
			tmsAmt,
			/// <summary>
			/// {VolumeFrom}~{VolumeTo}...
			/// </summary>
			amt,
			/// <summary>
			/// Other info that I don't need, like marketcap, 24h volumes.
			/// </summary>
			notUseful
		}

		private MsgType GetMsgType(string msg)
		{
			string type = msg.Substring(msg.LastIndexOf('~') + 1);

			foreach (var s in wsPrice)
			{
				if (s == type)
					return MsgType.p;
			}

			foreach (var s in wsCandle)
			{
				if (s == type)
					return MsgType.pAmt;
			}

			foreach (var s in wsPTmsC)
			{
				if (s == type)
					return MsgType.pTmsAmt;
			}

			foreach (var s in wsTmsC)
			{
				if (s == type)
					return MsgType.tmsAmt;
			}

			foreach (var s in amt)
			{
				if (s == type)
					return MsgType.amt;
			}

			foreach (var s in notUseful)
			{
				if (s == type)
					return MsgType.notUseful;
			}

			if (!File.Exists("socketlog.txt"))
				File.Create("socketlog.txt");

			using (StreamWriter w = File.AppendText("socketlog.txt"))
			{
				w.WriteLine(msg);
				Console.WriteLine(msg);
			}

			return MsgType.notUseful;
		}

		private float GetPrice(string msg, MsgType msgType)
		{
			msg = Utility.GetSubstring(msg, '~', 5, false);
			float volumeFrom, volumeTo;
			switch (msgType)
			{
				case MsgType.p: return float.Parse(Utility.GetSubstring(msg, '~', 0));
				case MsgType.pAmt: return float.Parse(Utility.GetSubstring(msg, '~', 0));
				case MsgType.pTmsAmt: return float.Parse(Utility.GetSubstring(msg, '~', 0));
				case MsgType.tmsAmt:
					volumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 1));
					volumeTo = float.Parse(Utility.GetSubstring(msg, '~', 2));
					return volumeTo / volumeFrom;
				case MsgType.amt:
					volumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 0));
					volumeTo = float.Parse(Utility.GetSubstring(msg, '~', 1));
					return volumeTo / volumeFrom;
				default:
					return 0;
			}
		}

		struct Trade
		{
			public float VolumeFrom { get; set; }
			public float VolumeTo { get; set; }
			public float Price { get; set; }
		}

		private Trade GetTrade(string msg, MsgType msgType)
		{
			msg = Utility.GetSubstring(msg, '~', 5, false);
			Trade trade = new Trade();
			switch (msgType)
			{
				case MsgType.pAmt:
					trade.Price = float.Parse(Utility.GetSubstring(msg, '~', 0));
					trade.VolumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 1));
					trade.VolumeTo = float.Parse(Utility.GetSubstring(msg, '~', 2));
					break;
				case MsgType.pTmsAmt:
					trade.Price = float.Parse(Utility.GetSubstring(msg, '~', 0));
					trade.VolumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 2));
					trade.VolumeTo = float.Parse(Utility.GetSubstring(msg, '~', 3));
					break;
				case MsgType.tmsAmt:
					trade.VolumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 1));
					trade.VolumeTo = float.Parse(Utility.GetSubstring(msg, '~', 2));
					trade.Price = trade.VolumeTo / trade.VolumeFrom;
					break;
				case MsgType.amt:
					trade.VolumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 0));
					trade.VolumeTo = float.Parse(Utility.GetSubstring(msg, '~', 1));
					trade.Price = trade.VolumeTo / trade.VolumeFrom;
					break;
				default:
					trade.Price = 0;
					break;
			}
			return trade;
		}

		// TODO: i think you can reduce number of updates because price hasn't changed but there was trade (use only message types that contains price)
		// gets called when websocket recieves message
		private void OnMessage(object message)
		{
			if ((string)message == "3~LOADCOMPLETE")
				return;
			Console.WriteLine((string)message);
			MsgType msgType = GetMsgType((string)message);
			string baseSymbol = Utility.GetSubstring((string)message, '~', 2);
			string quoteSymbol = Utility.GetSubstring((string)message, '~', 3);

			float price = GetPrice((string)message, msgType);
			if (price > 0 && price < 1)
				Console.WriteLine("Error.");
			if (msgType != MsgType.notUseful)
				if(priceSubscription.ContainsKey($"{Name};{baseSymbol};{quoteSymbol}"))
					priceSubscription.OnEventForKey($"{Name};{baseSymbol};{quoteSymbol}", new object[] { price });

			Trade trade = GetTrade((string)message, msgType);
			if (msgType != MsgType.notUseful && msgType != MsgType.p)
				ManageCandles(baseSymbol, quoteSymbol, trade);
		}

		// creates candles from websocket update messages
		private void ManageCandles(string baseSymbol, string quoteSymbol, Trade trade)
		{
			// gets keys for specific base and quote symbol
			string key = $"{Name};{baseSymbol};{quoteSymbol}";
			var keys = subs[key].Candles.Keys;
			int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			foreach (var k in keys)
			{
				if (k == Timeframe.NONE)
					continue;
				int range = subs[key].Candles[k][1].openTime - subs[key].Candles[k][0].openTime;
				if (subs[key].Candles[k].Last().openTime + range <= unixTimestamp)
				{
					subs[key].Candles[k].Add(new Candlestick(subs[key].Candles[k].Last().openTime + range, trade.Price, trade.Price, trade.Price, trade.Price, trade.VolumeTo));
					subs[key].Candles[k].RemoveAt(0);
				}
				else
				{
					if (subs[key].Candles[k].Last().high < trade.Price)
						subs[key].Candles[k].Last().high = trade.Price;
					if (subs[key].Candles[k].Last().low < trade.Price)
						subs[key].Candles[k].Last().low = trade.Price;
					subs[key].Candles[k].Last().close = trade.Price;
					subs[key].Candles[k].Last().volume += trade.VolumeTo;
				}
				candleSubscription.OnEventForKey($"{Name};{baseSymbol};{quoteSymbol};{(Timeframe)range}", new object[] { subs[key].Candles[k] });
			}
		}

		private async Task<Dictionary<string, Dictionary<string, float>>> GetPrice(List<string> fsyms, List<string> tsyms, string exchange = "CCCAGG")
		{
			string endpoint = minURL + "data/pricemulti?fsyms=";
			endpoint += fsyms[0];
			for (int i = 1; i < fsyms.Count; i++)
			{
				endpoint += "," + fsyms[i];
			}
			endpoint += "&tsyms=" + tsyms[0];
			for (int i = 1; i < tsyms.Count; i++)
			{
				endpoint += "," + tsyms[i];
			}
			if (exchange != "CCCAGG")
				endpoint += "&e=" + exchange;
			await Limit(RateLimitType.price);
			Dictionary<string, Dictionary<string, float>> ret = new Dictionary<string, Dictionary<string, float>>();
			JObject jObject = await GetJObject(endpoint);
			for (int j = 0; j < fsyms.Count; j++)
			{
				Dictionary<string, float> _prices = new Dictionary<string, float>();
				for (int i = 0; i < tsyms.Count; i++)
				{
					_prices.Add(tsyms[i], jObject[fsyms[j]][tsyms[i]].ToObject<float>());
				}
				ret.Add(fsyms[j], _prices);
			}

			return ret;
		}

		// TODO: candlestick time is open time
		
		private async Task<Candlestick[]> GetCandlesticks(string baseSymbol, string quoteSymbol, Timeframe timeframe, int limit, string exchange = "CCCAGG")
		{
			if (limit > 2000)
				throw new ArgumentException();
			string endpoint = minURL + "data/";
			int agregate = 1;
			
			if ((int)Timeframe.d1 <= (int)timeframe)
			{
				endpoint += "histoday";
				agregate = (int)timeframe / (int)Timeframe.d1;
			}
			else if ((int)Timeframe.h1 <= (int)timeframe)
			{
				endpoint += "histohour";
				agregate = (int)timeframe / (int)Timeframe.h1;
			}
			else if ((int)Timeframe.min1 <= (int)timeframe)
			{
				endpoint += "histominute";
				agregate = (int)timeframe / (int)Timeframe.min1;
			}
			
			endpoint += $"?fsym={baseSymbol}&tsym={quoteSymbol}&limit={limit}";
			if (agregate != 1)
				endpoint += $"&aggregate={agregate}";
			if (exchange != "CCCAGG")
				endpoint += $"&e={exchange}";

			await Limit(RateLimitType.history);
			return DeserializeToArray<Candlestick>((await GetJObject(endpoint))["Data"]);
			
			//string endpoint = minURL + "data/";
			//bool timeframeFound = true;
			//endpoint += GetHistoTimeframeEndpoint(timeframe);
			//Timeframe tf = Timeframe.min1;
			//// if timeframe isn't minute, hour or daily we have to convert
			//if (endpoint.Last() == '/')
			//{
			//	var enums = Enum.GetValues(typeof(Timeframe)).Cast<Timeframe>().ToList();
			//	int id = enums.FindIndex(x => x == timeframe);
			//	tf = enums.FindLast(x => x < timeframe && (x == Timeframe.min1 || x == Timeframe.h1 || x == Timeframe.d1));
			//	limit *= (int)timeframe / (int)tf;
			//	endpoint += GetHistoTimeframeEndpoint(tf);
			//}
			//if (limit > 2000)
			//{
			//	throw new ArgumentException("Limit is to big, either change limit or change timeframe.");
			//}
			//endpoint += $"?fsym={baseSymbol}&tsym={quoteSymbol}&limit={limit}";
			//if (exchange != "CCCAGG")
			//	endpoint += $"&e={exchange}";

			//await Limit(RateLimitType.history);
			//if (timeframeFound)
			//	return DeserializeToArray<Candlestick>((await GetJObject(endpoint))["Data"]);
			//else
			//{
			//	Candlestick[] tmp = DeserializeToArray<Candlestick>((await GetJObject(endpoint))["Data"]);
			//	return ConvertCandles(tmp, tf, timeframe);
			//}
		}

		private string GetHistoTimeframeEndpoint(Timeframe timeframe)
		{
			string endpoint = "";
			switch (timeframe)
			{
				case Timeframe.min1:
					endpoint += "histominute";
					break;
				case Timeframe.h1:
					endpoint += "histohour";
					break;
				case Timeframe.d1:
					endpoint += "histoday";
					break;
			}
			return endpoint;
		}

		private Candlestick[] ConvertCandles(Candlestick[] data, Timeframe fromTimeframe, Timeframe toTimeframe)
		{
			Candlestick[] ret = new Candlestick[(int)fromTimeframe / (int)toTimeframe * data.Length + 1];
			for (int i = 0, retId = 0; i < data.Length; i += (int)toTimeframe / (int)fromTimeframe)
			{
				if (i + (int)toTimeframe / (int)fromTimeframe > data.Length)
					break;
				ret[retId].high = 0;
				ret[retId].low = float.MaxValue;
				int j = i;
				ret[retId].open = data[j].open;
				for (; j < (int)toTimeframe / (int)fromTimeframe; j++)
				{
					if (ret[retId].high < data[j].high)
						ret[retId].high = data[j].high;
					else if (ret[retId].low > data[j].low)
						ret[retId].low = data[j].low;
				}
				ret[retId].close = data[j].close;
				retId++;
			}
			return ret;
		}

		private void StartingTimer_Tick(object sender, EventArgs e)
		{
			if (startingTimer.Interval == 1 && DateTime.Now.Millisecond == 0)
			{
				timerSecond.Interval = 1000;
				timerSecond.Tick += TimerSecond_Tick;
				timerSecond.Enabled = true;
				TimerSecond_Tick(null, null);
				startingTimer.Interval = 60000;
			}
			else if (startingTimer.Interval == 60000 && DateTime.Now.Minute == 0)
			{
				timerHour.Interval = 3600000;
				timerHour.Tick += TimerHour_Tick;
				timerHour.Enabled = true;
				startingTimer.Enabled = false;
				TimerHour_Tick(null, null);
			}
		}

		private void TimerHour_Tick(object sender, EventArgs e)
		{
			lock (clhDoor)
			{
				callsLeftHour = rateLimitHour;
				nextUpdateHour = DateTime.Now + new TimeSpan(1, 0, 0);
			}
		}

		private void TimerSecond_Tick(object sender, EventArgs e)
		{
			lock (clsDoor)
			{
				callsLeftSecond = rateLimitSecond;
				nextUpdateSecond = DateTime.Now + new TimeSpan(0, 0, 1);
			}
		}

		private async Task<List<Coin>> GetCoinList()
		{
			JToken jToken = (await GetJObject(minURL + "data/all/coinlist"))["Data"];
			List<Coin> _coins = new List<Coin>();
			foreach (var jt in jToken.Children())
			{
				_coins.Add(jt.Children().Last().ToObject<Coin>());
			}
			return _coins;
		}

		private async Task<List<Sub>> GetSubs(string id)
		{
			string endpoint = mainURL + "api/data/coinsnapshotfullbyid/?id=" + id;
			JToken jToken = (await GetJObject(endpoint))["Data"]["Subs"];
			List<Sub> subs = new List<Sub>();
			foreach (var jt in jToken.Children())
			{
				subs.Add(new Sub(jt.ToObject<string>()));
			}
			return subs;
		}

		private async void UpdateLimitsAsync()
		{
			rateLimitSecond = await GetRateLimitsAsync(false);
			rateLimitHour = await GetRateLimitsAsync(true);
		}

		private async Task<RateLimit> GetRateLimitsAsync(bool hourly)
		{
			string endpoint;
			if (hourly)
				endpoint = "stats/rate/hour/limit";
			else endpoint = "stats/rate/second/limit";
			JObject jObject = await GetJObject(minURL + endpoint);
			return jObject["CallsLeft"].ToObject<RateLimit>();
		}

		// TODO: all symbols are valid!!! there is even TRX/SMART, TRX/HRK, pair
		public override List<QuoteSymbol> GetQuoteSymbols(string name, string symbol)
		{
			return new List<QuoteSymbol> { new QuoteSymbol(Name, "USD"), new QuoteSymbol(Name, "BTC"),
				new QuoteSymbol(Name, "ETH"), new QuoteSymbol(Name, "USDT"), new QuoteSymbol(Name, "XMR"), new QuoteSymbol(Name, "QTUM"),
				new QuoteSymbol(Name, "KRW"), new QuoteSymbol(Name, "EUR"),  new QuoteSymbol(Name, "CAD"),  new QuoteSymbol(Name, "GBP"),
				new QuoteSymbol(Name, "JPY") };
		}

		private class Data
		{
			public int Subs { get; set; } = 1;
			public Dictionary<Timeframe, List<Candlestick>> Candles { get; set; }

			public Data()
			{
				Candles = new Dictionary<Timeframe, List<Candlestick>>();
			}

			public Data(Timeframe timeframe, List<Candlestick> candles)
			{
				Candles = new Dictionary<Timeframe, List<Candlestick>>();
				Candles.Add(timeframe, candles);
			}
		}

        public class RateLimit
        {
            public RateLimit(int histo, int price, int news)
            {
                Histo = histo;
                Price = price;
                News = news;
            }

            public int Histo { get; set; }
            public int Price { get; set; }
            public int News { get; set; }
        }

        public class Coin
        {
            public string Id { get; set; }
            public string Url { get; set; }
            public string ImageUrl { get; set; }
            public string Name { get; set; }
            public string Symbol { get; set; }
            public string CoinName { get; set; }
            public string FullName { get; set; }
            public string Algorithm { get; set; }
            public string ProofType { get; set; }
            public string FullyPremined { get; set; }
            public string TotalCoinSupply { get; set; }
            public string PreMinedValue { get; set; }
            public string TotalCoinsFreeFloat { get; set; }
            public string SortOrder { get; set; }
            public bool Sponsored { get; set; }
        }

        public class Sub
        {
            public Sub(string data)
            {
                int i;
                Id = int.Parse(data.Substring(0, (i = data.IndexOf("~"))));
                data = data.Substring(i + 1);
                Exchange = data.Substring(0, (i = data.IndexOf("~")));
                data = data.Substring(i + 1);
                BaseSymbol = data.Substring(0, (i = data.IndexOf("~")));
                data = data.Substring(i + 1);
                QuoteSymbol = data.Substring(0);
            }

            public Sub(int id, string exchange, string baseSymbol, string quoteSymbol)
            {
                Id = id;
                Exchange = exchange;
                BaseSymbol = baseSymbol;
                QuoteSymbol = quoteSymbol;
            }

            public int Id { get; set; }
            public string Exchange { get; set; }
            public string BaseSymbol { get; set; }
            public string QuoteSymbol { get; set; }

            public override string ToString()
            {
                return $"{Id}~{Exchange}~{BaseSymbol}~{QuoteSymbol}";
            }
        }
	}
}
