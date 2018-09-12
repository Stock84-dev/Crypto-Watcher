///******************************************************************************
// * CRYPTO WATCHER - cryptocurrency alert system that notifies you when certain 
// * cryptocurrency fulfills your condition.
// * Copyright (c) 2017-2018 Stock84-dev
// * https://github.com/Stock84-dev/Crypto-Watcher
// *
// * This file is part of CRYPTO WATCHER.
// *
// * CRYPTO WATCHER is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * CRYPTO WATCHER is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with CRYPTO WATCHER.  If not, see <http://www.gnu.org/licenses/>.
// *****************************************************************************/

//using System.Collections.Generic;
//using Newtonsoft.Json;
//using System;

//namespace CryptoWatcher.APIs
//{
//	public class SiteInformation
//	{
//		public string revision { get; set; }
//		public string uptime { get; set; }
//		public string documentation { get; set; }
//		public string[] indexes { get; set; }
//	}

//	/// <summary>
//	/// An asset can be a crypto or fiat currency. 
//	/// </summary>
//	public class Assets
//	{
//		public int id { get; set; }
//		public string symbol { get; set; }
//		public string name { get; set; }
//		public bool fiat { get; set; }
//		public string route { get; set; }
//	}

//	/// <summary>
//	/// An asset can be a crypto or fiat currency. 
//	/// </summary>
//	public class Asset
//	{
//		public int id { get; set; }
//		public string symbol { get; set; }
//		public string name { get; set; }
//		public bool fiat { get; set; }
//		public Markets1 markets { get; set; }
//	}

//	/// <summary>
//	/// A pair of assets. Each pair has a base and a quote. For example, btceur has base btc and quote eur.
//	/// </summary>
//	public class Pairs
//	{
//		public string symbol { get; set; }
//		public int id { get; set; }
//		[JsonProperty("base")]
//		public Base basePair { get; set; }
//		[JsonProperty("quote")]
//		public Quote quotePair { get; set; }
//		public string route { get; set; }
//		/// <summary>
//		/// Not always set.
//		/// </summary>
//		public string futuresContractPeriod { get; set; }
//	}

//	public class Pair
//	{
//		public string symbol { get; set; }
//		public int id { get; set; }
//		[JsonProperty("base")]
//		public Base basePair { get; set; }
//		[JsonProperty("quote")]
//		public Quote quotePair { get; set; }
//		public string route { get; set; }
//		public Markets[] markets { get; set; }
//	}

//	/// <summary>
//	/// Exchanges are where all the action happens!
//	/// </summary>
//	public class Exchanges
//	{
//		public string symbol { get; set; }
//		public string name { get; set; }
//		public string route { get; set; }
//		public bool active { get; set; }
//	}

//	/// <summary>
//	/// Exchanges are where all the action happens!
//	/// </summary>
//	public class Exchange
//	{
//		public string symbol { get; set; }
//		public string name { get; set; }
//		public bool active { get; set; }
//		public Route routes { get; set; }
//	}

//	/// <summary>
//	/// A market is a pair listed on an exchange. For example, pair btceur on exchange kraken is a market.
//	/// </summary>
//	public class Markets
//	{
//		public int id { get; set; }
//		public string exchange { get; set; }
//		public string pair { get; set; }
//		public bool active { get; set; }
//		public string route { get; set; }
//	}

//	/// <summary>
//	/// A market is a pair listed on an exchange. For example, pair btceur on exchange kraken is a market.
//	/// </summary>
//	public class Market
//	{
//		public string exchange { get; set; }
//		public string pair { get; set; }
//		public bool active { get; set; }
//		public Routes routes { get; set; }
//	}

//	public class Markets1
//	{
//		[JsonProperty("base")]
//		public Bases[] baseMarket { get; set; }
//		[JsonProperty("quote")]
//		public Quotes[] quoteMarket { get; set; }
//	}

//	public class Bases
//	{
//		public int id { get; set; }
//		public string exchange { get; set; }
//		public string pair { get; set; }
//		public bool active { get; set; }
//		public string route { get; set; }
//	}

//	public class Base
//	{
//		public int id { get; set; }
//		public string route { get; set; }
//		public string symbol { get; set; }
//		public string name { get; set; }
//		public bool fiat { get; set; }
//	}

//	public class Quotes
//	{
//		public int id { get; set; }
//		public string exchange { get; set; }
//		public string pair { get; set; }
//		public bool active { get; set; }
//		public string route { get; set; }
//	}

//	public class Quote
//	{
//		public int id { get; set; }
//		public string route { get; set; }
//		public string symbol { get; set; }
//		public string name { get; set; }
//		public bool fiat { get; set; }
//	}

//	public class Routes
//	{
//		public string price { get; set; }
//		public string summary { get; set; }
//		public string orderbook { get; set; }
//		public string trades { get; set; }
//		public string ohlc { get; set; }
//	}

//	public class Route
//	{
//		public string markets { get; set; }
//	}

//	public class Summary
//	{
//		public Price price { get; set; }
//		public float volume { get; set; }
//	}

//	public class Price
//	{
//		public float last { get; set; }
//		public float high { get; set; }
//		public float low { get; set; }
//		public Change change { get; set; }
//	}

//	public class Change
//	{
//		public float percentage { get; set; }
//		public float absolute { get; set; }
//	}

//	public class Trade
//	{
//		public int id { get; set; }
//		public long timestamp { get; set; }
//		public float price { get; set; }
//		public float amount { get; set; }

//		public Trade(int id, long timestamp, float price, float amount)
//		{
//			this.id = id;
//			this.timestamp = timestamp;
//			this.price = price;
//			this.amount = amount;
//		}
//	}

//	public class OrderBook
//	{
//		public List<Order> bids { get; set; }
//		public List<Order> asks { get; set; }

//		public OrderBook()
//		{
//			bids = new List<Order>();
//			asks = new List<Order>();
//		}
//	}

//	public class Order
//	{
//		public float price;
//		public float amount;

//		public Order(float price, float amount)
//		{
//			this.price = price;
//			this.amount = amount;
//		}
//	}

	

//	public class Allowance
//	{
//		public long cost { get; set; }
//		public long remaining { get; set; } = 8000000000;
//	}

//	public class AllowanceEventArgs : EventArgs
//	{
//		public Allowance allowance { get; set; }
//		public AllowanceEventArgs(Allowance allowance)
//		{
//			this.allowance = allowance;
//		}
//	}
//}
