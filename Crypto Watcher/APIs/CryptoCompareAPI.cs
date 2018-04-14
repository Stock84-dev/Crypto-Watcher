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
using System.Diagnostics;
using Newtonsoft.Json;

// for hour 
//{"Message":"Total Rate limit hour stats","CallsMade":{"Histo":0,"Price":0,"News":0},"CallsLeft":{"Histo":8000,"Price":150000,"News":3000}}
// for second
//{"Message":"Total Rate limit second stats","CallsMade":{"Histo":0,"Price":0,"News":0},"CallsLeft":{"Histo":15,"Price":50,"News":5}}
// for debuging websocked subs: https://streamer.cryptocompare.com/status


// TODO: add max limit of alerts only if you are using get function not websocket
// TODO: consider putting max length when user is creating alert (max converted length = 2000)
// TODO: Subscription isn't removing from websocket, couldn't find solution, try reconnecting every time someone unsubscribes
// TODO: add rate limits for minute
// TODO: use database for storing alerts, problem is how to dynamically change columns, use different table for each alert type?
//

// BUG: sometimes causes TOO_MANY_CONNECTIONS_MAX_2_PER_SECOND_AND_50_PER_MINUTE401~TOO_MANY_CONNECTIONS_MAX_2_PER_SECOND_AND_50_PER_MINUTE

namespace CryptoWatcher.APIs
{
	public class CryptoCompareAPI : AbstractAPI
	{
		public const string NAME = "CryptoCompare";
		
		private const string _STREAMERURL = "wss://streamer.cryptocompare.com";
		private const string _MAIN_URL = "https://www.cryptocompare.com/";
		private const string _MIN_URL = "https://min-api.cryptocompare.com/";
		private static readonly object _clsDoor = new object();
		private static readonly object _clhDoor = new object();
		private static readonly object _subsDoor = new object();
		// TODO: streamer unpacking: https://github.com/cryptoqween/cryptoqween.github.io/blob/master/streamer/ccc-streamer-utilities.js#L353
		private static readonly string[] _maskPrice = { "f01", "38f01", "20f89", "8f01", "fe9", "10f01", "30f01", "f89", "f81", "20f01", "fc1", "18f01", "28f01", "40fc0", "40fc9", "10fc1", "38c01", "18fe9", "60fe1", "22fc9", "2f01", "1af01", "3af01", "30fc9", "cc9", "28f81", "30f89", "8f89", "38fc1", "20fc1", "10f89", "2f89", "2afc9", "68fc9", "38f89", "cc1", "28f89", "10fc9", "50fc9", "78fc9", "18fc9", "8fc9", "38f81", "20fc9", "18f81", "10f81", "18f89" };
		private static readonly string[] _maskPriceAmount = { "fe01", "40fe1", "58fe1", "fe1", "8fe1", "38fe1", "10fe1", "78fe1", "50fe1", "70fe1", "28fe1", "60fe1", "48fe1", "52fe1", "2fe1", "5afe1", "68fe1", "30fe1", "18fe1", "20fe1", "ce1", "7afe1", "afe1" };
		// timestamp is between price and amount
		private static readonly string[] _maskPriceTimestampAmount = { "7ffe9", "fc9", "10fe9", "fe9", "40fe9", "8fe9", "20fe9", "70fe9", "50fe9", "60fe9", "30fe9", "58fe9", "38fe9", "78fe9", "28fe9", "68fe9", "48fe9", "44fe9", "4fe9", "18fe9", "12fe9", "3afe9", "7afe9", "52fe9", "2fe9", "1afe9", "38ce9", "ce9", "2afe9", "8ce9", "8ce8" };	
		// no price but timestamp and amount
		private static readonly string[] _maskTimestampAmount = { "fe8", "40fe8", "8fe8", "20fe8", "10fe8", "50fe8", "28fe8", "60fe8", "38fe8", "30fe8", "48fe8", "70fe8", "18fe8", "78fe8", "58fe8", "ce8", "42fe8", "2fe8" };
		// not useful //NOTE: there are some strings where price is close to the end of a string
		private static readonly string[] _maskNotUseful = { "f00", "f88", "20f00", "f80", "10f00", "c00", "fc8", "8f00", "8f88", "fc0", "30f00", "10f88", "2f00", "68fe8", "8c00", "c88", "20f88", "38f00", "28f00", "c80", "18f88", "40fc8", "28f80", "20f80", "48fc8", "18f00", "10f80", "28f88" };
		// amount
		private static readonly string[] _maskAmount = { "fe0", "40fe0", "68fe0", "60fe0", "10fe0", "38fe0", "20fe0", "8fe0", "50fe0", "70fe0", "ce0", "48fe0", "30fe0" };
		private static RateLimit _callsLeftHour = new RateLimit(8000, 150000, 3000);
		private static RateLimit _callsLeftSecond = new RateLimit(15, 50, 5);
		private static RateLimit _rateLimitHour = new RateLimit(8000, 150000, 3000);
		private static RateLimit _rateLimitSecond = new RateLimit(15, 50, 5);
		private Socket _socket = IO.Socket("wss://streamer.cryptocompare.com");
		private System.Windows.Forms.Timer _timerSecond = new System.Windows.Forms.Timer();
		private System.Windows.Forms.Timer _timerHour = new System.Windows.Forms.Timer();
		private System.Windows.Forms.Timer _startingTimer = new System.Windows.Forms.Timer();
		private DateTime _nextUpdateSecond;
		private DateTime _nextUpdateHour;
		private Dictionary<string, Data> _subs = new Dictionary<string, Data>();

