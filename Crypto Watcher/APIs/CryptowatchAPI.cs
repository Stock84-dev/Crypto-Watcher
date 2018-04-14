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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.IO;
//using Newtonsoft.Json; // install Newtonsoft.Json nuGet package
//using Newtonsoft.Json.Linq;
//using System.Threading.Tasks;

//namespace CryptoWatcher.APIs
//{
//	/// <summary>
//	/// Documentation can be found here https://cryptowat.ch/docs/api
//	/// </summary>
//	public class CryptowatchAPI : AbstractAPI
//	{
//		/// <summary>
//		/// Allowance is updated with every call.
//		/// </summary>
//		private static Allowance _allowance = new Allowance();
//		public static Allowance allowance {
//			get {
//				return _allowance;
//			}
//			set {
//				_allowance = value;
//				OnAllowanceChanged(new AllowanceEventArgs(_allowance));
//			}
//		}

//		public override bool SupportsCandles {
//			get {
//				throw new NotImplementedException();
//			}
//		}

//		public delegate void AllowanceChangedEventHandler(object sender, AllowanceEventArgs e);
//		public static event AllowanceChangedEventHandler AllowanceChanged;

//		private static void OnAllowanceChanged(AllowanceEventArgs e)
//		{
//			if (AllowanceChanged != null)
//				AllowanceChanged(typeof(CryptowatchAPI), e);
//		}



//		public static class AverageCost
//		{
//			public const int GetAssets = 715062;
//			public const int GetAsset = 3659875;
//			public const int GetCandlesticks = 8320491;
//			public const int GetExchanges = 83377;
//			public const int GetExchange = 46502;
//			public const int GetMarkets = 823717;
//			public const int GetMarkets1 = 203050; // get exchange specific market
//			public const int GetMarket = 227696;
//			public const int GetOrderBook = 6275745;
//			public const int GetPairs = 4753742;
//			public const int GetPair = 215526;
//			public const int GetPrices = 2379264;
//			public const int GetPrice = 2267609;
//			public const int GetSiteInformation = 15332;
//			public const int GetSummaries = 9139062;
//			public const int GetSummary = 20959771;
//			public const int GetTrades = 6478083;
//		}

//		public static class MaximumCost
//		{
//			public const int GetAssets = 24794198;
//			public const int GetAsset = 295800527;
//			public const int GetCandlesticks = 72606033;
//			public const int GetExchanges = 1668110;
//			public const int GetExchange = 647960;
//			public const int GetMarkets = 6569475;
//			public const int GetMarkets1 = 1964170; // get exchange specific market
//			public const int GetMarket = 14665724;
//			public const int GetOrderBook = 330483278;
//			public const int GetPairs = 130313939;
//			public const int GetPair = 1674564;
//			public const int GetPrices = 35111505;
//			public const int GetPrice = 20712684;
//			public const int GetSiteInformation = 9831044;
//			public const int GetSummaries = 192400297;
//			public const int GetSummary = 338321144;
//			public const int GetTrades = 6478083;//error
//		}

//		///<summary>
//		///You can always request this to query your allowance without any extra result - this request costs very little.
//		/// </summary>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<SiteInformation> GetSiteInformation()
//		{
//			try
//			{
//				return Deserialize<SiteInformation>(await GetJObject("https://api.cryptowat.ch"));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		///Returns all assets(in no particular order)
//		/// </summary>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<List<Assets>> GetAssets()
//		{
//			try
//			{
//				return DeserializeToList<Assets>(await GetJObject("https://api.cryptowat.ch/assets"));
//			}
//			catch
//			{
//				throw;
//			}

//		}

