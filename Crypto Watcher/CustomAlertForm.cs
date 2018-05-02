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
using CryptoWatcher.Alert;
using CryptoWatcher.APIs;
using MetroFramework.Controls;

namespace CryptoWatcher
{
	// TODO: implement network conection indicator and no network exception
	public partial class CustomAlertForm : MetroFramework.Forms.MetroForm
	{
		private Dictionary<string, List<string>> _exchangesAndQuotes;
		private string _baseName;
		private string _baseSymbol;
		private int _editedAlertId = -1;
		private Task<List<CryptoCompareAPI.Coin>> _coinList;
		private Task _populateMarketsTask;

		public CustomAlertForm()
		{
			_coinList = AbstractAPI.CryptoCompareAPI.GetCoinList();
			PopulateSymbols();
			InitializeComponent();
			cboxCondition.Items.AddRange(AbstractAlert.CreateChildClasses());
			AbstractAlert.OnSubscribeFail += AbstractAlert_OnSubscribeFail;
		}

		public static MainForm MainForm { get; set; }

		public async void ShowEdit(int alertId)
		{
			_editedAlertId = alertId;
			Show();

			txtSymbol.Text = AbstractAlert.Alerts[alertId].AlertData.BaseSymbol + " " + AbstractAlert.Alerts[alertId].AlertData.BaseName;
			await _populateMarketsTask;
			cboxCondition.SelectedIndex = cboxCondition.FindStringExact(AbstractAlert.Alerts[alertId].Name);
			cboxMarket.SelectedIndex = cboxMarket.FindStringExact(AbstractAlert.Alerts[alertId].AlertData.QuoteSymbol);
			cboxExchange.SelectedIndex = cboxExchange.FindStringExact(AbstractAlert.Alerts[alertId].AlertData.ExchangeName);

			cBoxSound.SelectedIndex = cBoxSound.FindStringExact(Notification.SoundTypeToString(AbstractAlert.Alerts[alertId].Notification.Sound));
			if(AbstractAlert.Alerts[alertId].Notification.Interval != -1)
			{
				txtInterval.Text = AbstractAlert.Alerts[alertId].Notification.Interval.ToString();
				chcBoxRepeatable.Checked = true;
			}
			chcBoxShowWindow.Checked = AbstractAlert.Alerts[alertId].Notification.ShowWindow;
			chcBoxWindowsMsg.Checked = AbstractAlert.Alerts[alertId].Notification.ShowWindowsMsg;


			if (tabCondition.Controls.ContainsKey("panel"))
				tabCondition.Controls.RemoveByKey("panel");
			tabCondition.Controls.Add(AbstractAlert.Alerts[alertId].GetFilledUI());
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Hide();
			e.Cancel = true;
			if (_editedAlertId != -1)
				AbstractAlert.Alerts[_editedAlertId].Subscribe();
		}

		private async void PopulateSymbols()
		{
			AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
			// populating autocomplete list in symbol textbox
			await _coinList;
			if (_coinList.Result == null)
				return;
            foreach (var coin in _coinList.Result)
                acsc.Add(coin.Symbol + " " + coin.CoinName);
            txtSymbol.AutoCompleteCustomSource = acsc;
        }

