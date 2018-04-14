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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoWatcher.APIs
{
    // NOTE: sometimes the server is overloaded and timeout error could happen
    public class CoinMarketCapAPI : AbstractAPI
    {
        public const string NAME = "CoinMarketCap";
        private static List<Ticker> _tickerList = null;
        private static System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
        private static System.Windows.Forms.Timer _startingTimer = new System.Windows.Forms.Timer();

        public CoinMarketCapAPI()
        {
            //if (_tickerList == null && _startingTimer.Enabled == false)
            //{
            //    Start();
            //}
        }

        public static async Task<List<Ticker>> GetTickerList()
        {
            await WaitForInit();
            return _tickerList;
        }

        public void Start()
        {
            //_timer.Interval = 300000;
            //_timer.Tick += Timer_Tick;
            Timer_Tick(null, null);
            //if (DateTime.Now.Minute % 5 != 0)
            //{
            //    _startingTimer.Interval = 1000;
            //    _startingTimer.Tick += StartingTimer_Tick;
            //    _startingTimer.Enabled = true;
            //}
            //else _timer.Enabled = true;
        }

		//public override List<QuoteSymbol> GetQuoteSymbols(string a = null, string b = null)
		//{
		//	return new List<QuoteSymbol>(3)
		//	{
		//		new QuoteSymbol(NAME, "USD"),
		//		new QuoteSymbol(NAME, "BTC")
		//	};
		//}

		public async Task<List<Ticker>> GetTickers(int limit = 100, int start = 1, QuoteType quote = QuoteType.NONE)
		{
			JToken jObject;
			List<string> param = new List<string>();
			if (limit != 100)
				param.Add($"limit={limit}");
			if (start != 1)
				param.Add($"start={start}");
			if (quote != QuoteType.NONE)
				param.Add($"convert={QuoteToString(quote)}");

			try
			{
				jObject = await GetJToken("https://api.coinmarketcap.com/v1/ticker" + GetEndpoint(param));
			}
			catch (TimeoutException)
			{
				return await GetTickers(limit, start, quote);
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
		}

		public async Task<Ticker> GetTicker(string id, QuoteType quote = QuoteType.NONE)
		{
			string endpoint = id + "/";

			JToken jObject;
			if (quote != QuoteType.NONE)
				endpoint += $"?convert={QuoteToString(quote)}";
			try
			{
				jObject = await GetJToken("https://api.coinmarketcap.com/v1/ticker/" + endpoint);
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

		public async Task<GlobalData> GetGlobalData(QuoteType quote = QuoteType.NONE)
		{
			string endpoint = "";
			JObject jObject;

			if (quote != QuoteType.NONE)
				endpoint += $"?convert={QuoteToString(quote)}";
			try
			{
				jObject = await GetJObject("https://api.coinmarketcap.com/v1/global/" + endpoint);
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

		//protected override void CandleSubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, int length, Action<object[]> action) { }
		//protected override void CandleUnsubscribe(string baseSymbol, string quoteSymbol, Timeframe timeframe, Action<object[]> action) { }
		//protected override void PriceUnsubscribe(string baseSymbol, string quoteSymbol, Action<object[]> action)
  //      {
		//	string key = $"{NAME};{baseSymbol};{quoteSymbol}";
		//	priceSubscription.RemoveEvent(key, action);
		//	if(!priceSubscription.ContainsKey(key))
		//		priceSubscription.Remove(key);
  //      }

  //      protected override void PriceSubscribe(string baseSymbol, string quoteSymbol, Action<object[]> action)
  //      {
		//	string key = $"{NAME};{baseSymbol};{quoteSymbol}";

		//	if (!priceSubscription.ContainsKey(key))
  //              priceSubscription.CreateEventForKey(key);
  //          priceSubscription.EventForKey(key).Add(action);
  //          foreach (var t in _tickerList)
  //          {
  //              if (t.symbol == baseSymbol)
  //              {
  //                  if (quoteSymbol == "USD")
  //                      priceSubscription.OnEventForKey(NAME + baseSymbol + quoteSymbol, new object[] { t.price_usd });
  //                  else
  //                      priceSubscription.OnEventForKey(NAME + baseSymbol + quoteSymbol, new object[] { t.price_btc });
  //              }
  //          }
  //      }

        private void StartingTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Minute % 5 == 0)
            {
                _startingTimer.Enabled = false;
                _timer.Enabled = true;
                Timer_Tick(null, null);
            }
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine($"{DateTime.Now} downloading tickers.");
            _tickerList = await GetTickers(0);
            Console.WriteLine($"{DateTime.Now} finished downloading tickers.");

            foreach (var t in _tickerList)
            {
                if (priceSubscription.ContainsKey(NAME + t.symbol + "USD"))
                    priceSubscription.OnEventForKey(NAME + t.name + t.symbol, new PriceUpdate() { Price = float.Parse(t.price_usd) });
                if (priceSubscription.ContainsKey(NAME + t.symbol + "BTC"))
                    priceSubscription.OnEventForKey(NAME + t.name + t.symbol, new PriceUpdate() { Price = float.Parse(t.price_btc) });
            }
        }

        private async static Task WaitForInit()
        {
            if (_tickerList == null)
            {
                await Task.Factory.StartNew(() =>
                {
                    while (_tickerList == null)
                        Thread.Sleep(10);
                });
            }
        }
    }

    public enum QuoteType { NONE, AUD, BRL, CAD, CHF, CLP, CNY, CZK, DKK, EUR, GBP, HKD, HUF, IDR, ILS, INR, JPY, KRW, MXN, MYR, NOK, NZD, PHP, PKR, PLN, RUB, SEK, SGD, THB, TRY, TWD, ZAR }

    public class Ticker
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

    public class GlobalData
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




