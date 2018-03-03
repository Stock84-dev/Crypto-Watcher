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

////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using Binance;
////using Binance.Api.WebSocket;
////using System.Threading;
////using Binance.Market;
////using Binance.Api;
////using Microsoft.Extensions.DependencyInjection;

//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Reflection;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using System.Linq;
//using Newtonsoft.Json.Linq;
//using System.Net.WebSockets;

//// https://stackoverflow.com/questions/30523478/connecting-to-websocket-using-c-sharp-i-can-connect-using-javascript-but-c-sha
//namespace CryptoWatcher.APIs
//{
//	class BinanceAPI : AbstractAPI
//	{
//		// TODO: if user has multiple alerts on same market don't haw multiple running sockets for it
//		static BinanceApi api = new BinanceApi();
//		static ServerInfo serverInfo;
//		static Dictionary<string, RunningSocket> runningSockets = new Dictionary<string, RunningSocket>();
//		static Dictionary<string, List<Candlestick>> runningCandles = new Dictionary<string, List<Candlestick>>();
//		WebSocketClient client = new WebSocketClient();
//		TradesWebSocketClient tc;
//		CandlestickWebSocketClient cc;

//		public const string Name = "Binance";
//		private const string apiUrl = "https://api.binance.com";

//		public override bool SupportsCandles {
//			get {
//				return true;
//			}
//		}

//		public BinanceAPI()
//		{
//			Init();
//			symbols = api.GetSymbolsAsync().Result.ToArray();
//			tc = new TradesWebSocketClient(client);
//			cc = new CandlestickWebSocketClient(client);
//		}

//		private async void Init()
//		{
//			serverInfo = await GetServerInfo();
//		}


//		public async void SubscribeCandle(string baseSymbol, string quoteSymbol, int length, Timeframe timeframe)
//		{
//			string key = baseSymbol + quoteSymbol + timeframe + length;

//			if (!runningSockets.ContainsKey(key))
//			{
//				runningSockets.Add(key, new RunningSocket());
//				runningCandles.Add(key, await GetCandlesAsync(baseSymbol, quoteSymbol, timeframe, length));
//			}

//			cc.SubscribeAsync(baseSymbol + quoteSymbol, ConvertToCandlestickInterval(timeframe), evt =>
//			{
//				if (runningCandles[key][runningCandles[key].Count - 1].closeTime < evt.Candlestick.CloseTime)
//				{
//					runningCandles[key].RemoveAt(0);
//					runningCandles[key].Add(ConvertToCandlestick(evt.Candlestick));
//				}
//				else runningCandles[key][runningCandles[key].Count - 1] = ConvertToCandlestick(evt.Candlestick);
//				candleSubscription.OnEventForKey(Name + key, new object[] { runningCandles[key] });

//			}, runningSockets[baseSymbol + quoteSymbol].Cts.Token);
//		}

//		// NOTE: ignoring name
//		public override List<QuoteSymbol> GetQuoteSymbols(string name, string symbol)
//		{
//			List<QuoteSymbol> ret = new List<QuoteSymbol>();
//			foreach (var s in symbols)
//			{
//				if (symbol == s.BaseAsset.Symbol)
//				{
//					ret.Add(new QuoteSymbol(Name, s.QuoteAsset.Symbol));
//				}
//			}
//			return ret;
//		}

//		protected async override void PriceSubscribe(string baseSymbol, string quoteSymbol, Action<object[]> action)
//		{
//			if (!priceSubscription.ContainsKey(Name + baseSymbol + quoteSymbol))
//				priceSubscription.CreateEventForKey(Name + baseSymbol + quoteSymbol);
//			priceSubscription.EventForKey(Name + baseSymbol + quoteSymbol).Add(action);

//			if (!runningSockets.ContainsKey(baseSymbol + quoteSymbol))
//				runningSockets.Add(baseSymbol + quoteSymbol, new RunningSocket());
//			await Task.Run(() => tc.SubscribeAsync(baseSymbol + quoteSymbol, evt =>
//			{
//				Console.WriteLine("Downloading price.");
//				priceSubscription.OnEventForKey(Name + baseSymbol + quoteSymbol, new object[] { (float)evt.Trade.Price });
//			}, runningSockets[baseSymbol + quoteSymbol].Cts.Token));
//		}

//		protected override void PriceUnsubscribe(string baseSymbol, string quoteSymbol)
//		{
//			priceSubscription.Remove(Name + baseSymbol + quoteSymbol);
//			runningSockets[baseSymbol + quoteSymbol].Cts.Cancel();
//			runningSockets.Remove(baseSymbol + quoteSymbol);
//		}