//		/// <summary>
//		///Returns a single asset. Lists all markets which have this asset as a base or quote.
//		/// </summary>
//		/// <param name="route"> Asset specific url, e.g. https://api.cryptowat.ch/assets/btc </param>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<Asset> GetAsset(string route)
//		{
//			try
//			{
//				return Deserialize<Asset>(await GetJObject(route));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns all pairs (in no particular order).
//		/// </summary>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<List<Pairs>> GetPairs()
//		{
//			try
//			{
//				return DeserializeToList<Pairs>(await GetJObject("https://api.cryptowat.ch/pairs"));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		///	Returns a single pair. Lists all markets for this pair.
//		/// </summary>
//		/// <param name="route"> Pair specific url, e.g. https://api.cryptowat.ch/pairs/ethbtc </param>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<Pair> GetPair(string route)
//		{
//			try
//			{
//				return Deserialize<Pair>(await GetJObject(route));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns a list of all supported exchanges.
//		/// </summary>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<List<Exchanges>> GetExchanges()
//		{
//			try
//			{
//				return DeserializeToList<Exchanges>(await GetJObject("https://api.cryptowat.ch/exchanges"));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		///	Returns a single exchange, with associated routes.
//		/// </summary>
//		/// <param name="route"> Exchange specific url, e.g. https://api.cryptowat.ch/exchanges/kraken </param> 
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<Exchange> GetExchange(string route)
//		{
//			try
//			{
//				return Deserialize<Exchange>(await GetJObject(route));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns a list of all supported markets.
//		/// </summary>
//		/// <param name="route">You can also get the supported markets for only a specific exchange. e.g. https://api.cryptowat.ch/markets/kraken </param>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<List<Markets>> GetMarkets(string route = "https://api.cryptowat.ch/markets")
//		{
//			try
//			{
//				return DeserializeToList<Markets>(await GetJObject(route));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		///	Returns a single market, with associated routes.
//		/// </summary>
//		/// <param name="route"> Market specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd </param> 
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<Market> GetMarket(string route)
//		{
//			try
//			{
//				return Deserialize<Market>(await GetJObject(route));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns the current price for all supported markets. Some values may be out of date by a few seconds. 
//		/// <para>
//		/// key = exchangeName:pairName
//		/// </para>
//		/// </summary>
//		/// <returns> dictionsry</returns>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<Dictionary<string, float>> GetPrices()
//		{
//			try
//			{
//				return Deserialize<Dictionary<string, float>>(await GetJObject("https://api.cryptowat.ch/markets/prices"));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		///	Returns a market’s last price.
//		/// </summary>
//		/// <param name="route"> Price specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/price </param> 
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public async static Task<float> GetPrice(string route)
//		{
//			try
//			{
//				return Deserialize<Price>(await GetJObject(route)).price;
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns the market summary for all supported markets. Some values may be out of date by a few seconds.
//		/// <para>
//		/// key = exchangeName:pairName
//		/// </para>
//		/// </summary>
//		/// <returns> dictionsry</returns>
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<Dictionary<string, Summary>> GetSummaries()
//		{
//			try
//			{
//				return Deserialize<Dictionary<string, Summary>>(await GetJObject("https://api.cryptowat.ch/markets/summaries"));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns a market’s last price as well as other stats based on a 24-hour sliding window: High price, Low price, % change, Absolute change, Volume
//		/// </summary>
//		/// <param name="route"> Summary specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/summary </param> 
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<Summary> GetSummary(string route)
//		{
//			try
//			{
//				return Deserialize<Summary>(await GetJObject(route));
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns a market’s most recent trades, incrementing chronologically. Note some exchanges don’t provide IDs for public trades.
//		/// </summary>
//		/// <param name="route"> Trade specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/trades </param> 
//		/// <param name="limit"> Limit amount of trades returned. If 0 returns all.</param> 
//		/// <param name="since"> Only return trades at or after this time. </param> 
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<List<Trade>> GetTrades(string route, int limit = 50, long since = -1)
//		{
//			if (limit != 50 && since != -1)
//			{
//				route += "?limit=" + limit.ToString() + "&since=" + since.ToString();
//			}
//			else if (since != -1)
//			{
//				route += "?since=" + since.ToString();
//			}
//			else if (limit != 50)
//			{
//				route += "?limit=" + limit.ToString();
//			}
//			try
//			{
//				List<float[]> tradeList = DeserializeToList<float[]>(await GetJObject(route));
//				List<Trade> trades = new List<Trade>();
//				foreach (var t in tradeList)
//				{
//					trades.Add(new Trade((int)t[0], (long)t[1], t[2], t[3]));
//				}
//				return trades;
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns a market’s order book.
//		/// </summary>
//		/// <param name="route"> OrderNook specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/orderbook </param> 
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<OrderBook> GetOrderBook(string route)
//		{
//			try
//			{
//				_OrderBook _orderBook = Deserialize<_OrderBook>(await GetJObject(route));
//				OrderBook orderBook = new OrderBook();
//				foreach (var bid in _orderBook.bids)
//				{
//					orderBook.bids.Add(new Order(bid[0], bid[1]));
//				}
//				foreach (var ask in _orderBook.asks)
//				{
//					orderBook.asks.Add(new Order(ask[0], ask[1]));
//				}
//				return orderBook;
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Returns a market’s OHLC candlestick data.
//		/// </summary>
//		/// <param name="route"> Candlestick specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/ohlc </param> 
//		/// <param name="timeFrame"> Candlestick timeframe.</param> 
//		/// <param name="after"> Only return candles opening after this time. If set to -1 max limit is 6000, otherwise it's 500.</param> 
//		/// <param name="before"> Only return candles opening before this time. </param> 
//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		public static async Task<List<Candlestick>> GetCandlesticks(string route, Timeframe timeFrame, long after = -2, long before = 0)
//		{
//			route += "?periods=" + ((int)timeFrame).ToString();
//			if (after != -2)
//			{
//				route += "&after=" + after.ToString();
//			}
//			if (before != 0)
//			{
//				route += "&before=" + before.ToString();
//			}
//			try
//			{
//				_Candlestick _candlestick = Deserialize<_Candlestick>(await GetJObject(route));
//				List<Candlestick> candles = new List<Candlestick>();
//				foreach (var c in _candlestick.allCandlesticks)
//				{
//					candles.Add(new Candlestick((long)c[0], c[1], c[2], c[3], c[4], c[5]));
//				}
//				return candles;
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		private static List<T> DeserializeToList<T>(JObject jObject)
//		{
//			try
//			{
//				// if we get any other error from cryptowatch
//				foreach (var responseType in jObject)
//				{
//					if (responseType.Key == "error")
//						throw new Exception("Cryptowatch error: " + responseType.Value.ToObject<string>());
//				}
//				// get JSON result objects into a list
//				List<JToken> jTokens = jObject["result"].Children().ToList();
//				allowance = jObject["allowance"].ToObject<Allowance>();

