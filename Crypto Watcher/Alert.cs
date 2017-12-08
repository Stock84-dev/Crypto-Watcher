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
using System.Web.Script.Serialization;
using System.IO;
using System.Windows.Forms;

namespace Crypto_watcher
{
    class Alert
    {
        public static List<Alert> alerts = new List<Alert>();
        public string coin_API_url;
        public Condition condition;
        public string site = "www.coinmarketcap.com";// TODO: make dynamic
        public Coin coin;
        static string save_path = "Saves/Save.txt";

        public Alert() { }

        public Alert(string coin_url, ConditionType condition_type, float value, MarketType market_type, Coin current_coin_value)
        {
            coin_API_url = coin_url;
            condition = new Condition(condition_type, value, market_type);
            coin = current_coin_value;
        }

        public string[] ToRow()
        {
            return new string[]{ coin.name,
                condition.ToString(),
                condition.value.ToString(),
                condition.market_type == (int)MarketType.USD ? "USD" : "BTC",
                site
            };
        }

        public bool Test()
        {
            coin = Coin.Update(coin_API_url);

            if (condition.Test(coin))
                return true;

            return false;
        }

        public static void Save()
        {
            // creating filestream that can write a file
            FileStream fs = null;
            try
            {
                fs = new FileStream(save_path, FileMode.Create, FileAccess.Write);
                // if we don't have permission to write we exit function
                if (!fs.CanWrite)
                    return;
                // creating JSON format from alerts list
                string str_alerts = new JavaScriptSerializer().Serialize(alerts);
                // converting string to byte array
                byte[] buffer = Encoding.ASCII.GetBytes(str_alerts);
                // writing whole buffer array
                fs.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // closing filestream
                fs.Flush();
                fs.Close();
            }
        }

        public static void Load()
        {
            if (!File.Exists(save_path))
                return;
            // creating reading filestream
            FileStream fs = null;
            try
            {
                fs = new FileStream(save_path, FileMode.OpenOrCreate, FileAccess.Read);
                // if we don't have permission to read we exit
                if (!fs.CanRead)
                    return;

                byte[] buffer = new byte[2048];// TODO: possible bug here (not enough space)
                int bytesRead;
                // reading 1024 bytes from file
                bytesRead = fs.Read(buffer, 0, buffer.Length);
                // creating list of alert by decoding bytes to string and then deserializing JSON format to object
                alerts = (List<Alert>)new JavaScriptSerializer().Deserialize(Encoding.ASCII.GetString(buffer, 0, bytesRead), typeof(List<Alert>));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // closing filestream
                fs.Flush();
                fs.Close();
            }
        }



    }

}