		public CryptoCompareAPI()
		{
			
			Console.WriteLine("Establishing connection...");
			_socket.Connect();
			//_socket.Open();
			_socket.On("m", data => OnMessage(data));
			_socket.On(Socket.EVENT_CONNECT, () =>
			{
				Debug.WriteLine("Socket connected.");
			});
			UpdateLimitsAsync();
			DateTime currentTime = DateTime.Now;
			_nextUpdateHour = currentTime + new TimeSpan(1, 0, 0);
			_nextUpdateSecond = currentTime + new TimeSpan(0, 0, 1);
			_startingTimer.Interval = 1;
			_startingTimer.Tick += StartingTimer_Tick;
			_startingTimer.Enabled = true;
		}

		protected enum RateLimitType { Price, History, News }
		// VolumeFrom = BTC, VolumeTo = USD
		/// <summary>
		/// Constants to find data on specific place of a message.
		/// </summary>
		private enum MsgType
		{
			/// <summary>
			/// Price is in first section.
			/// </summary>
			Price,
			/// <summary>
			/// {Price}~{VolumeFrom}~{VolumeTo}...
			/// </summary>
			PriceAmount,
			/// <summary>
			/// {Price}~{Timestamp}~{VolumeFrom}~{VolumeTo}...
			/// </summary>
			PriceTimestampAmount,
			/// <summary>
			/// {Timestamp}~{VolumeFrom}~{VolumeTo}...
			/// </summary>
			TimestampAmount,
			/// <summary>
			/// {VolumeFrom}~{VolumeTo}...
			/// </summary>
			Amount,
			/// <summary>
			/// Other info that I don't need, like marketcap, 24h volumes.
			/// </summary>
			NotUseful
		}

		public async Task<Dictionary<string,List<string>>> GetQuotesAndExchanges(string symbol)
		{
			Dictionary<string, SubByPair> pairSubs = await GetSubsByPair(symbol);
			Dictionary<string, List<string>> output = new Dictionary<string, List<string>>();
			
			foreach (var quote in pairSubs.Keys)
			{
				List<string> exchanges = new List<string>();

				foreach (var exchange in pairSubs[quote].CURRENT)
					exchanges.Add(Utility.GetSubstring(exchange, '~', 1));

				exchanges.Add("Aggregate");

				output.Add(quote, exchanges);
			}
			return output;
		}

