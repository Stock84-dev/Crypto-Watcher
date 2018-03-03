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
using System.Threading;
using System.Diagnostics;
using CryptoWatcher.Alerts;
using CryptoWatcher.APIs;

namespace CryptoWatcher
{
	// TODO: implement network conection indicator and no network exception
	// TODO: when binance is selected notify user that we compare symbol not name (BCC BitConnect = BCC Bitcoin Cash on binance)
	public partial class CustomAlertForm : MetroFramework.Forms.MetroForm
	{
		string baseName;
		string baseSymbol;
		public static MainForm MainForm { get; set; }
		//List<Exchange> exchanges;
		// these are only needed when price needs to be converted
		//Dictionary<int, string> middleMarket;

		public CustomAlertForm()
		{
			InitializeComponent();
			PopulateSymbols();
			PopulateConditions();
		}

		private async void PopulateSymbols()
		{
            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
            // populating autocomplete list in symbol textbox
            foreach (var ticker in await CoinMarketCapAPI.GetTickerList())
                acsc.Add(ticker.symbol + " " + ticker.name);
            txtSymbol.AutoCompleteCustomSource = acsc;
        }

		private void PopulateConditions()
		{
			cboxCondition.Items.Add(new AlertItem(new PriceAlert()));
			cboxCondition.Items.Add(new AlertItem(new RSIAlert()));
			cboxCondition.Items.Add(new AlertItem(new StochAlert()));
			cboxCondition.Items.Add(new AlertItem(new MACDAlert()));
		}

		private void CustomAlertForm_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
			{
				tabControl.SelectedTab = tabCondition;
				baseName = null;
				baseSymbol = null;
				txtSymbol.Text = "";
				cboxCondition.SelectedIndex = -1;
				while (cboxMarket.Items.Count > 0)
					cboxMarket.Items.RemoveAt(0);
				while (cboxExchange.Items.Count > 0)
					cboxExchange.Items.RemoveAt(0);
				cBoxSound.SelectedIndex = 0;
				chcBoxWindowsMsg.Checked = true;
				chcBoxShowWindow.Checked = true;
				chcBoxRepeatable.Checked = false;
				txtInterval.Text = "";
				cBoxInterval.SelectedIndex = 0;
				lblError.Text = "";
				pBox5Min.Visible = false;
			}
		}

		// evolution of lambda expressions
		//private delegate bool TxtSymbolContains_d(string str);
		//private bool TxtSymbolContains(string str)
		//{
		//	if (txtSymbol.Text.Contains(str))
		//		return true;
		//	return false;
		//}
		// 1.
		//txtSymbol.Invoke(new TxtSymbolContains_d(TxtSymbolContains), a.symbol.ToUpper());
		//2.
		//TxtSymbolContains_d method = delegate (string str) { return txtSymbol.Text.Contains(str) ? true : false; };
		//txtSymbol.Invoke(method, a.symbol.ToUpper());
		//3.
		//TxtSymbolContains_d method = str => { return txtSymbol.Text.Contains(str) ? true : false; };
		//txtSymbol.Invoke(method, a.symbol.ToUpper());
		//4.
		//Func<string, bool> func = str => { return txtSymbol.Text.Contains(str) ? true : false; };
		//txtSymbol.Invoke(func, a.symbol.ToUpper());
		//5.
		//txtSymbol.Invoke(new Func<string, bool>(str => { return txtSymbol.Text.Contains(str) ? true : false; }), a.symbol.ToUpper());

		private async void txtSymbol_LeaveAsync(object sender, EventArgs e)
		{
            //if(!txtSymbol.Text.Contains(" ["))
            //{
            //	lblError.Text = "Asset not found!";
            //	lblError.Visible = true;
            //	return;
            //}
            //baseName = txtSymbol.Text.Substring(0, txtSymbol.Text.IndexOf(" ["));
            //baseSymbol = txtSymbol.Text.Substring(txtSymbol.Text.IndexOf("[") + 1, txtSymbol.Text.IndexOf("]") - txtSymbol.Text.IndexOf("[") - 1);
            bool coinFound = false;
			if(txtSymbol.Text.IndexOf(" ") != -1)
			{
				baseSymbol = txtSymbol.Text.Substring(0, txtSymbol.Text.IndexOf(" "));
				baseName = txtSymbol.Text.Substring(txtSymbol.Text.IndexOf(" ") + 1);
				foreach (var ticker in await CoinMarketCapAPI.GetTickerList())
				{
					if (ticker.name == baseName && ticker.symbol == baseSymbol)
					{
						coinFound = true;
						break;
					}
				}
			}
            if(!coinFound)
            {
				lblError.Text = "Asset not found!";
				lblError.Visible = true;
				return;
			}
			else
			{
				cboxMarket.Items.AddRange(AbstractAPI.GetQuoteItems(baseName, baseSymbol).ToArray());
				lblError.Text = "";
				lblError.Visible = false;
			}
		}