		private void CustomAlertForm_VisibleChanged(object sender, EventArgs e)
		{
			// changing controls to default state
			if (Visible)
			{
				tabControl.SelectedTab = tabCondition;
				_baseName = null;
				_baseSymbol = null;
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
				if (_editedAlertId == -1)
					ActiveControl = txtSymbol;
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

		private void txtSymbol_TextChanged(object sender, EventArgs e)
		{
			_populateMarketsTask = PopulateMarkets();
		}

		private async Task PopulateMarkets()
		{
			bool coinFound = false;
			if (txtSymbol.Text.IndexOf(" ") != -1)
			{
				_baseSymbol = txtSymbol.Text.Substring(0, txtSymbol.Text.IndexOf(" "));
				_baseName = txtSymbol.Text.Substring(txtSymbol.Text.IndexOf(" ") + 1);
				await _coinList;
				if (_coinList.Result != null)
				{
					foreach (var coin in _coinList.Result)
					{
						if (coin.CoinName == _baseName && coin.Symbol == _baseSymbol)
						{
							coinFound = true;
							break;
						}
					}
				}
			}
			if (!coinFound)
			{
				lblError.Text = "Asset not found!";
				return;
			}
			else
			{
				_exchangesAndQuotes = ((await (AbstractAPI.CryptoCompareAPI).GetQuotesAndExchanges(_baseSymbol)));
				cboxMarket.Items.Clear();
				if (_exchangesAndQuotes == null)
					return;
				cboxMarket.Items.AddRange(_exchangesAndQuotes.Keys.OrderBy(x => x).ToArray());
				cboxMarket.SelectedIndex = 0;
				PopulateExchanges();
				lblError.Text = "";
			}
		}

		private void SymbolChanged()
		{
			lblError.Text = "";

			// removing selected items because user changed coin name
			cboxExchange.Items.Clear();
			cboxMarket.Items.Clear();
		}

		// changes panel to customize alert
		private void cboxCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(tabCondition.Controls.ContainsKey("panel"))
				tabCondition.Controls.RemoveByKey("panel");
			if (cboxCondition.SelectedItem != null)
				tabCondition.Controls.Add(((AbstractAlert)cboxCondition.SelectedItem).GetUI());
		}

		private void cboxMarket_SelectedIndexChanged(object sender, EventArgs e)
		{
			PopulateExchanges();
		}

		private void PopulateExchanges()
		{
			cboxExchange.Items.Clear();
			cboxExchange.Items.AddRange(_exchangesAndQuotes[cboxMarket.Text].OrderBy(x => x).ToArray());
			cboxExchange.SelectedIndex = cboxExchange.FindStringExact("Aggregate");
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

			alertData = new AlertData(_baseSymbol, cboxMarket.Text.ToUpper(), _baseName, cboxExchange.Text);

			if (_editedAlertId != -1)
			{
				AbstractAlert.Alerts[_editedAlertId].Unsubscribe();
				AbstractAlert.Alerts[_editedAlertId].StopNotifying(false, true);
				MainForm.RemoveNotification(_editedAlertId);
				// Alert type changed
				if (AbstractAlert.Alerts[_editedAlertId].GetType() != ((AbstractAlert)cboxCondition.SelectedItem).GetType())
				{
					if (!AbstractAlert.Alerts[_editedAlertId].CreateAlert(alertData, ntfy, (MetroPanel)tabCondition.Controls["panel"], true, (AbstractAlert)cboxCondition.SelectedItem))
					{
						_editedAlertId = -1;
						return;
					}
					AbstractAlert.Alerts[_editedAlertId] = (AbstractAlert)cboxCondition.SelectedItem;
				}
				if (!AbstractAlert.Alerts[_editedAlertId].CreateAlert(alertData, ntfy, (MetroPanel)tabCondition.Controls["panel"], true))
				{
					_editedAlertId = -1;
					return;
				}
			}
			else
			{
				// value in custom alert is not valid
				if (!((AbstractAlert)(cboxCondition.SelectedItem)).CreateAlert(alertData, ntfy, (MetroPanel)tabCondition.Controls["panel"]))
				{
					return;
				}
			}

			MainForm.AddAlertToGridView(_editedAlertId);
			_editedAlertId = -1;
			// creating new alert to replace the old one
			cboxCondition.Items[cboxCondition.SelectedIndex] = AbstractAlert.CreateChildClass(((AbstractAlert)cboxCondition.SelectedItem).GetType());
			Close();
		}

		private void AbstractAlert_OnSubscribeFail(object sender, EventArgs e)
		{
			MessageBox.Show("Not enough data on the server consider lowering timeframe.");
			ShowEdit(AbstractAlert.Alerts.IndexOf((AbstractAlert)sender));
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
	}
}