		public void PriceSubscribe(string exchange, string baseSymbol, string quoteSymbol, Action<PriceUpdate> action)
		{
			if (exchange == "Aggregate")
				exchange = "CCCAGG";
			string key = $"{exchange};{baseSymbol};{quoteSymbol}";
			Debug.WriteLine($"Price subscribe {key}, thread: {Thread.CurrentThread.ManagedThreadId}");
			lock (_subsDoor)
			{
				if (!_subs.ContainsKey(key))
				{
					priceSubscription.CreateEventForKey(key);
					priceSubscription.EventForKey(key).Add(action);
					_subs.Add(key, new Data());
					WsSubscribe(exchange, baseSymbol, quoteSymbol);
				}
				else
				{
					if (!priceSubscription.ContainsKey(key))
						priceSubscription.CreateEventForKey(key);
					priceSubscription.EventForKey(key).Add(action);
				}
				_subs[key].Subs++;
			}
		}

		public void PriceUnsubscribe(string exchange, string baseSymbol, string quoteSymbol, Action<PriceUpdate> action)
		{
			if (exchange == "Aggregate")
				exchange = "CCCAGG";
			string key = $"{exchange};{baseSymbol};{quoteSymbol}";
			lock (_subsDoor)
			{
				if (!_subs.ContainsKey(key))
					return;
				Debug.WriteLine($"Unsubscribe price{key}, thread: {Thread.CurrentThread.ManagedThreadId}");

				priceSubscription.RemoveEvent(key, action);
				_subs[key].Subs--;
				if (_subs[key].Subs == 0)
				{
					priceSubscription.Remove(key);
					_subs.Remove(key);
					WsUnsubscribe(exchange, baseSymbol, quoteSymbol);
				}
				else if (_subs[key].Subs < 0)
				{
					System.Windows.Forms.MessageBox.Show("ERROR: Cryptocompare, sub count is negative.");
					Debug.WriteLine("ERROR: Cryptocompare, sub count is negative.");
				}
			}
		}
		// TODO: when multiple alerts with same keys are subscribing there could be mixing of candles, either add a way to download only once with longest length or wait for completition of CandleSubscribe maybe dict of keys with tasks of lists of candlesticks?
		/// <param name="minLength">If the length downloaded candles is less than minLength then it returns false.</param>
		public async Task<bool> CandleSubscribe(string exchange, string baseSymbol, string quoteSymbol, Timeframe timeframe, int minLength, Action<PriceUpdate> action, int maxLength = -1, bool autocall = true)
		{
			if (maxLength == -1)
				maxLength = minLength;
			if (exchange == "Aggregate")
				exchange = "CCCAGG";
			string key = $"{exchange};{baseSymbol};{quoteSymbol}";
			Debug.WriteLine($"Candle subscribe {key}, thread: {Thread.CurrentThread.ManagedThreadId}");
			List<Candlestick> candlesticks = null;
			bool downloadCandles = false;
			lock (_subsDoor)
			{
				if (!_subs.ContainsKey(key) || !_subs[key].Candles.ContainsKey(timeframe) || _subs[key].Candles[timeframe].Count < maxLength)
					downloadCandles = true;
			}
			if (downloadCandles)
				candlesticks = (await GetCandlesticks(baseSymbol, quoteSymbol, timeframe, maxLength, exchange)).ToList();
			// there are less candles than required
			if (downloadCandles && candlesticks.Count < minLength)
				return false;
			lock (_subsDoor)
			{
				// We don't have key in dictionary.
				if (!_subs.ContainsKey(key))
				{
					Debug.WriteLine($"!_subs.ContainsKey(key) = {!_subs.ContainsKey(key)}");
					candleSubscription.CreateEventForKey(key + ";" + timeframe);
					candleSubscription.EventForKey(key + ";" + timeframe).Add(action);
					_subs.Add(key, new Data(timeframe, candlesticks));
					WsSubscribe(exchange, baseSymbol, quoteSymbol);
				}// We have key in dictionary, but different timeframe.
				else if (!candleSubscription.ContainsKey(key + ";" + timeframe))
				{
					candleSubscription.CreateEventForKey(key + ";" + timeframe);
					candleSubscription.EventForKey(key + ";" + timeframe).Add(action);
					_subs[key].Candles.Add(timeframe, candlesticks);
				}
				else
				{
					candleSubscription.EventForKey(key + ";" + timeframe).Add(action);

					if (_subs[key].Candles[timeframe].Count < maxLength)
						_subs[key].Candles[timeframe] = candlesticks;
				}
				_subs[key].Subs++;
			}
			if(autocall)
				action(new PriceUpdate() { Candlesticks = _subs[key].Candles[timeframe] });
			return true;
		}