//				// serialize JSON results into .NET objects
//				List<T> objects = new List<T>();
//				foreach (JToken jToken in jTokens)
//				{
//					// JToken.ToObject is a helper method that uses JsonSerializer internally
//					objects.Add(jToken.ToObject<T>());
//				}
//				return objects;
//			}
//			catch
//			{
//				throw;
//			}

//		}

//		/// <exception cref="OutOfMemoryException"></exception>
//		/// <exception cref="IOException"></exception>
//		/// <exception cref="JsonReaderException"></exception>
//		/// <exception cref="NotSupportedException"></exception>
//		/// <exception cref="ArgumentNullException"></exception>
//		/// <exception cref="System.Security.SecurityException"></exception>
//		/// <exception cref="UriFormatException"></exception>
//		/// <exception cref="ArgumentException"></exception>
//		/// <exception cref="InvalidOperationException"></exception>
//		/// <exception cref="ProtocolViolationException"></exception>
//		/// <exception cref="WebException"></exception>
//		/// <exception cref="ObjectDisposedException"></exception>
//		private static T Deserialize<T>(JObject jObject)
//		{
//			try
//			{
//				// if we get any other error from cryptowatch
//				foreach (var responseType in jObject)
//				{
//					if (responseType.Key == "error")
//						throw new Exception("Cryptowatch error: " + responseType.Value.ToObject<string>());
//				}
//				allowance = jObject["allowance"].ToObject<Allowance>();

//				return jObject["result"].ToObject<T>();
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		public override List<QuoteSymbol> GetQuoteSymbols(string name, string symbol)
//		{
//			throw new NotImplementedException();
//		}

