// Copyright(c) 2017 Stock84-dev
// https://github.com/Stock84-dev/Cryptowatch-API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json; // install Newtonsoft.Json nuGet package
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace APIs
{
	/// <summary>
	/// Documentation can be found here https://cryptowat.ch/docs/api
	/// </summary>
	public class CryptowatchAPI
	{
		/// <summary>
		/// Allowance is updated with every call.
		/// </summary>
		private static Allowance _allowance = new Allowance();
		public static Allowance allowance {
			get {
				return _allowance;
			}
			set {
				_allowance = value;
				OnAllowanceChanged(new AllowanceEventArgs(_allowance));
			}
		}
		public delegate void AllowanceChangedEventHandler(object sender, AllowanceEventArgs e);
		public static event AllowanceChangedEventHandler AllowanceChanged;

		private static void OnAllowanceChanged(AllowanceEventArgs e)
		{
			if (AllowanceChanged != null)
				AllowanceChanged(typeof(CryptowatchAPI), e);
		}

		public static class AverageCost
		{
			public const int GetAssets = 715062;
			public const int GetAsset = 3659875;
			public const int GetCandlesticks = 8320491;
			public const int GetExchanges = 83377;
			public const int GetExchange = 46502;
			public const int GetMarkets = 823717;
			public const int GetMarkets1 = 203050; // get exchange specific market
			public const int GetMarket = 227696;
			public const int GetOrderBook = 6275745;
			public const int GetPairs = 4753742;
			public const int GetPair = 215526;
			public const int GetPrices = 2379264;
			public const int GetPrice = 2267609;
			public const int GetSiteInformation = 15332;
			public const int GetSummaries = 9139062;
			public const int GetSummary = 20959771;
			public const int GetTrades = 6478083;
		}

		public static class MaximumCost
		{
			public const int GetAssets = 24794198;
			public const int GetAsset = 295800527;
			public const int GetCandlesticks = 72606033;
			public const int GetExchanges = 1668110;
			public const int GetExchange = 647960;
			public const int GetMarkets = 6569475;
			public const int GetMarkets1 = 1964170; // get exchange specific market
			public const int GetMarket = 14665724;
			public const int GetOrderBook = 330483278;
			public const int GetPairs = 130313939;
			public const int GetPair = 1674564;
			public const int GetPrices = 35111505;
			public const int GetPrice = 20712684;
			public const int GetSiteInformation = 9831044;
			public const int GetSummaries = 192400297;
			public const int GetSummary = 338321144;
			public const int GetTrades = 6478083;//error
		}

		///<summary>
		///You can always request this to query your allowance without any extra result - this request costs very little.
		/// </summary>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<SiteInformation> GetSiteInformation()
		{
			try
			{
				return Deserialize<SiteInformation>(await GetJObject("https://api.cryptowat.ch"));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///Returns all assets(in no particular order)
		/// </summary>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<List<Assets>> GetAssets()
		{
			try
			{
				return DeserializeToList<Assets>(await GetJObject("https://api.cryptowat.ch/assets"));
			}
			catch
			{
				throw;
			}
			
		}

		/// <summary>
		///Returns a single asset. Lists all markets which have this asset as a base or quote.
		/// </summary>
		/// <param name="route"> Asset specific url, e.g. https://api.cryptowat.ch/assets/btc </param>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<Asset> GetAsset(string route)
		{
			try
			{
				return Deserialize<Asset>(await GetJObject(route));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns all pairs (in no particular order).
		/// </summary>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<List<Pairs>> GetPairs()
		{
			try
			{
				return DeserializeToList<Pairs>(await GetJObject("https://api.cryptowat.ch/pairs"));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///	Returns a single pair. Lists all markets for this pair.
		/// </summary>
		/// <param name="route"> Pair specific url, e.g. https://api.cryptowat.ch/pairs/ethbtc </param>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<Pair> GetPair(string route)
		{
			try
			{
				return Deserialize<Pair>(await GetJObject(route));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a list of all supported exchanges.
		/// </summary>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<List<Exchanges>> GetExchanges()
		{
			try
			{
				return DeserializeToList<Exchanges>(await GetJObject("https://api.cryptowat.ch/exchanges"));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///	Returns a single exchange, with associated routes.
		/// </summary>
		/// <param name="route"> Exchange specific url, e.g. https://api.cryptowat.ch/exchanges/kraken </param> 
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<Exchange> GetExchange(string route)
		{
			try
			{
				return Deserialize<Exchange>(await GetJObject(route));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a list of all supported markets.
		/// </summary>
		/// <param name="route">You can also get the supported markets for only a specific exchange. e.g. https://api.cryptowat.ch/markets/kraken </param>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<List<Markets>> GetMarkets(string route = "https://api.cryptowat.ch/markets")
		{
			try
			{
				return DeserializeToList<Markets>(await GetJObject(route));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///	Returns a single market, with associated routes.
		/// </summary>
		/// <param name="route"> Market specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd </param> 
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<Market> GetMarket(string route)
		{
			try
			{
				return Deserialize<Market>(await GetJObject(route));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the current price for all supported markets. Some values may be out of date by a few seconds. 
		/// <para>
		/// key = exchangeName:pairName
		/// </para>
		/// </summary>
		/// <returns> dictionsry</returns>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<Dictionary<string, float>> GetPrices()
		{
			try
			{
				return Deserialize<Dictionary<string, float>>(await GetJObject("https://api.cryptowat.ch/markets/prices"));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///	Returns a market’s last price.
		/// </summary>
		/// <param name="route"> Price specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/price </param> 
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public async static Task<float> GetPrice(string route)
		{
			try
			{
				return Deserialize<Price>(await GetJObject(route)).price;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the market summary for all supported markets. Some values may be out of date by a few seconds.
		/// <para>
		/// key = exchangeName:pairName
		/// </para>
		/// </summary>
		/// <returns> dictionsry</returns>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<Dictionary<string, Summary>> GetSummaries()
		{
			try
			{
				return Deserialize<Dictionary<string, Summary>>(await GetJObject("https://api.cryptowat.ch/markets/summaries"));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a market’s last price as well as other stats based on a 24-hour sliding window: High price, Low price, % change, Absolute change, Volume
		/// </summary>
		/// <param name="route"> Summary specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/summary </param> 
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<Summary> GetSummary(string route)
		{
			try
			{
				return Deserialize<Summary>(await GetJObject(route));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a market’s most recent trades, incrementing chronologically. Note some exchanges don’t provide IDs for public trades.
		/// </summary>
		/// <param name="route"> Trade specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/trades </param> 
		/// <param name="limit"> Limit amount of trades returned. If 0 returns all.</param> 
		/// <param name="since"> Only return trades at or after this time. </param> 
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<List<Trade>> GetTrades(string route, int limit = 50, long since = -1)
		{
			if (limit != 50 && since != -1)
			{
				route += "?limit=" + limit.ToString() + "&since=" + since.ToString();
			}
			else if(since != -1)
			{
				route += "?since=" + since.ToString();
			}
			else if(limit != 50)
			{
				route += "?limit=" + limit.ToString();
			}
			try
			{
				List<float[]> tradeList = DeserializeToList<float[]>(await GetJObject(route));
				List<Trade> trades = new List<Trade>();
				foreach (var t in tradeList)
				{
					trades.Add(new Trade((int)t[0], (long)t[1], t[2], t[3]));
				}
				return trades;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a market’s order book.
		/// </summary>
		/// <param name="route"> OrderNook specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/orderbook </param> 
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<OrderBook> GetOrderBook(string route)
		{
			try
			{
				_OrderBook _orderBook = Deserialize<_OrderBook>(await GetJObject(route));
				OrderBook orderBook = new OrderBook();
				foreach (var bid in _orderBook.bids)
				{
					orderBook.bids.Add(new Order(bid[0], bid[1]));
				}
				foreach (var ask in _orderBook.asks)
				{
					orderBook.asks.Add(new Order(ask[0], ask[1]));
				}
				return orderBook;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a market’s OHLC candlestick data.
		/// </summary>
		/// <param name="route"> Candlestick specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/ohlc </param> 
		/// <param name="timeFrame"> Candlestick timeframe.</param> 
		/// <param name="after"> Only return candles opening after this time. If set to -1 max limit is 6000, otherwise it's 500.</param> 
		/// <param name="before"> Only return candles opening before this time. </param> 
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		public static async Task<List<Candlestick>> GetCandlesticks(string route, TimeFrame timeFrame, long after = -2, long before = 0)
		{
			route += "?periods=" + ((int)timeFrame).ToString();
			if (after != -2)
			{
				route += "&after=" + after.ToString();
			}
			if (before != 0)
			{
				route += "&before=" + before.ToString();
			}
			try
			{
			 	_Candlestick _candlestick = Deserialize<_Candlestick>(await GetJObject(route));
				List<Candlestick> candles = new List<Candlestick>();
				foreach (var c in _candlestick.allCandlesticks)
				{
					candles.Add(new Candlestick((long)c[0], c[1], c[2], c[3], c[4], c[5]));
				}
				return candles;
			}
			catch
			{
				throw;
			}
		}

		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		private static async Task<Stream> GetResponseStream(string url)
		{
			try
			{
				var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Accept = "*/*";
				httpWebRequest.Method = "GET";
				//httpWebRequest.Headers.Add("Authorization", "Basic reallylongstring");
				//var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
				return httpResponse.GetResponseStream();
			}
			catch
			{
				throw;
			}
			
		}
	
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		private static async Task<JObject> GetJObject(string url)
		{
			try
			{
				StreamReader sr = new StreamReader(await GetResponseStream(url));
				string response = sr.ReadToEnd();
				JObject jObject = JObject.Parse(response);
				sr.Close();
				return jObject;
			}
			catch
			{
				throw;
			}
		}
		
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		private static List<T> DeserializeToList<T>(JObject jObject)
		{
			try
			{
				// if we get any other error from cryptowatch
				foreach (var responseType in jObject)
				{
					if (responseType.Key == "error")
						throw new Exception("Cryptowatch error: " + responseType.Value.ToObject<string>());
				}
				// get JSON result objects into a list
				List<JToken> jTokens = jObject["result"].Children().ToList();
				allowance = jObject["allowance"].ToObject<Allowance>();

				// serialize JSON results into .NET objects
				List<T> objects = new List<T>();
				foreach (JToken jToken in jTokens)
				{
					// JToken.ToObject is a helper method that uses JsonSerializer internally
					objects.Add(jToken.ToObject<T>());
				}
				return objects;
			}
			catch
			{
				throw;
			}
			
		}
		
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="JsonReaderException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ProtocolViolationException"></exception>
		/// <exception cref="WebException"></exception>
		/// <exception cref="ObjectDisposedException"></exception>
		private static T Deserialize<T>(JObject jObject)
		{
			try
			{
				// if we get any other error from cryptowatch
				foreach (var responseType in jObject)
				{
					if (responseType.Key == "error")
						throw new Exception("Cryptowatch error: " + responseType.Value.ToObject<string>());
				}
				allowance = jObject["allowance"].ToObject<Allowance>();

				return jObject["result"].ToObject<T>();
			}
			catch
			{
				throw;
			}
		}

		private class _OrderBook
		{
			public float[][] asks { get; set; }
			public float[][] bids { get; set; }
		}

		private class _Candlestick
		{
			public float[][] allCandlesticks { get; set; }
			[JsonProperty("60")]
			private float[][] min { set { allCandlesticks = value; } }
			[JsonProperty("180")]
			private float[][] _180 { set { allCandlesticks = value; } }
			[JsonProperty("300")]
			private float[][] _300 { set { allCandlesticks = value; } }
			[JsonProperty("900")]
			private float[][] _900 { set { allCandlesticks = value; } }
			[JsonProperty("1800")]
			private float[][] _1800 { set { allCandlesticks = value; } }
			[JsonProperty("3600")]
			private float[][] _3600 { set { allCandlesticks = value; } }
			[JsonProperty("7200")]
			private float[][] _7200 { set { allCandlesticks = value; } }
			[JsonProperty("14400")]
			private float[][] _14400 { set { allCandlesticks = value; } }
			[JsonProperty("21600")]
			private float[][] _21600 { set { allCandlesticks = value; } }
			[JsonProperty("43200")]
			private float[][] _43200 { set { allCandlesticks = value; } }
			[JsonProperty("86400")]
			private float[][] _86400 { set { allCandlesticks = value; } }
			[JsonProperty("259200")]
			private float[][] _259200 { set { allCandlesticks = value; } }
			[JsonProperty("604800")]
			private float[][] _604800 { set { allCandlesticks = value; } }
		}

		private class Price
		{
			public float price { get; set; }
		}
	}
}