		public void CandleUnsubscribe(string exchange, string baseSymbol, string quoteSymbol, Timeframe timeframe, Action<PriceUpdate> action)
		{
			if (exchange == "Aggregate")
				exchange = "CCCAGG";
			string key = $"{exchange};{baseSymbol};{quoteSymbol}";

			lock (_subsDoor)
			{
				Debug.WriteLine($"Unsubscribe candle {key}, thread: {Thread.CurrentThread.ManagedThreadId}");
				candleSubscription.RemoveEvent(key + ";" + timeframe, action);
				if (candleSubscription.Count(key + ";" + timeframe) == 0)
				{
					candleSubscription.Remove(key + ";" + timeframe);
					_subs[key].Candles.Remove(timeframe);
				}

				_subs[key].Subs--;
				if (_subs[key].Subs == 0)
				{
					_subs.Remove(key);
					WsUnsubscribe(exchange, baseSymbol, quoteSymbol);
				}
				else if (_subs[key].Subs < 0)
				{
					System.Windows.Forms.MessageBox.Show("ERROR: Cryptocompare, sub count is negative.");
					Debug.WriteLine("ERROR: Cryptocompare, sub count is negative.");
				}
			}
		}

		/// <summary>
		/// Subscribes to specific pair for websocket.
		/// </summary>
		private void WsSubscribe(string exchange, string baseSymbol, string quoteSymbol)
		{
			string sub;
			if (exchange == "CCCAGG")
				sub = $"5~CCCAGG~{baseSymbol}~{quoteSymbol}";
			else sub = $"2~{exchange}~{baseSymbol}~{quoteSymbol}";

			var obj = JObject.FromObject(new { subs = new[] { sub } });
			Debug.WriteLine("Subscribing: " + sub);
			_socket.Emit("SubAdd", obj);
		}

		/// <summary>
		/// Removes price from tracking (prices dictionary).
		/// </summary>
		private void WsUnsubscribe(string exchange, string baseSymbol, string quoteSymbol)
		{
			string sub;
			if (exchange == "CCCAGG")
				sub = $"5~CCCAGG~{baseSymbol}~{quoteSymbol}";
			else sub = $"2~{exchange}~{baseSymbol}~{quoteSymbol}";

			var obj = JObject.FromObject(new { subs = new[] { sub } });
			_socket.Emit("SubRemove", obj);
			Debug.WriteLine("Removing: " + sub);
		}
		// trying to get rid of too many connections error by reconnecting after 2 sec
		private void Reconnect()
		{
			_socket.Disconnect();
			Task.Delay(2000);
			_socket.Connect();
			Task.Delay(100);
			foreach (var key in _subs.Keys)
			{
				string exchange = Utility.GetSubstring(key, '~', 0);
				string baseSymbol = Utility.GetSubstring(key, '~', 1);
				string quoteSymbol = Utility.GetSubstring(key, '~', 2);
				WsSubscribe(exchange, baseSymbol, quoteSymbol);
			}
		}

		private MsgType GetMsgType(string msg)
		{
			string type = msg.Substring(msg.LastIndexOf('~') + 1);

			if (_maskPriceTimestampAmount.Contains(type)) return MsgType.PriceTimestampAmount;
			if (_maskTimestampAmount.Contains(type)) return MsgType.TimestampAmount;
			if (_maskPriceAmount.Contains(type)) return MsgType.PriceAmount;
			if (_maskNotUseful.Contains(type)) return MsgType.NotUseful;
			if (_maskAmount.Contains(type)) return MsgType.Amount;
			if (_maskPrice.Contains(type)) return MsgType.Price;

			if (!File.Exists("Saves/socketlog.txt"))
				File.Create("Saves/socketlog.txt");

			using (StreamWriter w = File.AppendText("Saves/socketlog.txt"))
			{
				w.WriteLine(msg);
				Console.WriteLine(msg);
				w.Close();
			}

			return MsgType.NotUseful;
		}