//		protected override void PriceSubscribe(string name, string symbol, Action<object[]> action)
//		{
//			throw new NotImplementedException();
//		}

//		protected override void PriceUnsubscribe(string baseSymbol, string quoteSymbol)
//		{
//			throw new NotImplementedException();
//		}

//		protected override void CandleSubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, int length, Action<object[]> action)
//		{
//			throw new NotImplementedException();
//		}

//		protected override void CandleUnsubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, int length)
//		{
//			throw new NotImplementedException();
//		}

//		private class _OrderBook
//		{
//			public float[][] asks { get; set; }
//			public float[][] bids { get; set; }
//		}

//		private class _Candlestick
//		{
//			public float[][] allCandlesticks { get; set; }
//			[JsonProperty("60")]
//			private float[][] min { set { allCandlesticks = value; } }
//			[JsonProperty("180")]
//			private float[][] _180 { set { allCandlesticks = value; } }
//			[JsonProperty("300")]
//			private float[][] _300 { set { allCandlesticks = value; } }
//			[JsonProperty("900")]
//			private float[][] _900 { set { allCandlesticks = value; } }
//			[JsonProperty("1800")]
//			private float[][] _1800 { set { allCandlesticks = value; } }
//			[JsonProperty("3600")]
//			private float[][] _3600 { set { allCandlesticks = value; } }
//			[JsonProperty("7200")]
//			private float[][] _7200 { set { allCandlesticks = value; } }
//			[JsonProperty("14400")]
//			private float[][] _14400 { set { allCandlesticks = value; } }
//			[JsonProperty("21600")]
//			private float[][] _21600 { set { allCandlesticks = value; } }
//			[JsonProperty("43200")]
//			private float[][] _43200 { set { allCandlesticks = value; } }
//			[JsonProperty("86400")]
//			private float[][] _86400 { set { allCandlesticks = value; } }
//			[JsonProperty("259200")]
//			private float[][] _259200 { set { allCandlesticks = value; } }
//			[JsonProperty("604800")]
//			private float[][] _604800 { set { allCandlesticks = value; } }
//		}

//		private class Price
//		{
//			public float price { get; set; }
//		}
//	}
//}
///*
// using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Globalization;
//using Alerts;
//using System.Threading;
//using System.Diagnostics;
//using CryptoWatcher.Alerts;
//using CryptoWatcher.APIs;

//namespace CryptoWatcher
//{
//	// NOTE: when binance is selected notify user that we compare symbol not name (BCC BitConnect = BCC Bitcoin Cash on binance)
//	public partial class CustomAlertForm : MetroFramework.Forms.MetroForm
//	{
//		string baseName;
//		string baseSymbol;
//		bool marketIsRunning;
//		public static MainForm MainForm { get; set; }
//		//List<Exchange> exchanges;
//		// these are only needed when price needs to be converted
//		//Dictionary<int, string> middleMarket;

//		public CustomAlertForm()
//		{
//			Alert.TickerList1stLoad += Alert_TickerList1stLoad;
//			InitializeComponent();
//			AbstractAlert.AlertForm = this;
//			PopulateConditions();

//			if (Alert.AssetList == null)
//				PopulateAssetsAsync();
//		}

//		private void Alert_TickerList1stLoad(object sender, EventArgs e)
//		{
//			if (txtSymbol.AutoCompleteCustomSource.Count == 0)
//			{
//				AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
//				// populating autocomplete list in symbol textbox
//				foreach (var ticker in Alert.TickerList)
//					acsc.Add(ticker.symbol.ToUpper() + " " + ticker.name);

//				txtSymbol.AutoCompleteCustomSource = acsc;
//			}
//		}

//		private void PopulateConditions()
//		{
//			cboxCondition.Items.Add(new AlertItem(new PriceAlert()));
//			cboxCondition.Items.Add(new AlertItem(new RSIAlert()));
//			cboxCondition.Items.Add(new AlertItem(new StochAlert()));
//		}

