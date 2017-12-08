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
using System.Globalization;

namespace Crypto_watcher
{
    public enum MarketType { USD, BTC }
    public enum ConditionType { higher, lower }

    public class Condition
    {
        public float value;
        public int market_type;
        public int condition_type;

        public Condition() { }

        public Condition(ConditionType condition_type, float value, MarketType market_type)
        {
            this.value = value;
            this.market_type = (int)market_type;
            this.condition_type = (int)condition_type;
        }

        public bool Test(Coin coin)
        {
            float current_value;

            switch (market_type)
            {
                case (int)MarketType.USD:
                    current_value = float.Parse(coin.price_usd, CultureInfo.InvariantCulture);
                    break;
                case (int)MarketType.BTC:
                    current_value = float.Parse(coin.price_btc, CultureInfo.InvariantCulture);
                    break;
                default:
                    current_value = -1;
                    break;
            }

            switch (condition_type)
            {
                case (int)ConditionType.higher:
                    if(current_value>value)
                        return true;
                    break;
                case (int)ConditionType.lower:
                    if (current_value < value)
                        return true;
                    break;
                default:
                    return false;
            }

            return false;
        }

        public override string ToString()
        {
            string strConditionType;

            switch (condition_type)
            {
                case (int)ConditionType.higher:
                    strConditionType = "higher than ";
                    break;
                case (int)ConditionType.lower:
                    strConditionType = "lower than ";
                    break;
                default:
                    strConditionType = "NO CONDITION ";
                    break;
            }
            
            return strConditionType;
        }
    }
}
        

   