		private float GetPrice(string msg, MsgType msgType)
		{
			msg = Utility.GetSubstring(msg, '~', 5, false);
			float volumeFrom, volumeTo;
			switch (msgType)
			{
				case MsgType.Price: return float.Parse(Utility.GetSubstring(msg, '~', 0));
				case MsgType.PriceAmount: return float.Parse(Utility.GetSubstring(msg, '~', 0));
				case MsgType.PriceTimestampAmount: return float.Parse(Utility.GetSubstring(msg, '~', 0));
				case MsgType.TimestampAmount:
					volumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 1));
					volumeTo = float.Parse(Utility.GetSubstring(msg, '~', 2));
					return volumeTo / volumeFrom;
				case MsgType.Amount:
					volumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 0));
					volumeTo = float.Parse(Utility.GetSubstring(msg, '~', 1));
					return volumeTo / volumeFrom;
				default:
					return 0;
			}
		}

		// puts thread to sleep if limit is exceeded and revives it when limit renews
		private async Task Limit(RateLimitType type)
		{
			await Task.Factory.StartNew(() =>
			{
				if (RateLimitType.Price == type)
				{
					lock (_clsDoor)
					{
						if (_callsLeftSecond.Price == 0)
							Thread.Sleep(_nextUpdateSecond - DateTime.Now);
						_callsLeftSecond.Price--;
					}
					lock (_clhDoor)
					{
						if (_callsLeftHour.Price == 0)
							Thread.Sleep(_nextUpdateHour - DateTime.Now);
						_callsLeftHour.Price--;
					}
				}
				else if (RateLimitType.History == type)
				{
					lock (_clsDoor)
					{
						if (_callsLeftSecond.Histo == 0)
							Thread.Sleep(_nextUpdateSecond - DateTime.Now);
						_callsLeftSecond.Histo--;
					}
					lock (_clhDoor)
					{
						if (_callsLeftHour.Histo == 0)
							Thread.Sleep(_nextUpdateHour - DateTime.Now);
						_callsLeftHour.Histo--;
					}
				}
				else
				{
					lock (_clsDoor)
					{
						if (_callsLeftSecond.News == 0)
							Thread.Sleep(_nextUpdateSecond - DateTime.Now);
						_callsLeftSecond.News--;
					}
					lock (_clhDoor)
					{
						if (_callsLeftHour.News == 0)
							Thread.Sleep(_nextUpdateHour - DateTime.Now);
						_callsLeftHour.News--;
					}
				}
			});
		}

		private Trade GetTrade(string msg, MsgType msgType)
		{
			msg = Utility.GetSubstring(msg, '~', 5, false);
			Trade trade = new Trade();
			switch (msgType)
			{
				case MsgType.PriceAmount:
					trade.Price = float.Parse(Utility.GetSubstring(msg, '~', 0));
					trade.VolumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 1));
					trade.VolumeTo = float.Parse(Utility.GetSubstring(msg, '~', 2));
					break;
				case MsgType.PriceTimestampAmount:
					trade.Price = float.Parse(Utility.GetSubstring(msg, '~', 0));
					trade.VolumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 2));
					trade.VolumeTo = float.Parse(Utility.GetSubstring(msg, '~', 3));
					break;
				case MsgType.TimestampAmount:
					trade.VolumeFrom = float.Parse(Utility.GetSubstring(msg, '~', 1));
					trade.VolumeTo = float.Parse(Utility.GetSubstring(msg, '~', 2));
					trade.Price = trade.VolumeTo / trade.VolumeFrom;
					break;
				case MsgType.Amount:
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
		// gets called when websocket recieves message on other thread
		private void OnMessage(object message)
		{
			if ((string)message == "3~LOADCOMPLETE")
			{
				return;
			}
			if ((string)message == "401~TOO_MANY_CONNECTIONS_MAX_2_PER_SECOND_AND_50_PER_MINUTE")
			{
				Reconnect();
				Debug.Indent();
				Debug.WriteLine("Message obstructed: " + message);
				Debug.Unindent();
				return;
			}
			
			Console.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId + ": " + (string)message);
			MsgType msgType = GetMsgType((string)message);
			string exchange = Utility.GetSubstring((string)message, '~', 1);
			string baseSymbol = Utility.GetSubstring((string)message, '~', 2);
			string quoteSymbol = Utility.GetSubstring((string)message, '~', 3);

			float price = GetPrice((string)message, msgType);
			if (msgType != MsgType.NotUseful)
			{
				if (priceSubscription.ContainsKey($"{exchange};{baseSymbol};{quoteSymbol}"))
					priceSubscription.OnEventForKey($"{exchange};{baseSymbol};{quoteSymbol}", new PriceUpdate() { Price = price });
			}

			Trade trade = GetTrade((string)message, msgType);
			if (msgType != MsgType.NotUseful && msgType != MsgType.Price)//38ce9
				ManageCandles(baseSymbol, quoteSymbol, exchange, trade);
		}

		// creates candles from websocket update messages
		private void ManageCandles(string baseSymbol, string quoteSymbol, string exchange, Trade trade)
		{
			// gets keys for specific base and quote symbol
			string key = $"{exchange};{baseSymbol};{quoteSymbol}";
			lock (_subsDoor)
			{
				Debug.WriteLine($"Managing candles: START, thread: {Thread.CurrentThread.ManagedThreadId}");
				if (!_subs.ContainsKey(key))
					return;
				var keys = _subs[key].Candles.Keys;
				int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
				// changing candle for each timeframe
				foreach (var k in keys.ToList())
				{
					if (k == Timeframe.NONE)
						continue;
					int range = _subs[key].Candles[k][1].openTime - _subs[key].Candles[k][0].openTime;
					if (_subs[key].Candles[k].Last().openTime + range <= unixTimestamp)
					{
						_subs[key].Candles[k].Add(new Candlestick(_subs[key].Candles[k].Last().openTime + range, trade.Price, trade.Price, trade.Price, trade.Price, trade.VolumeTo));
						_subs[key].Candles[k].RemoveAt(0);
					}
					else
					{
						if (_subs[key].Candles[k].Last().high < trade.Price)
							_subs[key].Candles[k].Last().high = trade.Price;
						if (_subs[key].Candles[k].Last().low < trade.Price)
							_subs[key].Candles[k].Last().low = trade.Price;
						_subs[key].Candles[k].Last().close = trade.Price;
						_subs[key].Candles[k].Last().volume += trade.VolumeTo;
					}
					candleSubscription.OnEventForKey($"{exchange};{baseSymbol};{quoteSymbol};{(Timeframe)range}", new PriceUpdate() { Candlesticks = _subs[key].Candles[k] });
				}
				Debug.WriteLine($"Managing candles: END, thread: {Thread.CurrentThread.ManagedThreadId}");
			}
		}

		private async Task<Dictionary<string, Dictionary<string, float>>> GetPrice(List<string> fsyms, List<string> tsyms, string exchange = "CCCAGG")
		{
			string endpoint = _MIN_URL + "data/pricemulti?fsyms=";
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
			await Limit(RateLimitType.Price);
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
			Stopwatch s = new Stopwatch();
			s.Start();
			if (limit > 2000)
				throw new ArgumentException();
			string endpoint = _MIN_URL + "data/";
			int aggregate = 1;

			if ((int)Timeframe.d1 <= (int)timeframe)
			{
				endpoint += "histoday";
				aggregate = (int)timeframe / (int)Timeframe.d1;
			}
			else if ((int)Timeframe.h1 <= (int)timeframe)
			{
				endpoint += "histohour";
				aggregate = (int)timeframe / (int)Timeframe.h1;
			}
			else if ((int)Timeframe.min1 <= (int)timeframe)
			{
				endpoint += "histominute";
				aggregate = (int)timeframe / (int)Timeframe.min1;
			}
			endpoint += $"?fsym={baseSymbol}&tsym={quoteSymbol}";
			if (limit == -1)
				endpoint += $"&allData=true";
			else endpoint += $"&limit={limit}";
			if (aggregate != 1)
				endpoint += $"&aggregate={aggregate}";
			if (exchange != "CCCAGG")
				endpoint += $"&e={exchange}";

			await Limit(RateLimitType.History);
			var candlesticks = DeserializeToArray<Candlestick>((await GetJObject(endpoint))["Data"]);
			// if limit is higher than the number of candles, server returns empty candles to fill array
			if(candlesticks[0].close == 0)
			{
				int i = 1;
				while (candlesticks[i].close == 0) { i++; }
				Candlestick[] output = new Candlestick[candlesticks.Length - i];
				Array.Copy(candlesticks, i, output, 0, candlesticks.Length - i);
				return output;
			}
			s.Stop();
			Debug.WriteLine($"Get candlesticks, length: {candlesticks.Length}, time: {s.ElapsedMilliseconds} ms");
			return candlesticks;
		}

		private async Task<Dictionary<string, SubByPair>> GetSubsByPair(string baseSymbol, string[] quoteSymbols = null)
		{
			Dictionary<string, SubByPair> pairSubs = new Dictionary<string, SubByPair>();
			string endpoint = $"https://min-api.cryptocompare.com/data/subs?fsym={baseSymbol}";
			if (quoteSymbols != null)
			{
				endpoint += "&tsyms=";
				for (int i = 0; i < quoteSymbols.Length; i++)
				{
					endpoint += quoteSymbols[i];
					if (i != quoteSymbols.Length - 1)
						endpoint += ',';
				}
			}
			// TODO: add await lock with different ratelimit type "Strict"
			JObject jObject = await GetJObject(endpoint);
			foreach (var quote in jObject)
			{
				if(quote.Key != "")
					pairSubs.Add(quote.Key, quote.Value.ToObject<SubByPair>());
			}
			return pairSubs;
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
			if (_startingTimer.Interval == 1 && DateTime.Now.Millisecond == 0)
			{
				_timerSecond.Interval = 1000;
				_timerSecond.Tick += TimerSecond_Tick;
				_timerSecond.Enabled = true;
				TimerSecond_Tick(null, null);
				_startingTimer.Interval = 60000;
			}
			else if (_startingTimer.Interval == 60000 && DateTime.Now.Minute == 0)
			{
				_timerHour.Interval = 3600000;
				_timerHour.Tick += TimerHour_Tick;
				_timerHour.Enabled = true;
				_startingTimer.Enabled = false;
				TimerHour_Tick(null, null);
			}
		}

		private void TimerHour_Tick(object sender, EventArgs e)
		{
			lock (_clhDoor)
			{
				_callsLeftHour = _rateLimitHour;
				_nextUpdateHour = DateTime.Now + new TimeSpan(1, 0, 0);
			}
		}

		private void TimerSecond_Tick(object sender, EventArgs e)
		{
			lock (_clsDoor)
			{
				_callsLeftSecond = _rateLimitSecond;
				_nextUpdateSecond = DateTime.Now + new TimeSpan(0, 0, 1);
			}
		}

		public async Task<List<Coin>> GetCoinList()
		{
			JToken jToken = (await GetJObject(_MIN_URL + "data/all/coinlist"))["Data"];
			List<Coin> _coins = new List<Coin>();
			foreach (var jt in jToken.Children())
			{
				_coins.Add(jt.Children().Last().ToObject<Coin>());
			}
			return _coins;
		}

		private async Task<List<Sub>> GetSubs(string id)
		{
			string endpoint = _MAIN_URL + "api/data/coinsnapshotfullbyid/?id=" + id;
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
			_rateLimitSecond = await GetRateLimitsAsync(false);
			_rateLimitHour = await GetRateLimitsAsync(true);
		}

		private async Task<RateLimit> GetRateLimitsAsync(bool hourly)
		{
			string endpoint;
			if (hourly)
				endpoint = "stats/rate/hour/limit";
			else endpoint = "stats/rate/second/limit";
			JObject jObject = await GetJObject(_MIN_URL + endpoint);
			return jObject["CallsLeft"].ToObject<RateLimit>();
		}

		public struct Coin
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

		private struct Trade
		{
			public float VolumeFrom { get; set; }
			public float VolumeTo { get; set; }
			public float Price { get; set; }
		}

		private struct RateLimit
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

		private struct Sub
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

		private struct SubByPair
		{
			public string[] TRADES { get; set; }
			public string[] CURRENT { get; set; }
			public string CURRENTAGG { get; set; }
		}

		private class Data
		{
			public int Subs { get; set; } = 0;
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
	}

	public class PriceUpdate
	{
		public List<Candlestick> Candlesticks { get; set; } = null;
		public float Price { get; set; } = -1;
	}
}