//		private void CustomAlertForm_VisibleChanged(object sender, EventArgs e)
//		{
//			if (Visible)
//			{
//				tabControl.SelectedTab = tabCondition;
//				baseName = null;
//				baseSymbol = null;
//				txtSymbol.Text = "";
//				cboxCondition.SelectedIndex = -1;
//				while (cboxMarket.Items.Count > 0)
//					cboxMarket.Items.RemoveAt(0);
//				while (cboxExchange.Items.Count > 0)
//					cboxExchange.Items.RemoveAt(0);
//				cBoxSound.SelectedIndex = 0;
//				chcBoxWindowsMsg.Checked = true;
//				chcBoxShowWindow.Checked = true;
//				chcBoxRepeatable.Checked = false;
//				txtInterval.Text = "";
//				cBoxInterval.SelectedIndex = 0;
//				lblError.Text = "";
//				pBox5Min.Visible = false;
//				marketIsRunning = false;
//			}
//		}

//		private void PopulateAssetsAsync()
//		{
//			cboxMarket.Items.AddRange(AbstractAPI.GetQuoteSymbols(txtSymbol.Text).ToArray());
//			//Alert.AssetList = await CryptowatchAPI.GetAssets();
//		}

//		// evolution of lambda expressions
//		//private delegate bool TxtSymbolContains_d(string str);
//		//private bool TxtSymbolContains(string str)
//		//{
//		//	if (txtSymbol.Text.Contains(str))
//		//		return true;
//		//	return false;
//		//}
//		// 1.
//		//txtSymbol.Invoke(new TxtSymbolContains_d(TxtSymbolContains), a.symbol.ToUpper());
//		//2.
//		//TxtSymbolContains_d method = delegate (string str) { return txtSymbol.Text.Contains(str) ? true : false; };
//		//txtSymbol.Invoke(method, a.symbol.ToUpper());
//		//3.
//		//TxtSymbolContains_d method = str => { return txtSymbol.Text.Contains(str) ? true : false; };
//		//txtSymbol.Invoke(method, a.symbol.ToUpper());
//		//4.
//		//Func<string, bool> func = str => { return txtSymbol.Text.Contains(str) ? true : false; };
//		//txtSymbol.Invoke(func, a.symbol.ToUpper());
//		//5.
//		//txtSymbol.Invoke(new Func<string, bool>(str => { return txtSymbol.Text.Contains(str) ? true : false; }), a.symbol.ToUpper());

//		private async void txtSymbol_LeaveAsync(object sender, EventArgs e)
//		{
//			await LoadMarkets();

//			await LoadExchanges(false);
//			//await task.ContinueWith(task => LoadExchanges(source.Token));
//		}

//		private async Task LoadMarkets()
//		{
//			Console.WriteLine("LoadMarkets started");

//			// looking with cryptowatch API
//			foreach (var a in Alert.AssetList)
//			{
//				// searching for symbol
//				if (txtSymbol.Text == a.symbol.ToUpper() + " " + a.name)
//				{
//					SymbolChanged();

//					Asset asset = await CryptowatchAPI.GetAsset(a.route);
//					baseSymbol = a.symbol.ToLower();
//					baseName = a.name;

//					await AddQuotesToListAsync(asset);
//					Console.WriteLine("Load markets finished");
//					return;
//				}
//			}
//			if (Alert.TickerList == null)
//				return; // TODO: thorw network error / signal no network / network slow
//			// if we got here that means we haven't found symbol in cryptowatch so we load it with coinmarketcap
//			foreach (var t in Alert.TickerList)
//			{
//				if (txtSymbol.Text == t.symbol + " " + t.name)
//				{
//					SymbolChanged();
//					baseSymbol = t.symbol;
//					baseName = t.name;
//					cboxMarket.Items.Add(new MarketItem("USD", false));
//					cboxMarket.Items.Add(new MarketItem("BTC", false));
//					cboxMarket.SelectedIndex = 0;
//					Console.WriteLine("Load markets finished");
//					break;
//				}
//			}
//		}

//		void SymbolChanged()
//		{
//			lblError.Text = "";

//			// removing selected items because user changed coin name
//			cboxExchange.Items.Clear();
//			cboxMarket.Items.Clear();
//			if (pBox5Min.Visible)
//				pBox5Min.Visible = false;
//		}

