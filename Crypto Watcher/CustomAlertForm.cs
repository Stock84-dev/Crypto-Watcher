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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Crypto_watcher
{
    public partial class CustomAlertForm : MetroFramework.Forms.MetroForm
    {
        public bool added_alert = false;
        public CustomAlertForm()
        {
            InitializeComponent();
        }

        private void btnAddAlert_Click(object sender, EventArgs e)
        {
            // user input checking
            Coin.UpdateCoins();
            Coin coin = Coin.SearchForCoin(txtSymbol.Text);

            if (coin == null)
            {
                MessageBox.Show("Wrong coin symbol.");
                return;
            }

            float price;
            if (!float.TryParse(txtPrice.Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
            {
                MessageBox.Show("Something is wrong in price section.");
                return;
            }

            ConditionType conditionType = ConditionType.higher;
            switch (cboxCondition.Text)
            {
                case "Higher than":
                    conditionType = ConditionType.higher;
                    break;
                case "Lower than":
                    conditionType = ConditionType.lower;
                    break;
            }

            MarketType priceType;
            if (cboxBtcPrice.Checked)
                priceType = MarketType.BTC;
            else priceType = MarketType.USD;
           
            // creating alert
            Alert.alerts.Add(new Alert("https://api.coinmarketcap.com/v1/ticker/" + coin.id, conditionType, price, priceType, coin));
            added_alert = true;
            Alert.Save();
            Close();
        }
    }
}
