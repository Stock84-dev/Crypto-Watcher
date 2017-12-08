/******************************************************************************
 * CRYPTO WATCHER - cryptocurrency alert system that notifies you when certain 
 * cryptocurrency fulfills your condition.
 * Copyright (c) 2017 Stock84-dev
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace Crypto_watcher
{
    public class Coin
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string rank { get; set; }
        public string price_usd { get; set; }
        public string price_btc { get; set; }
        public string _24h_volume_usd { get; set; }
        public string market_cap_usd { get; set; }
        public string available_supply { get; set; }
        public string total_supply { get; set; }
        public string max_supply { get; set; }
        public string percent_change_1h { get; set; }
        public string percent_change_24h { get; set; }
        public string percent_change_7d { get; set; }
        public string last_updated { get; set; }

        // these objects will not get serialized
        [ScriptIgnore]
        public static List<Coin> coins = new List<Coin>();

        public static Coin Update(string coin_API_url)
        {
            // we have to store it in list, because serializer requires it, we are calling this for one coin 
            return Request(coin_API_url).Last();
        }

        private static List<Coin> Request(string url)
        {
            List<Coin> tmp_coin;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = "GET";
            //httpWebRequest.Headers.Add("Authorization", "Basic reallylongstring");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            // deserializing response that is in JSON format to list of coins
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                tmp_coin = (List<Coin>)javaScriptSerializer.Deserialize(streamReader.ReadToEnd(), typeof(List<Coin>));
                return tmp_coin;
            }
        }

        public static void UpdateCoins()
        {
            coins.Clear();
            // downloading all coins
            coins = Request("https://api.coinmarketcap.com/v1/ticker/?limit=0");
        }

        public static Coin SearchForCoin(string symbol)
        {
            foreach (var coin in coins)
            {
                if (coin.symbol == symbol.ToUpper())
                    return coin;
            }
            return null;
        }
    }

}