//		private async Task AddQuotesToListAsync(Asset asset)
//		{
//			Console.WriteLine("Loading quotes");
//			List<Asset> quoteAssets = new List<Asset>();

//			// populating market combo box for that asset
//			foreach (var m in asset.markets.baseMarket)
//			{
//				string quote = m.pair.Substring(baseSymbol.Length).ToUpper();

//				// ignoring btcusd-weekly-futures pairs
//				if (quote.Contains('-'))
//					continue;
//				MarketItem cq = new MarketItem(quote, true);

//				if (!IsInCBox(quote))
//				{
//					cboxMarket.Items.Add(cq);
//					cboxMarket.Refresh();
//					// auto select
//					if (cboxMarket.SelectedIndex == -1)
//						cboxMarket.SelectedIndex = 0;
//					try
//					{
//						quoteAssets.Add(await CryptowatchAPI.GetAsset("https://api.cryptowat.ch/assets/" + quote.ToLower()));
//					}
//					catch
//					{
//						continue;
//					}
//				}
//			}

//			// adding option to convert price to higher quote (e.g. etcbtc to etcusd because btcusd pair exists)
//			foreach (var quoteAsset in quoteAssets)
//			{
//				if (quoteAsset.markets.baseMarket != null)
//				{
//					AddCalculatedQuotesToList(quoteAsset);
//				}
//			}
//			Console.WriteLine("finishing quotes");

//		}

//		private bool IsInCBox(string quote)
//		{
//			foreach (MarketItem item in cboxMarket.Items)
//			{
//				if(item.Text == quote)
//				{
//					return true;
//				}
//			}
//			return false;
//		}

//		private void AddCalculatedQuotesToList(Asset quoteAsset)
//		{
//			foreach (var qbm in quoteAsset.markets.baseMarket)
//			{
//				string quoteQuote = qbm.pair.Substring(quoteAsset.symbol.Length).ToUpper() + " (calculated)";

//				// ignoring btcusd-weekly-futures pairs
//				if (quoteQuote.Contains('-'))
//					continue;
//				MarketItem cq = new MarketItem(quoteQuote, true);
//				if (!IsInCBox(quoteQuote))
//				{
//					cq.MiddleMarket = qbm.exchange + "/" + qbm.pair;
//					cboxMarket.Items.Add(cq);
//					cboxMarket.Refresh();
//					// bitfinex/btcusd
//				}
//			}
//		}

//		// changes panel to customize alert
//		private void cboxCondition_SelectedIndexChanged(object sender, EventArgs e)
//		{
//			if(tabCondition.Controls.ContainsKey("panel"))
//				tabCondition.Controls.RemoveByKey("panel");
//			if (cboxCondition.SelectedItem != null)
//				tabCondition.Controls.Add(((AlertItem)cboxCondition.SelectedItem).abstractAlert.GetOptions());
//		}

//		private async void cboxMarket_LeaveAsync(object sender, EventArgs e)
//		{
//			//marketIsRunning = true;
//			//await LoadExchanges(true);
//			//marketIsRunning = false;
//			cboxExchange.Items.AddRange(((AbstractAPI.QuoteItem)cboxMarket.SelectedItem).Exchanges.ToArray());
//		}

//		private string GetQuoteSymbol()
//		{
//			return ((MarketItem)cboxMarket.SelectedItem).MiddleMarket.Substring(((MarketItem)cboxMarket.SelectedItem).MiddleMarket.IndexOf('/') + 1, (((MarketItem)cboxMarket.SelectedItem).MiddleMarket.Substring(((MarketItem)cboxMarket.SelectedItem).MiddleMarket.IndexOf('/') + 1).Length - cboxMarket.Text.Substring(0, cboxMarket.Text.IndexOf(" (calculated)")).Length));
//		}

//		private async Task LoadExchanges(bool isUserIssued)
//		{
//			Console.WriteLine("LoadExchanges started");
//			if (baseSymbol == null)
//			{
//				lblError.Text = "Asset not found.";
//				return;
//			}
//			cboxExchange.Items.Clear();
//			// there is no exchange in coinmarketcap
//			if (!((MarketItem)cboxMarket.SelectedItem).IsInCryptowatch)
//			{
//				cboxExchange.Items.Add("CoinMarketCap");
//				pBox5Min.Visible = true;
//				cboxExchange.SelectedIndex = 0;
//				return;
//			}
//			Pair pair;


