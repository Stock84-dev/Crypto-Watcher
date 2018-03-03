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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CryptoWatcher.Utilities;
using Newtonsoft.Json;

namespace CryptoWatcher.APIs
{
	public abstract class AbstractAPI
	{
		protected static MyEvent priceSubscription = new MyEvent();
		protected static MyEvent candleSubscription = new MyEvent();
		//static BinanceAPI binanceAPI = new BinanceAPI();
		private static Dictionary<string, AbstractAPI> exchanges;

        public abstract List<QuoteSymbol> GetQuoteSymbols(string name, string symbol);
		protected abstract void PriceSubscribe(string baseSymbol, string quoteSymbol, Action<object[]> action);
		protected abstract void PriceUnsubscribe(string baseSymbol, string quoteSymbol, Action<object[]> action);
		protected abstract void CandleSubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, int length, Action<object[]> action);
		protected abstract void CandleUnsubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, Action<object[]> action);
		public abstract bool SupportsCandles { get; }

		public static void Init()
		{
            exchanges = new Dictionary<string, AbstractAPI>
            {
                { CoinMarketCapAPI.Name, new CoinMarketCapAPI() },
                { CryptoCompareAPI.Name, new CryptoCompareAPI() }
            };
        }

		public static void UnsubscribePrice(string exchange, string baseSymbol, string quoteSymbol, Action<object[]> action)
		{
			exchanges[exchange].PriceUnsubscribe(baseSymbol, quoteSymbol, action);
		}

		public static void SubscrbePrice(string exchange, string baseSymbol, string quoteSymbol, Action<object[]> action)
		{
			exchanges[exchange].PriceSubscribe(baseSymbol, quoteSymbol, action);
		}

		public static void UnsubscribeCandle(string exchange, string baseSymbol, string quoteSymbol, Timeframe timeframe, Action<object[]> action)
		{
			exchanges[exchange].CandleUnsubscribe(baseSymbol, quoteSymbol, timeframe, action);
		}

		public static void SubscrbeCandle(string exchange, string baseSymbol, string quoteSymbol, Timeframe timeframe, int length, Action<object[]> action)
		{
			exchanges[exchange].CandleSubscribe(baseSymbol, quoteSymbol, timeframe, length, action);
		}

		public static List<QuoteItem> GetQuoteItems(string name, string symbol)
		{
			List<QuoteSymbol> quoteSymbols = new List<QuoteSymbol>();
			foreach (var exchange in exchanges)
			{
				quoteSymbols.AddRange(exchange.Value.GetQuoteSymbols(name, symbol));
			}
			return Group(quoteSymbols);
		}
		
		public class QuoteSymbol
		{
			public QuoteSymbol(string exchange, string quote)
			{
				Exchange = exchange;
				Quote = quote;
			}

			public string Exchange { get; set; }
			public string Quote { get; set; }

		}

		public static Timeframe StringToTimeframe(string timeframe)
		{
			switch (timeframe)
			{
				case "1 minute":
					return Timeframe.min1;
				case "3 minutes":
					return Timeframe.min3;
				case "5 minutes":
					return Timeframe.min5;
				case "15 minutes":
					return Timeframe.min15;
				case "30 minutes":
					return Timeframe.min30;
				case "1 hour":
					return Timeframe.h1;
				case "2 hours":
					return Timeframe.h2;
				case "4 hours":
					return Timeframe.h4;
				case "6 hours":
					return Timeframe.h6;
				case "12 hours":
					return Timeframe.h12;
				case "1 day":
					return Timeframe.d1;
				case "3 days":
					return Timeframe.d3;
				case "1 week":
					return Timeframe.w1;
			}
			throw new ArgumentException();
		}

		public static string[] GetTimeframes()
		{
			var values = Enum.GetValues(typeof(Timeframe)).Cast<Timeframe>().ToArray();
			string[] types = new string[values.Count()-1];

			for (int i = 1; i <= types.Length; i++)
			{
				types[i-1] = TimeframeToString(values[i]);
			}
			return types;
		}

		public static string TimeframeToString(Timeframe timeframe)
		{
			switch (timeframe)
			{
				case Timeframe.min1: return "1 minute";
				case Timeframe.min3: return "3 minutes";
				case Timeframe.min5: return "5 minutes";
				case Timeframe.min15: return "15 minutes";
				case Timeframe.min30: return "30 minutes";
				case Timeframe.h1: return "1 hour";
				case Timeframe.h2: return "2 hours";
				case Timeframe.h4: return "4 hours";
				case Timeframe.h6: return "6 hours";
				case Timeframe.h12: return "12 hours";
				case Timeframe.d1: return "1 day";
				case Timeframe.d3: return "3 days";
				case Timeframe.w1: return "1 week";
			}
			throw new ArgumentException();
		}

		protected static string GetEndpoint(List<string> param)
		{
			if (param.Count == 0)
				return "";
			string endpoint = $"?{param[0]}";
			for (int i = 1; i < param.Count; i++)
			{
				endpoint += $"&{param[i]}";
			}
			return endpoint;
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
		protected virtual T Deserialize<T>(JObject jObject)
		{
			return jObject.ToObject<T>();
		}

		protected async virtual Task<T> Deserialize<T>(string url)
		{
			return (await GetJObject(url)).ToObject<T>();
		}

		protected static T[] DeserializeToArray<T>(JToken jToken)
		{
			try
			{
				JToken[] jTokens = jToken.Children().ToArray();

				// serialize JSON results into .NET objects
				T[] objects = new T[jTokens.Length];
				for (int i = 0; i < jTokens.Length; i++)
				{
					objects[i] = jTokens[i].ToObject<T>();
				}
				return objects;
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
		protected virtual async Task<Stream> GetResponseStream(string url)
		{
			try
			{
				var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Accept = "*/*";
				httpWebRequest.Method = "GET";
				httpWebRequest.Timeout = 10000;
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
		protected virtual async Task<JObject> GetJObject(string url)
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

		protected virtual async Task<JToken> GetJToken(string url)
		{
			// trying 3 times because we can have bad connection
			for (int i = 0; i < 3; i++)
			{
				try
				{
					StreamReader sr = new StreamReader(await GetResponseStream(url));
					string response = sr.ReadToEnd();
					JToken jToken = JToken.Parse(response);
					sr.Close();
					return jToken;
				}
				catch (WebException)
				{
					await Task.Delay(1000);
				}
			}
			throw new Exception("Couldn't get response from server.");
		}

		private static List<QuoteItem> Group(List<QuoteSymbol> quoteSymbols)
		{
			List<QuoteItem> quoteItems = new List<QuoteItem>();
			foreach (var qs in quoteSymbols)
			{
				bool added = false;
				foreach (var qi in quoteItems)
				{
					if (qi.Quote == qs.Quote)
					{
						qi.Exchanges.Add(qs.Exchange);
						added = true;
						break;
					}
				}
				if (!added)
				{
					quoteItems.Add(new QuoteItem(qs.Exchange, qs.Quote));
				}
			}
			return quoteItems;
		}
	}

	public class Candlestick
	{
		[JsonProperty("time")]
		public int openTime;
		public float open;
		public float high;
		public float low;
		public float close;
		/// <summary>
		/// Volume in quote currency.
		/// </summary>
		[JsonProperty("volumeto")]
		public float volume;

		public Candlestick(int closeTime, float open, float high, float low, float close, float volume)
		{
			this.openTime = closeTime;
			this.open = open;
			this.high = high;
			this.low = low;
			this.close = close;
			this.volume = volume;
		}

		public static Candlestick operator *(Candlestick a, Candlestick b)
		{
			return new Candlestick(a.openTime, a.open * b.open, a.high * b.high, a.low * b.low, a.close * b.close, a.volume);
		}
	}

	public class QuoteItem
	{
		public QuoteItem(string exchange, string quote)
		{
			Exchanges = new List<string>();
			Exchanges.Add(exchange);
			Quote = quote;
		}

		public List<string> Exchanges { get; set; }
		public string Quote { get; set; }

		public override string ToString()
		{
			return Quote;
		}
	}

	public class ExchangeItem
	{
		public ExchangeItem(string exchangeName)
		{
			Exchange = exchangeName;
		}

		public string Exchange { get; set; }

		public override string ToString()
		{
			return Exchange;
		}
	}
}