		void SymbolChanged()
		{
			lblError.Text = "";

			// removing selected items because user changed coin name
			cboxExchange.Items.Clear();
			cboxMarket.Items.Clear();
			if (pBox5Min.Visible)
				pBox5Min.Visible = false;
		}

		// changes panel to customize alert
		private void cboxCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(tabCondition.Controls.ContainsKey("panel"))
				tabCondition.Controls.RemoveByKey("panel");
			if (cboxCondition.SelectedItem != null)
				tabCondition.Controls.Add(((AlertItem)cboxCondition.SelectedItem).abstractAlert.GetOptions());
		}

		private void cboxMarket_LeaveAsync(object sender, EventArgs e)
		{
			if (cboxMarket.SelectedItem != null)
				cboxExchange.Items.AddRange(((QuoteItem)cboxMarket.SelectedItem).Exchanges.ToArray());
		}

		private void btnAddAlert_Click(object sender, EventArgs e)
		{
			AlertData alertData;
			Notification ntfy;
			
			// user input checking
			// creating notification
			if (!GetNotification(out ntfy))
			{
				MessageBox.Show("Something is wrong in interval section.");
				return;
			}

			if (cboxMarket.Items.Count == 0 || cboxExchange.Items.Count == 0)
			{
				MessageBox.Show("Please populate required fields.");
				return;
			}

			alertData = new AlertData(baseSymbol, cboxMarket.Text.ToUpper(), baseName, cboxExchange.Text);

			// value in custom alert is not valid
			if (!((AlertItem)(cboxCondition.SelectedItem)).abstractAlert.Create(alertData, ntfy, (MetroFramework.Controls.MetroPanel) tabCondition.Controls["panel"]))
			{
				return;
			}
			MainForm.AddAlertToListView();
			Close();
		}

		private bool GetNotification(out Notification notification)
		{
			int interval = -1;

			if (chcBoxRepeatable.Checked)
			{
				notification = null;
				// user input checking
				if (!int.TryParse(txtInterval.Text, out interval))
					return false;
				// converting to seconds
				switch (cBoxInterval.Text)
				{
					case "Minutes": interval *= 60; break;
					case "Hours": interval *= 3600; break;
					case "Days": interval *= 86400; break;
					case "Weeks": interval *= 604800; break;
					case "Months": interval *= 18144000; break;
				}
			}
			notification = new Notification((Notification.SoundType)cBoxSound.SelectedIndex, chcBoxWindowsMsg.Checked, chcBoxShowWindow.Checked, interval);
		
			return true;
		}
		// when user tabs to combobox dropdown appears
		private void cboxCondition_Enter(object sender, EventArgs e)
		{
			if (MouseButtons == MouseButtons.None)
				((ComboBox)sender).DroppedDown = true;
		}

		private void cboxMarket_Enter(object sender, EventArgs e)
		{
			if (MouseButtons == MouseButtons.None)
				((ComboBox)sender).DroppedDown = true;
		}

		private void cboxExchange_Enter(object sender, EventArgs e)
		{
			if (MouseButtons == MouseButtons.None)
				((ComboBox)sender).DroppedDown = true;
		}

		private void chcBoxRepeatable_CheckedChanged(object sender, EventArgs e)
		{
			if (chcBoxRepeatable.Checked)
			{
				lblInterval.Visible = true;
				txtInterval.Visible = true;
				cBoxInterval.SelectedIndex = 0;
				cBoxInterval.Visible = true;
			}
			else
			{
				lblInterval.Visible = false;
				txtInterval.Visible = false;
				cBoxInterval.Visible = false;
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Hide();
			e.Cancel = true;
			//base.OnFormClosing(e);
		}

		private class AlertItem
		{
			public AbstractAlert abstractAlert { get; }

			public AlertItem(AbstractAlert abstractAlert)
			{
				this.abstractAlert = abstractAlert;
			}

			public override string ToString()
			{
				return abstractAlert.Name;
			}
		}

        private class CoinItem
        {
            public CoinItem(string name, string symbol)
            {
                Name = name;
                Symbol = symbol;
            }

            public string Name { get; set; }
            public string Symbol { get; set; }

            public override string ToString()
            {
                return Symbol + " " + Name;
            }
        }
	}
}