//			// geting middle pair
//			if (((MarketItem)cboxMarket.SelectedItem).MiddleMarket != null)
//			{
//				//bitfinex/btcusd
//				pair = await CryptowatchAPI.GetPair("https://api.cryptowat.ch/pairs/" + baseSymbol + GetQuoteSymbol());
//			}
//			else
//			{
//				try
//				{
//					pair = await CryptowatchAPI.GetPair("https://api.cryptowat.ch/pairs/" + baseSymbol + cboxMarket.Text.ToLower());
//				}
//				catch
//				{
//					pair = null;
//					return;
//				}
//			}

//			// populating exchange combo box
//			foreach (var market in pair.markets)
//			{
//				Exchange exchange = await CryptowatchAPI.GetExchange("https://api.cryptowat.ch/exchanges/" + market.exchange);
//				// preventing old method that isn't finished yet
//				if (!isUserIssued && marketIsRunning)
//					return;
//				cboxExchange.Items.Add(new ExchangeItem(exchange));
//				// auto select
//				if (cboxExchange.SelectedIndex == -1)
//					cboxExchange.SelectedIndex = 0;
//			}
//			Console.WriteLine("LoadExchanges finished");

//		}

//		private void btnAddAlert_Click(object sender, EventArgs e)
//		{
//			string route;
//			string endRoute = "";
//			AlertData alertData;
//			Notification ntfy;

//			// user input checking
//			// creating notification
//			if (!GetNotification(out ntfy))
//			{
//				MessageBox.Show("Something is wrong in interval section.");
//				return;
//			}

//			if (cboxMarket.Items.Count == 0 || cboxExchange.Items.Count == 0)
//			{
//				MessageBox.Show("Please populate required fields.");
//				return;
//			}

//			// creating alert data which requires price to convert
//			if (((MarketItem)cboxMarket.SelectedItem).MiddleMarket != null)
//			{
//				string quoteSymbol = GetQuoteSymbol();
//				route = "https://api.cryptowat.ch/markets/" + ((ExchangeItem)cboxExchange.SelectedItem).Exchange.symbol + "/" + baseSymbol + quoteSymbol;
//				endRoute = "https://api.cryptowat.ch/markets/" + ((MarketItem)cboxMarket.SelectedItem).MiddleMarket;
//				alertData = new AlertData(baseSymbol.ToUpper(), cboxMarket.Text.Substring(0, cboxMarket.Text.IndexOf(" (calculated)") + 1), baseName, ((ExchangeItem)cboxExchange.SelectedItem).Exchange.name, route, endRoute);
//			}
//			// creating cryptowatch alert data
//			else if(((MarketItem)cboxMarket.SelectedItem).IsInCryptowatch)
//			{
//				route = "https://api.cryptowat.ch/markets/" + ((ExchangeItem)cboxExchange.SelectedItem).Exchange.symbol + "/" + baseSymbol + cboxMarket.Text.ToLower();
//				alertData = new AlertData(baseSymbol.ToUpper(), cboxMarket.Text.ToUpper(), baseName, ((ExchangeItem)cboxExchange.SelectedItem).Exchange.name, route, null);
//			}
//			// creating coinmarketcap alert data
//			else
//			{
//				alertData = new AlertData(baseSymbol.ToUpper(), cboxMarket.Text.ToUpper(), baseName, "CoinMarketCap", null, null);
//			}

//			// value in custom alert is not valid
//			if (!((AlertItem)(cboxCondition.SelectedItem)).abstractAlert.Create(alertData, ntfy, (MetroFramework.Controls.MetroPanel) tabCondition.Controls["panel"]))
//			{
//				return;
//			}
//			Alert.AlertList.Add(((AlertItem)(cboxCondition.SelectedItem)).abstractAlert);
//			Alert.SaveAlerts();
//			MainForm.AddAlertToListView();
//			Close();
//		}

