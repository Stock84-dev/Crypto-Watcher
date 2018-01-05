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
using APIs;
using Alerts;
using System.Threading;
using System.Diagnostics;

namespace CryptoWatcher
{
	// TODO: implement network conection indicator and no network exception
	public partial class CustomAlertForm : MetroFramework.Forms.MetroForm
	{
		string baseName;
		string baseSymbol;
		bool marketIsRunning;
		public static MainForm MainForm { get; set; }
		//List<Exchange> exchanges;
		// these are only needed when price needs to be converted
		//Dictionary<int, string> middleMarket;

		public CustomAlertForm()
		{
			InitializeComponent();
			
			PopulateTickersAsync();
			if (Alert.AssetList == null)
				PopulateAssetsAsync();
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
				txtPrice.Text = "";
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
				marketIsRunning = false;
			}
		}

		private async void PopulateAssetsAsync()
		{
			Alert.AssetList = await CryptowatchAPI.GetAssets();
		}

		async void PopulateTickersAsync()
		{
			if(Alert.TickerList == null)
			{
				await Alert.IsTickerListDone();
			}
			if(txtSymbol.AutoCompleteCustomSource.Count == 0)
			{
				AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
				// populating autocomplete list in symbol textbox
				foreach (var ticker in Alert.TickerList)
					acsc.Add(ticker.symbol.ToUpper() + " " + ticker.name);

				txtSymbol.AutoCompleteCustomSource = acsc;
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
			await LoadMarkets();
			
			await LoadExchanges(false);
			//await task.ContinueWith(task => LoadExchanges(source.Token));
		}

		private async Task LoadMarkets()
		{
			Console.WriteLine("LoadMarkets started");

			// looking with cryptowatch API
			foreach (var a in Alert.AssetList)
			{
				// searching for symbol
				if (txtSymbol.Text == a.symbol.ToUpper() + " " + a.name)
				{
					SymbolChanged();

					Asset asset = await CryptowatchAPI.GetAsset(a.route);
					baseSymbol = a.symbol.ToLower();
					baseName = a.name;

					await AddQuotesToListAsync(asset);
					Console.WriteLine("Load markets finished");
					return;
				}
			}
			// if we got here that means we haven't found symbol in cryptowatch so we load it with coinmarketcap
			foreach (var t in Alert.TickerList)
			{
				if (txtSymbol.Text == t.symbol + " " + t.name)
				{
					SymbolChanged();
					baseSymbol = t.symbol;
					baseName = t.name;
					cboxMarket.Items.Add(new MarketItem("USD", false));
					cboxMarket.Items.Add(new MarketItem("BTC", false));
					cboxMarket.SelectedIndex = 0;
					Console.WriteLine("Load markets finished");
					break;
				}
			}
		}

		void SymbolChanged()
		{
			lblError.Text = "";

			// removing selected items because user changed coin name
			cboxExchange.Items.Clear();
			cboxMarket.Items.Clear();
			txtPrice.Text = "";
			if (pBox5Min.Visible)
				pBox5Min.Visible = false;
		}

		private async Task AddQuotesToListAsync(Asset asset)
		{
			Console.WriteLine("Loading quotes");
			List<Asset> quoteAssets = new List<Asset>();

			// populating market combo box for that asset
			foreach (var m in asset.markets.baseMarket)
			{
				string quote = m.pair.Substring(baseSymbol.Length).ToUpper();

				// ignoring btcusd-weekly-futures pairs
				if (quote.Contains('-'))
					continue;
				MarketItem cq = new MarketItem(quote, true);
				
				if (!IsInCBox(quote))
				{
					cboxMarket.Items.Add(cq);
					cboxMarket.Refresh();
					// auto select
					if (cboxMarket.SelectedIndex == -1)
						cboxMarket.SelectedIndex = 0;
					try
					{
						quoteAssets.Add(await CryptowatchAPI.GetAsset("https://api.cryptowat.ch/assets/" + quote.ToLower()));
					}
					catch
					{
						continue;
					}
				}
			}

			// adding option to convert price to higher quote (e.g. etcbtc to etcusd because btcusd pair exists)
			foreach (var quoteAsset in quoteAssets)
			{
				if (quoteAsset.markets.baseMarket != null)
				{
					AddCalculatedQuotesToList(quoteAsset);
				}
			}
			Console.WriteLine("finishing quotes");

		}

		private bool IsInCBox(string quote)
		{
			foreach (MarketItem item in cboxMarket.Items)
			{
				if(item.Text == quote)
				{
					return true;
				}
			}
			return false;
		}

		private void AddCalculatedQuotesToList(Asset quoteAsset)
		{
			foreach (var qbm in quoteAsset.markets.baseMarket)
			{
				string quoteQuote = qbm.pair.Substring(quoteAsset.symbol.Length).ToUpper() + " (calculated)";
				
				// ignoring btcusd-weekly-futures pairs
				if (quoteQuote.Contains('-'))
					continue;
				MarketItem cq = new MarketItem(quoteQuote, true);
				if (!IsInCBox(quoteQuote))
				{
					cq.MiddleMarket = qbm.exchange + "/" + qbm.pair;
					cboxMarket.Items.Add(cq);
					cboxMarket.Refresh();
					// bitfinex/btcusd
				}
			}
		}

		private async void cboxMarket_LeaveAsync(object sender, EventArgs e)
		{
			marketIsRunning = true;
			await LoadExchanges(true);
			marketIsRunning = false;
		}

		private string GetQuoteSymbol()
		{
			return ((MarketItem)cboxMarket.SelectedItem).MiddleMarket.Substring(((MarketItem)cboxMarket.SelectedItem).MiddleMarket.IndexOf('/') + 1, (((MarketItem)cboxMarket.SelectedItem).MiddleMarket.Substring(((MarketItem)cboxMarket.SelectedItem).MiddleMarket.IndexOf('/') + 1).Length - cboxMarket.Text.Substring(0, cboxMarket.Text.IndexOf(" (calculated)")).Length));
		}

		private async Task LoadExchanges(bool isUserIssued)
		{
			Console.WriteLine("LoadExchanges started");
			if (baseSymbol == null)
			{
				lblError.Text = "Asset not found.";
				return;
			}
			cboxExchange.Items.Clear();
			// there is no exchange in coinmarketcap
			if (!((MarketItem)cboxMarket.SelectedItem).IsInCryptowatch)
			{
				cboxExchange.Items.Add("CoinMarketCap");
				pBox5Min.Visible = true;
				cboxExchange.SelectedIndex = 0;
				return;
			}
			Pair pair;


			// geting middle pair
			if (((MarketItem)cboxMarket.SelectedItem).MiddleMarket != null)
			{
				//bitfinex/btcusd
				pair = await CryptowatchAPI.GetPair("https://api.cryptowat.ch/pairs/" + baseSymbol + GetQuoteSymbol());
			}
			else
			{
				try
				{
					pair = await CryptowatchAPI.GetPair("https://api.cryptowat.ch/pairs/" + baseSymbol + cboxMarket.Text.ToLower());
				}
				catch
				{
					pair = null;
					return;
				}
			}

			// populating exchange combo box
			foreach (var market in pair.markets)
			{
				Exchange exchange = await CryptowatchAPI.GetExchange("https://api.cryptowat.ch/exchanges/" + market.exchange);
				// preventing old method that isn't finished yet
				if (!isUserIssued && marketIsRunning)
					return;
				cboxExchange.Items.Add(new ExchangeItem(exchange));
				// auto select
				if (cboxExchange.SelectedIndex == -1)
					cboxExchange.SelectedIndex = 0;
			}
			Console.WriteLine("LoadExchanges finished");

		}

		private void btnAddAlert_Click(object sender, EventArgs e)
		{
			float triggerPrice;
			string route;
			string endRoute = "";
			AlertData alertData;
			Notification ntfy;
			
			// user input checking
			if (!float.TryParse(txtPrice.Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out triggerPrice))
			{
				MessageBox.Show("Something is wrong in price section.");
				return;
			}
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

			// creating alert data which requires price to convert
			if (((MarketItem)cboxMarket.SelectedItem).MiddleMarket != null)
			{
				string quoteSymbol = GetQuoteSymbol();
				route = "https://api.cryptowat.ch/markets/" + ((ExchangeItem)cboxExchange.SelectedItem).Exchange.symbol + "/" + baseSymbol + quoteSymbol;
				endRoute = "https://api.cryptowat.ch/markets/" + ((MarketItem)cboxMarket.SelectedItem).MiddleMarket;
				alertData = new AlertData(baseSymbol.ToUpper(), cboxMarket.Text.Substring(0, cboxMarket.Text.IndexOf(" (calculated)") + 1), baseName, ((ExchangeItem)cboxExchange.SelectedItem).Exchange.name, route);
			}
			// creating cryptowatch alert data
			else if(((MarketItem)cboxMarket.SelectedItem).IsInCryptowatch)
			{
				route = "https://api.cryptowat.ch/markets/" + ((ExchangeItem)cboxExchange.SelectedItem).Exchange.symbol + "/" + baseSymbol + cboxMarket.Text.ToLower();
				alertData = new AlertData(baseSymbol.ToUpper(), cboxMarket.Text.ToUpper(), baseName, ((ExchangeItem)cboxExchange.SelectedItem).Exchange.name, route);
			}
			// creating coinmarketcap alert data
			else
			{
				alertData = new AlertData(baseSymbol.ToUpper(), cboxMarket.Text.ToUpper(), baseName, "CoinMarketCap", "");
			}

			// creating alert
			switch (cboxCondition.Text)
			{
				case "Higher than":
					Alert.AlertList.Add(new AbsoluteAlert(alertData, ntfy, triggerPrice, AbsoluteAlert.Type.higher, endRoute));
					break;
				case "Higher or equal than":
					Alert.AlertList.Add(new AbsoluteAlert(alertData, ntfy, triggerPrice, AbsoluteAlert.Type.higherOrEqual, endRoute));
					break;
				case "Lower than":
					Alert.AlertList.Add(new AbsoluteAlert(alertData, ntfy, triggerPrice, AbsoluteAlert.Type.lower, endRoute));
					break;
				case "Lower or equal than":
					Alert.AlertList.Add(new AbsoluteAlert(alertData, ntfy, triggerPrice, AbsoluteAlert.Type.lowerOrEqual, endRoute));
					break;
			}
			Alert.SaveAlerts();
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

		
	}

	public class MarketItem
	{
		public string Text { get; set; }
		public bool IsInCryptowatch { get; set; }
		public string MiddleMarket { get; set; } = null;

		public MarketItem(string text, bool isInCryptowatch)
		{
			Text = text;
			IsInCryptowatch = isInCryptowatch;
		}

		public override string ToString()
		{
			return Text;
		}
	}

	public class ExchangeItem
	{
		public Exchange	Exchange { get; set; }

		public ExchangeItem(Exchange exchange)
		{
			Exchange = exchange;
		}

		public override string ToString()
		{
			return Exchange.name;
		}
	}
}
/*


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
using APIs;
using Alerts;
using System.Threading;

namespace CryptoWatcher
{
	// TODO: implement network conection indicator and no notwork exception
	public partial class CustomAlertForm : MetroFramework.Forms.MetroForm
	{
		string baseName;
		string baseSymbol;
		List<Exchange> exchanges = new List<Exchange>();
		// these are only needed when price needs to be converted
		Dictionary<int, string> middleMarket = new Dictionary<int, string>();
		bool isDoubleLayer = false;

		Task task;
		bool isSymbolFound = true;
		CancellationTokenSource source = new CancellationTokenSource();

		public CustomAlertForm()
		{
			Console.WriteLine(DateTime.Now);
			InitializeComponent();
			if (Alert.AssetList == null)
				task = Task.Factory.StartNew(() => PopulateAssetsAsync(source.Token));
			else
			{
				task = Task.Factory.StartNew(new Action(() => { }));
				AssignTxtSymbolAutoComplete();
			}
			Console.WriteLine("Finishing initializer");
			// populating autocomplete list in symbol textbox
			txtPrice.Focus();
			cBoxSound.SelectedIndex = 0;
			tabControl.SelectedTab = tabCondition;
			Console.WriteLine(DateTime.Now);

		}

		private void PopulateAssetsAsync(CancellationToken token)
		{
			Console.WriteLine("Populating assets");
			Alert.AssetList = CryptowatchAPI.GetAssets();

			if (token.IsCancellationRequested) token.ThrowIfCancellationRequested();
			txtSymbol.BeginInvoke(new Action(AssignTxtSymbolAutoComplete));
			Console.WriteLine("Finishing assets");
		}

		void AssignTxtSymbolAutoComplete()
		{
			foreach (var asset in Alert.AssetList)
			{
				txtSymbol.AutoCompleteCustomSource.Add(asset.symbol.ToUpper() + " " + asset.name);
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

		private void txtSymbol_LeaveAsync(object sender, EventArgs e)
		{
			task = task.ContinueWith(t => LoadSymbols(source.Token));
			// you can also return value from previous task and use it in the next one as t1
			//await task.ContinueWith(task => LoadSymbols(source.Token));
			// also loading exchanges
			task = task.ContinueWith(t => LoadExchanges(source.Token));
			//await task.ContinueWith(task => LoadExchanges(source.Token));
			Console.WriteLine("UI working");
		}

		private void LoadSymbols(CancellationToken token)
		{
			Console.WriteLine("LoadSymbols started");
			Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
			foreach (var a in Alert.AssetList)
			{
				if (token.IsCancellationRequested) return;
				// searching for symbol
				if ((bool)txtSymbol.Invoke(new Func<bool>(() => txtSymbol.Text == a.symbol.ToUpper() + " " + a.name)))
				{
					isSymbolFound = true;
					if (token.IsCancellationRequested) return;
					lblError.BeginInvoke(new Action(() =>
					{
						lblError.Text = "";
					}));

					if (token.IsCancellationRequested) return;
					// removing selected items because user changed coin name
					cboxExchange.BeginInvoke(new Action(() =>
					{
						while (cboxExchange.Items.Count > 0)
						{
							cboxExchange.Items.RemoveAt(0);
						}
					}));
					if (token.IsCancellationRequested) return;
					cboxMarket.BeginInvoke(new Action(() =>
					{
						while (cboxMarket.Items.Count > 0)
						{
							cboxMarket.Items.RemoveAt(0);
						}
					}));
					exchanges = new List<Exchange>();
					middleMarket = new Dictionary<int, string>();

					Asset asset = CryptowatchAPI.GetAsset(a.route);
					baseSymbol = a.symbol.ToLower();
					baseName = a.name;

					// exiting on cancelation
					if (!AddQuotesToList(asset, token))
						return;
					Console.WriteLine("LoadSymbols finished");

					return;
				}
			}
			isSymbolFound = false;
		}

		private bool AddQuotesToList(Asset asset, CancellationToken token)
		{
			List<Asset> quoteAssets = new List<Asset>();

			// populating market combo box for that asset
			foreach (var m in asset.markets.baseMarket)
			{
				string quote = m.pair.Substring(baseSymbol.Length).ToUpper();

				// ignoring btcusd-weekly-futures pairs
				if (quote.Contains('-'))
					continue;

				if (token.IsCancellationRequested) return false;
				if ((bool)cboxMarket.Invoke(new Func<bool>(() =>
				{
					if (!cboxMarket.Items.Contains(quote))
					{
						cboxMarket.Items.Add(quote);
						// auto select
						if (cboxMarket.SelectedIndex == -1)
							cboxMarket.SelectedIndex = 0;
						return true;
					}
					return false;
				})))
				{
					try
					{
						quoteAssets.Add(CryptowatchAPI.GetAsset("https://api.cryptowat.ch/assets/" + quote.ToLower()));
					}
					catch
					{
						continue;
					}
				}
			}

			// adding option to convert price to higher quote (e.g. etcbtc to etcusd because btcusd pair exists)
			foreach (var quoteAsset in quoteAssets)
			{
				if (quoteAsset.markets.baseMarket != null)
				{
					if (!AddCalculatedQuotesToList(quoteAsset, token))
						return false;
				}
			}
			return true;
		}

		private bool AddCalculatedQuotesToList(Asset quoteAsset, CancellationToken token)
		{
			foreach (var qbm in quoteAsset.markets.baseMarket)
			{
				string quoteQuote = qbm.pair.Substring(quoteAsset.symbol.Length).ToUpper();

				// ignoring btcusd-weekly-futures pairs
				if (quoteQuote.Contains('-'))
					continue;

				if (token.IsCancellationRequested) return false;
				cboxMarket.BeginInvoke(new Action<string, string, string>((str, exchangeSymbol, pair) =>
				{
					if (!cboxMarket.Items.Contains(str + " (calculated)"))
					{
						cboxMarket.Items.Add(str + " (calculated)");
						// bitfinex/btcusd
						middleMarket.Add(cboxMarket.Items.Count - 1, exchangeSymbol + "/" + pair);
					}
				}), quoteQuote, qbm.exchange, qbm.pair);
			}
			return true;
		}

		private void cboxMarket_LeaveAsync(object sender, EventArgs e)
		{
			task = task.ContinueWith(t => LoadExchanges(source.Token));
		}

		private string GetQuoteSymbol()
		{
			return middleMarket[cboxMarket.SelectedIndex].Substring(middleMarket[cboxMarket.SelectedIndex].IndexOf('/') + 1, (middleMarket[cboxMarket.SelectedIndex].Substring(middleMarket[cboxMarket.SelectedIndex].IndexOf('/') + 1).Length - cboxMarket.Text.Substring(0, cboxMarket.Text.IndexOf(" (calculated)")).Length));
		}

		private void LoadExchanges(CancellationToken token)
		{
			Console.WriteLine("LoadExchanges started");
			Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
			if (!isSymbolFound)
			{
				if (token.IsCancellationRequested) return;
				lblError.BeginInvoke((new Action(() => lblError.Text = "Asset not found.")));
				return;
			}
			Pair pair;

			// removing old list
			if (exchanges.Count != 0)
			{
				if (token.IsCancellationRequested) return;
				cboxExchange.BeginInvoke(new Action(() =>
				{
					while (cboxExchange.Items.Count > 0)
					{
						cboxExchange.Items.RemoveAt(0);
					}
				}));
				exchanges = new List<Exchange>();
			}

			// geting middle pair
			if (token.IsCancellationRequested) return;
			if ((bool)cboxMarket.Invoke(new Func<bool>(() => middleMarket.ContainsKey(cboxMarket.SelectedIndex))))
			{
				//bitfinex/btcusd
				if (token.IsCancellationRequested) return;
				pair = CryptowatchAPI.GetPair("https://api.cryptowat.ch/pairs/" + baseSymbol + cboxMarket.Invoke(new Func<string>(() => GetQuoteSymbol())));
				isDoubleLayer = true;
			}
			else
			{
				isDoubleLayer = false;
				try
				{
					if (token.IsCancellationRequested) return;
					pair = CryptowatchAPI.GetPair("https://api.cryptowat.ch/pairs/" + baseSymbol + cboxMarket.Invoke(new Func<string>(() => cboxMarket.Text.ToLower())));
				}
				catch
				{
					pair = null;
					return;
				}
			}

			// populating exchange combo box
			foreach (var market in pair.markets)
			{
				Exchange exchange = CryptowatchAPI.GetExchange("https://api.cryptowat.ch/exchanges/" + market.exchange);
				if (token.IsCancellationRequested) return;
				cboxExchange.BeginInvoke(new Action<string>(str =>
				{
					cboxExchange.Items.Add(str);
					// auto select
					if (cboxExchange.SelectedIndex == -1)
						cboxExchange.SelectedIndex = 0;
				}), exchange.name);
				exchanges.Add(exchange);
			}
			Console.WriteLine("LoadExchanges finished");

		}

		private void btnAddAlert_Click(object sender, EventArgs e)
		{
			double triggerPrice;
			string route;
			string endRoute = "0";
			AlertData alertData;
			Notification ntfy;

			// user input checking
			if (!double.TryParse(txtPrice.Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out triggerPrice))
			{
				MessageBox.Show("Something is wrong in price section.");
				return;
			}
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

			// creating alert data which requires price to convert
			if (isDoubleLayer)
			{
				string quoteSymbol = middleMarket[cboxMarket.SelectedIndex].Substring(middleMarket[cboxMarket.SelectedIndex].IndexOf('/') + 1, (middleMarket[cboxMarket.SelectedIndex].Substring(middleMarket[cboxMarket.SelectedIndex].IndexOf('/') + 1).Length - cboxMarket.Text.Substring(0, cboxMarket.Text.IndexOf(" (calculated)")).Length));
				route = "https://api.cryptowat.ch/markets/" + exchanges[cboxExchange.SelectedIndex].symbol + "/" + baseSymbol + quoteSymbol;
				endRoute = "https://api.cryptowat.ch/markets/" + middleMarket[cboxMarket.SelectedIndex];
				alertData = new AlertData(baseSymbol.ToUpper(), cboxMarket.Text.Substring(0, cboxMarket.Text.IndexOf(" (calculated)") + 1), baseName, exchanges[cboxExchange.SelectedIndex].name, route);
			}
			else // creating normal alert data
			{
				route = "https://api.cryptowat.ch/markets/" + exchanges[cboxExchange.SelectedIndex].symbol + "/" + baseSymbol + cboxMarket.Text.ToLower();
				alertData = new AlertData(baseSymbol.ToUpper(), cboxMarket.Text.ToUpper(), baseName, exchanges[cboxExchange.SelectedIndex].name, route);
			}

			// creating alert
			switch (cboxCondition.Text)
			{
				case "Higher than":
					Alert.AlertList.Add(new AbsoluteAlert(alertData, ntfy, triggerPrice, AbsoluteAlert.Type.higher, endRoute));
					break;
				case "Higher or equal than":
					Alert.AlertList.Add(new AbsoluteAlert(alertData, ntfy, triggerPrice, AbsoluteAlert.Type.higherOrEqual, endRoute));
					break;
				case "Lower than":
					Alert.AlertList.Add(new AbsoluteAlert(alertData, ntfy, triggerPrice, AbsoluteAlert.Type.lower, endRoute));
					break;
				case "Lower or equal than":
					Alert.AlertList.Add(new AbsoluteAlert(alertData, ntfy, triggerPrice, AbsoluteAlert.Type.lowerOrEqual, endRoute));
					break;
			}
			Alert.SaveAlerts();
			((IFormReference)Application.OpenForms["MainForm"]).AddAlertToListView();
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
			source.Cancel();
			base.OnFormClosing(e);
		}
	}
}

	 */