//		protected override void CandleSubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, int length, Action<object[]> action)
//		{
//			string key = baseSymbol + quoteSymbol + timeframe + length;
//			if (!candleSubscription.ContainsKey(Name + key))
//				candleSubscription.CreateEventForKey(Name + key);
//			candleSubscription.EventForKey(Name + key).Add(action);
//			SubscribeCandle(baseSymbol, quoteSymbol, length, timeframe);
//		}

//		protected override void CandleUnsubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, int length)
//		{
//			candleSubscription.Remove(Name + baseSymbol + quoteSymbol + timeframe + length);
//			runningSockets[baseSymbol + quoteSymbol + timeframe + length].Cts.Cancel();
//			runningSockets.Remove(baseSymbol + quoteSymbol + timeframe + length);
//			runningCandles.Remove(baseSymbol + quoteSymbol + timeframe + length);
//		}

//		protected override async Task<List<Candlestick>> GetCandlesAsync(string baseSymbol, string quoteSymbol, Timeframe timeframe, int length)
//		{
//			DateTime currentTime = DateTime.Now;
//			DateTime startTime = currentTime - new TimeSpan(0, 0, length * (int)timeframe);
//			return ConvertToCandlestick((await api.GetCandlesticksAsync(baseSymbol + quoteSymbol, ConvertToCandlestickInterval(timeframe), startTime, currentTime)).ToArray());
//		}

//		private Candlestick ConvertToCandlestick(Binance.Market.Candlestick source)
//		{
//			return new Candlestick(source.CloseTime, (float)source.Open, (float)source.High, (float)source.Low, (float)source.Close, (float)source.Volume);
//		}

//		private List<Candlestick> ConvertToCandlestick(Binance.Market.Candlestick[] source)
//		{
//			List<Candlestick> ret = new List<Candlestick>(source.Length);

//			for (int i = 0; i < source.Length; i++)
//			{
//				ret[i] = new Candlestick(source[i].CloseTime, (float)source[i].Open, (float)source[i].High, (float)source[i].Low, (float)source[i].Close, (float)source[i].Volume);
//			}
//			return ret;
//		}

//		private static CandlestickInterval ConvertToCandlestickInterval(Timeframe timeframe)
//		{
//			switch (timeframe)
//			{
//				case Timeframe.min1:
//					return CandlestickInterval.Minute;
//				case Timeframe.min3:
//					return CandlestickInterval.Minutes_3;
//				case Timeframe.min5:
//					return CandlestickInterval.Minutes_5;
//				case Timeframe.min15:
//					return CandlestickInterval.Minutes_15;
//				case Timeframe.min30:
//					return CandlestickInterval.Minutes_30;
//				case Timeframe.h1:
//					return CandlestickInterval.Hour;
//				case Timeframe.h2:
//					return CandlestickInterval.Hours_2;
//				case Timeframe.h4:
//					return CandlestickInterval.Hours_4;
//				case Timeframe.h6:
//					return CandlestickInterval.Hours_6;
//				case Timeframe.h12:
//					return CandlestickInterval.Hours_12;
//				case Timeframe.d1:
//					return CandlestickInterval.Day;
//				case Timeframe.d3:
//					return CandlestickInterval.Days_3;
//				case Timeframe.w1:
//					return CandlestickInterval.Week;
//			}
//			throw new ArgumentException();
//		}

//		class RunningSocket
//		{
//			public CancellationTokenSource Cts { get; set; }
//			public WebSocket WebSocket { get; set; }

//			public RunningSocket()
//			{
//				WebSocket.
//				Cts = new CancellationTokenSource();
//			}
//		}

//		private async Task<ServerInfo> GetServerInfo()
//		{
//			return await Deserialize<ServerInfo>(apiUrl + "/api/v1/exchangeInfo");
//		}

//		public class ServerInfo
//		{
//			public string timezone { get; set; }
//			public long serverTime { get; set; }
//			public Ratelimit[] rateLimits { get; set; }
//			public object[] exchangeFilters { get; set; }
//			public Symbol[] symbols { get; set; }
//		}

//		public class Ratelimit
//		{
//			public string rateLimitType { get; set; }
//			public string interval { get; set; }
//			public int limit { get; set; }
//		}

//		public class Symbol
//		{
//			public string symbol { get; set; }
//			public string status { get; set; }
//			public string baseAsset { get; set; }
//			public int baseAssetPrecision { get; set; }
//			public string quoteAsset { get; set; }
//			public int quotePrecision { get; set; }
//			public string[] orderTypes { get; set; }
//			public bool icebergAllowed { get; set; }
//			public Filter[] filters { get; set; }
//		}

//		public class Filter
//		{
//			public string filterType { get; set; }
//			public string minPrice { get; set; }
//			public string maxPrice { get; set; }
//			public string tickSize { get; set; }
//			public string minQty { get; set; }
//			public string maxQty { get; set; }
//			public string stepSize { get; set; }
//			public string minNotional { get; set; }
//		}

//	}
//}