//		private bool GetNotification(out Notification notification)
//		{
//			int interval = -1;

//			if (chcBoxRepeatable.Checked)
//			{
//				notification = null;
//				// user input checking
//				if (!int.TryParse(txtInterval.Text, out interval))
//					return false;
//				// converting to seconds
//				switch (cBoxInterval.Text)
//				{
//					case "Minutes": interval *= 60; break;
//					case "Hours": interval *= 3600; break;
//					case "Days": interval *= 86400; break;
//					case "Weeks": interval *= 604800; break;
//					case "Months": interval *= 18144000; break;
//				}
//			}
//			notification = new Notification((Notification.SoundType)cBoxSound.SelectedIndex, chcBoxWindowsMsg.Checked, chcBoxShowWindow.Checked, interval);

//			return true;
//		}
//		// when user tabs to combobox dropdown appears
//		private void cboxCondition_Enter(object sender, EventArgs e)
//		{
//			if (MouseButtons == MouseButtons.None)
//				((ComboBox)sender).DroppedDown = true;
//		}

//		private void cboxMarket_Enter(object sender, EventArgs e)
//		{
//			if (MouseButtons == MouseButtons.None)
//				((ComboBox)sender).DroppedDown = true;
//		}

//		private void cboxExchange_Enter(object sender, EventArgs e)
//		{
//			if (MouseButtons == MouseButtons.None)
//				((ComboBox)sender).DroppedDown = true;
//		}

//		private void chcBoxRepeatable_CheckedChanged(object sender, EventArgs e)
//		{
//			if (chcBoxRepeatable.Checked)
//			{
//				lblInterval.Visible = true;
//				txtInterval.Visible = true;
//				cBoxInterval.SelectedIndex = 0;
//				cBoxInterval.Visible = true;
//			}
//			else
//			{
//				lblInterval.Visible = false;
//				txtInterval.Visible = false;
//				cBoxInterval.Visible = false;
//			}
//		}

//		protected override void OnFormClosing(FormClosingEventArgs e)
//		{
//			Hide();
//			e.Cancel = true;
//			//base.OnFormClosing(e);
//		}

//		private class AlertItem
//		{
//			public AbstractAlert abstractAlert { get; }

//			public AlertItem(AbstractAlert abstractAlert)
//			{
//				this.abstractAlert = abstractAlert;
//			}

//			public override string ToString()
//			{
//				return abstractAlert.Name;
//			}
//		}

//		public class MarketItem
//		{
//			public string Text { get; set; }
//			public bool IsInCryptowatch { get; set; }
//			public string MiddleMarket { get; set; } = null;

//			public MarketItem(string text, bool isInCryptowatch)
//			{
//				Text = text;
//				IsInCryptowatch = isInCryptowatch;
//			}

//			public override string ToString()
//			{
//				return Text;
//			}
//		}

//		public class ExchangeItem
//		{
//			public Exchange Exchange { get; set; }

//			public ExchangeItem(Exchange exchange)
//			{
//				Exchange = exchange;
//			}

//			public override string ToString()
//			{
//				return Exchange.name;
//			}
//		}

//		public class TimeframeItem
//		{
//			public Timeframe timeframe { get; set; }

//			public TimeframeItem(Timeframe timeframe)
//			{
//				this.timeframe = timeframe;
//			}

//			public override string ToString()
//			{
//				switch (timeframe)
//				{
//					case Timeframe.min1: return "1 minute";
//					case Timeframe.min3: return "3 minutes";
//					case Timeframe.min5: return "5 minutes";
//					case Timeframe.min15: return "15 minutes";
//					case Timeframe.min30: return "30 minutes";
//					case Timeframe.h1: return "1 hour";
//					case Timeframe.h2: return "2 hours";
//					case Timeframe.h4: return "4 hours";
//					case Timeframe.h6: return "6 hours";
//					case Timeframe.h12: return "12 hours";
//					case Timeframe.d1: return "1 day";
//					case Timeframe.d3: return "3 days";
//					case Timeframe.w1: return "1 week";
//				}
//				return "NO TIMEFRAME";
//			}
//		}
//	}
//}*/
