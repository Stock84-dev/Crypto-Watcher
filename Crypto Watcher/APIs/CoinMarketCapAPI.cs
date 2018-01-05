using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIs
{
	class CoinMarketCapAPI
	{
		public static Task<List<Ticker>> GetTickers(int start = 1, int limit = 100, QuoteType quote = QuoteType.NONE)
		{
			return Task.Factory.StartNew(() =>
			{
				JToken jObject;
				string endpoint = "";
				if (start != 1)
					endpoint += $"start={start}&";
				if (limit != 100)
					endpoint += $"limit={limit}&";
				if (quote != QuoteType.NONE)
					endpoint += $"convert={QuoteToString(quote)}";
				try
				{
					jObject = GetJToken("https://api.coinmarketcap.com/v1/ticker/?" + endpoint);
				}
				catch
				{
					return null;
				}
				// get JSON result objects into a list
				List<JToken> jTokens = jObject.Children().ToList();
				// serialize JSON results into .NET objects
				List<Ticker> tickers = new List<Ticker>();
				foreach (JToken jToken in jTokens)
				{
					// JToken.ToObject is a helper method that uses JsonSerializer internally
					tickers.Add(jToken.ToObject<Ticker>());
					if (quote != QuoteType.NONE)
					{
						tickers[tickers.Count - 1].ConvertedPrice = jToken[$"price_{QuoteToString(quote)}"].ToObject<string>();
						tickers[tickers.Count - 1].Converted24hVolume = jToken[$"24h_volume_{QuoteToString(quote)}"].ToObject<string>();
						tickers[tickers.Count - 1].ConvertedMarketCap = jToken[$"market_cap_{QuoteToString(quote)}"].ToObject<string>();
						tickers[tickers.Count - 1].Quote = quote;
					}
				}
				return tickers;
			});
		}

		public static Ticker GetTicker(string id, QuoteType quote = QuoteType.NONE)
		{
			string endpoint = id + "/";

			JToken jObject;
			if (quote != QuoteType.NONE)
				endpoint += $"?convert={QuoteToString(quote)}";
			try
			{
				jObject = GetJToken("https://api.coinmarketcap.com/v1/ticker/" + endpoint);
			}
			catch
			{
				return null;
			}
			// get JSON result objects into a list
			List<JToken> jTokens = jObject.Children().ToList();
			// serialize JSON results into .NET objects
			Ticker ticker = jTokens[0].ToObject<Ticker>(); ;

			if (quote != QuoteType.NONE)
			{
				ticker.ConvertedPrice = jTokens[0][$"price_{QuoteToString(quote)}"].ToObject<string>();
				ticker.Converted24hVolume = jTokens[0][$"24h_volume_{QuoteToString(quote)}"].ToObject<string>();
				ticker.ConvertedMarketCap = jTokens[0][$"market_cap_{QuoteToString(quote)}"].ToObject<string>();
				ticker.Quote = quote;
			}

			return ticker;
		}

		public static GlobalData GetGlobalData(QuoteType quote = QuoteType.NONE)
		{
			string endpoint = "";
			JObject jObject;

			if (quote != QuoteType.NONE)
				endpoint += $"?convert={QuoteToString(quote)}";
			try
			{
				jObject = GetJObject("https://api.coinmarketcap.com/v1/global/" + endpoint);
			}
			catch
			{
				return null;
			}
			GlobalData globalData = jObject.ToObject<GlobalData>();
			if (quote != QuoteType.NONE)
			{
				globalData.Converted24hVolume = jObject[$"total_24h_volume_{QuoteToString(quote)}"].ToObject<float>();
				globalData.ConvertedMarketCap = jObject[$"total_market_cap_{QuoteToString(quote)}"].ToObject<float>();
				globalData.Quote = quote;
			}
			return globalData;
		}

		private static JObject GetJObject(string url)
		{
			try
			{
				StreamReader sr = new StreamReader(GetResponseStream(url));
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

		private static JToken GetJToken(string url)
		{
			try
			{
				StreamReader sr = new StreamReader(GetResponseStream(url));
				string response = sr.ReadToEnd();
				JToken jToken = JToken.Parse(response);
				sr.Close();
				return jToken;
			}
			catch
			{
				throw;
			}
		}


		private static Stream GetResponseStream(string url)
		{
			try
			{
				var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Accept = "*/*";
				httpWebRequest.Method = "GET";
				httpWebRequest.Timeout = 99000;
				//httpWebRequest.Headers.Add("Authorization", "Basic reallylongstring");
				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				return httpResponse.GetResponseStream();
			}
			catch
			{
				throw;
			}
			
		}

		public static string QuoteToString(QuoteType quote)
		{
			switch (quote)
			{
				case QuoteType.AUD: return "aud";
				case QuoteType.BRL: return "brl";
				case QuoteType.CAD: return "cad";
				case QuoteType.CHF: return "chf";
				case QuoteType.CLP: return "clp";
				case QuoteType.CNY: return "cny";
				case QuoteType.CZK: return "czk";
				case QuoteType.DKK: return "dkk";
				case QuoteType.EUR: return "eur";
				case QuoteType.GBP: return "gbp";
				case QuoteType.HKD: return "hkd";
				case QuoteType.HUF: return "huf";
				case QuoteType.IDR: return "idr";
				case QuoteType.ILS: return "ils";
				case QuoteType.INR: return "inr";
				case QuoteType.JPY: return "jpy";
				case QuoteType.KRW: return "krw";
				case QuoteType.MXN: return "mxn";
				case QuoteType.MYR: return "myr";
				case QuoteType.NOK: return "nok";
				case QuoteType.NZD: return "nzd";
				case QuoteType.PHP: return "php";
				case QuoteType.PKR: return "pkr";
				case QuoteType.PLN: return "pln";
				case QuoteType.RUB: return "rub";
				case QuoteType.SEK: return "sek";
				case QuoteType.SGD: return "sgd";
				case QuoteType.THB: return "thb";
				case QuoteType.TRY: return "try";
				case QuoteType.TWD: return "twd";
				case QuoteType.ZAR: return "zar";
				default:
					return null;
			}
		}
	}

	enum QuoteType { NONE, AUD, BRL, CAD, CHF, CLP, CNY, CZK, DKK, EUR, GBP, HKD, HUF, IDR, ILS, INR, JPY, KRW, MXN, MYR, NOK, NZD, PHP, PKR, PLN, RUB, SEK, SGD, THB, TRY, TWD, ZAR }

	class Ticker
	{
		public QuoteType Quote { get; set; } = QuoteType.NONE;
		public string id { get; set; }
		public string name { get; set; }
		public string symbol { get; set; }
		public string rank { get; set; }
		public string price_usd { get; set; }
		public string price_btc { get; set; }
		[JsonProperty("24h_volume_usd")]
		public string _24h_volume_usd { get; set; }
		public string market_cap_usd { get; set; }
		public string available_supply { get; set; }
		public string total_supply { get; set; }
		public string max_supply { get; set; }
		public string percent_change_1h { get; set; }
		public string percent_change_24h { get; set; }
		public string percent_change_7d { get; set; }
		public string last_updated { get; set; }
		public string ConvertedPrice { get; set; } = null;
		public string Converted24hVolume { get; set; } = null;
		public string ConvertedMarketCap { get; set; } = null;
	}

	class GlobalData
	{
		public QuoteType Quote { get; set; } = QuoteType.NONE;
		public float total_market_cap_usd { get; set; }
		public float total_24h_volume_usd { get; set; }
		public float bitcoin_percentage_of_market_cap { get; set; }
		public int active_currencies { get; set; }
		public int active_assets { get; set; }
		public int active_markets { get; set; }
		public int last_updated { get; set; }
		public float Converted24hVolume { get; set; } = -1;
		public float ConvertedMarketCap { get; set; } = -1;
	}
}




