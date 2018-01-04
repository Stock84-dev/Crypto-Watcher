// Copyright(c) 2017 Stock84-dev
// https://github.com/Stock84-dev/Cryptowatch-API

using System.Collections.Generic;
using Newtonsoft.Json;

namespace APIs
{
	public class SiteInformation
	{
		public string revision { get; set; }
		public string uptime { get; set; }
		public string documentation { get; set; }
		public string[] indexes { get; set; }
	}

	/// <summary>
	/// An asset can be a crypto or fiat currency. 
	/// </summary>
	public class Assets
	{
		public int id { get; set; }
		public string symbol { get; set; }
		public string name { get; set; }
		public bool fiat { get; set; }
		public string route { get; set; }
	}

	/// <summary>
	/// An asset can be a crypto or fiat currency. 
	/// </summary>
	public class Asset
	{
		public int id { get; set; }
		public string symbol { get; set; }
		public string name { get; set; }
		public bool fiat { get; set; }
		public Markets1 markets { get; set; }
	}

	/// <summary>
	/// A pair of assets. Each pair has a base and a quote. For example, btceur has base btc and quote eur.
	/// </summary>
	public class Pairs
	{
		public string symbol { get; set; }
		public int id { get; set; }
		[JsonProperty("base")]
		public Base basePair { get; set; }
		[JsonProperty("quote")]
		public Quote quotePair { get; set; }
		public string route { get; set; }
		/// <summary>
		/// Not always set.
		/// </summary>
		public string futuresContractPeriod { get; set; }
	}

	public class Pair
	{
		public string symbol { get; set; }
		public int id { get; set; }
		[JsonProperty("base")]
		public Base basePair { get; set; }
		[JsonProperty("quote")]
		public Quote quotePair { get; set; }
		public string route { get; set; }
		public Markets[] markets { get; set; }
	}

	/// <summary>
	/// Exchanges are where all the action happens!
	/// </summary>
	public class Exchanges
	{
		public string symbol { get; set; }
		public string name { get; set; }
		public string route { get; set; }
		public bool active { get; set; }
	}

	/// <summary>
	/// Exchanges are where all the action happens!
	/// </summary>
	public class Exchange
	{
		public string symbol { get; set; }
		public string name { get; set; }
		public bool active { get; set; }
		public Route routes { get; set; }
	}

	/// <summary>
	/// A market is a pair listed on an exchange. For example, pair btceur on exchange kraken is a market.
	/// </summary>
	public class Markets
	{
		public int id { get; set; }
		public string exchange { get; set; }
		public string pair { get; set; }
		public bool active { get; set; }
		public string route { get; set; }
	}

	/// <summary>
	/// A market is a pair listed on an exchange. For example, pair btceur on exchange kraken is a market.
	/// </summary>
	public class Market
	{
		public string exchange { get; set; }
		public string pair { get; set; }
		public bool active { get; set; }
		public Routes routes { get; set; }
	}

	public class Markets1
	{
		[JsonProperty("base")]
		public Bases[] baseMarket { get; set; }
		[JsonProperty("quote")]
		public Quotes[] quoteMarket { get; set; }
	}

	public class Bases
	{
		public int id { get; set; }
		public string exchange { get; set; }
		public string pair { get; set; }
		public bool active { get; set; }
		public string route { get; set; }
	}

	public class Base
	{
		public int id { get; set; }
		public string route { get; set; }
		public string symbol { get; set; }
		public string name { get; set; }
		public bool fiat { get; set; }
	}

	public class Quotes
	{
		public int id { get; set; }
		public string exchange { get; set; }
		public string pair { get; set; }
		public bool active { get; set; }
		public string route { get; set; }
	}

	public class Quote
	{
		public int id { get; set; }
		public string route { get; set; }
		public string symbol { get; set; }
		public string name { get; set; }
		public bool fiat { get; set; }
	}

	public class Routes
	{
		public string price { get; set; }
		public string summary { get; set; }
		public string orderbook { get; set; }
		public string trades { get; set; }
		public string ohlc { get; set; }
	}

	public class Route
	{
		public string markets { get; set; }
	}

	public class Summary
	{
		public Price price { get; set; }
		public float volume { get; set; }
	}

	public class Price
	{
		public float last { get; set; }
		public float high { get; set; }
		public float low { get; set; }
		public Change change { get; set; }
	}

	public class Change
	{
		public float percentage { get; set; }
		public float absolute { get; set; }
	}

	public class Trade
	{
		public int id { get; set; }
		public long timestamp { get; set; }
		public float price { get; set; }
		public float amount { get; set; }

		public Trade(int id, long timestamp, float price, float amount)
		{
			this.id = id;
			this.timestamp = timestamp;
			this.price = price;
			this.amount = amount;
		}
	}

	public class OrderBook
	{
		public List<Order> bids { get; set; }
		public List<Order> asks { get; set; }

		public OrderBook()
		{
			bids = new List<Order>();
			asks = new List<Order>();
		}
	}

	public class Order
	{
		public float price;
		public float amount;

		public Order(float price, float amount)
		{
			this.price = price;
			this.amount = amount;
		}
	}

	public enum TimeFrame { min1 = 60, min3 = 180, min5 = 300, min15 = 900, min30 = 1800, h1 = 3600, h2 = 7200, h4 = 14400, h6 = 21600, h12 = 43200, d1 = 86400, d3 = 259200, w1 = 604800 }

	public class Candlestick
	{
		public long closeTime;
		public float openPrice;
		public float highPrice;
		public float lowPrice;
		public float closePrice;
		public float volume;

		public Candlestick(long closeTime, float openPrice, float highPrice, float lowPrice, float closePrice, float volume)
		{
			this.closeTime = closeTime;
			this.openPrice = openPrice;
			this.highPrice = highPrice;
			this.lowPrice = lowPrice;
			this.closePrice = closePrice;
			this.volume = volume;
		}
	}

	public class Allowance
	{
		public long cost { get; set; }
		public long remaining { get; set; } = 8000000000;
	}
}
