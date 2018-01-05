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
using System.Media;
using System.Configuration;
using CryptoWatcher.Properties;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Threading;
using Alerts;
using System.Diagnostics;
using System.Globalization;

// https://github.com/CoinCapDev/CoinCap.io
// https://www.cryptocompare.com/api/#-api-data-price // https://min-api.cryptocompare.com/data/histoday?aggregate=1&e=CCCAGG&extraParams=CryptoCompare&fsym=BTC&limit=365&tryConversion=false&tsym=USD
//https://cryptocoincharts.info/tools/api
//https://www.iconfinder.com

// search for apis: https://www.programmableweb.com/category/all/apis?keyword=cryptocurrency
// reducing file size https://stackoverflow.com/questions/5397070/how-to-save-large-data-to-file
// documentation // https://docs.microsoft.com/en-us/dotnet/csharp/codedoc

// fastest read speed
/*using (StreamReader sr = File.OpenText(fileName))
{
        string s = String.Empty;
        while ((s = sr.ReadLine()) != null)
        {
            //we're just testing read speeds
        }
}
 *
 */
// TODO: change site to cryptowatcher (if coin isn't found in cryptowatcher then take it from coinmarketcap 5min update)
// TODO: integrate email notification
// TODO: add edit alert button
// TODO: add fund calculator use coinmarketcap for total amount of currencies e.g. 1000 download 100 then 200 then 400 coins... with max 10 downloads
// TDOD: when user selects absolute alert and conditions are met tell him that alert would imidetly be hit
// TODO: maybe add price and values of other indicators whne adding alert so user can see it?
// TODO: get graph for watch section got to coinmarketcap.com - view source - line 1027 - generated sparkline link and download sparkline

// nice tutorial for setup http://www.c-sharpcorner.com/UploadFile/1492b1/creating-an-msi-package-for-C-Sharp-windows-application-using-a-v/
// TODO: when there are more than 1 notification and user deletes the mw get indoex out of range exception because indexes got updated whe aler got deleted. Suggestion: remove id in grid view and add it to notification calsss when notification is deleted update id in all notificatons 
namespace CryptoWatcher
{
	public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        private bool show_baloonTip = true;
		DataTable table = new DataTable();
		CustomAlertForm customAlertForm = new CustomAlertForm();

		public MainForm()
		{
			InitializeComponent();
			ntfyIconMinimized.BalloonTipTitle = "Crypto Watcher";
			ntfyIconMinimized.BalloonTipText = "Application minimized.";
			LoadSettings();
			tabControl.SelectedTab = tabAlert;
			Notification.MainForm = this;
			CustomAlertForm.MainForm = this;
			//Alerts.assets = CryptowatchAPI.GetAssets();
			//StreamWriter sw = new StreamWriter("allowanceGetTrades.txt");
			//	long max = long.MinValue;
			//	long min = long.MaxValue;
			//	double sum = 0;
			//	long i = 0;
			//	while (i < 100)
			//	{

			//			List<Assets> assets = CryptowatchAPI.GetAssets();
			//			Asset asset = CryptowatchAPI.GetAsset("https://api.cryptowat.ch/assets/btc");
			//			List<Candlestick> candlesticks = CryptowatchAPI.GetCandlesticks("https://api.cryptowat.ch/markets/gdax/btcusd/ohlc", TimeFrame.min1);
			//			List<Exchanges> exchanges = CryptowatchAPI.GetExchanges();
			//			Exchange exchange = CryptowatchAPI.GetExchange(exchanges[0].route);
			//			List<Markets> markets = CryptowatchAPI.GetMarkets();
			//			List<Markets> exchange_markets = CryptowatchAPI.GetMarkets(exchange.routes.markets);
			//			Market market = CryptowatchAPI.GetMarket(markets[0].route);//https://api.cryptowat.ch/markets/gdax/btcusd
			//			OrderBook orderBook = CryptowatchAPI.GetOrderBook(market.routes.orderbook);
			//			List<Pairs> pairs = CryptowatchAPI.GetPairs();
			//			Pair pair = CryptowatchAPI.GetPair(pairs[0].route);
			//			Dictionary<string, double> prices = CryptowatchAPI.GetPrices();
			//			double price = CryptowatchAPI.GetPrice(market.routes.price);
			//			SiteInformation siteInformation = CryptowatchAPI.GetSiteInformation();
			//			Dictionary<string, Summary> simmaries = CryptowatchAPI.GetSummaries();
			//			Summary summary = CryptowatchAPI.GetSummary("https://api.cryptowat.ch/markets/gdax/btcusd/summary");
			//			List<Trade> trades = CryptowatchAPI.GetTrades("https://api.cryptowat.ch/markets/gdax/btcusd/trades");
			//			if (CryptowatchAPI.allowance.cost < min)
			//				min = CryptowatchAPI.allowance.cost;
			//			else if (CryptowatchAPI.allowance.cost > max)
			//				max = CryptowatchAPI.allowance.cost;
			//			//sw.WriteLine(i.ToString() + ".cost=" + CryptowatchAPI.allowance.cost.ToString());
			//			//sum += CryptowatchAPI.allowance.cost;


			//			//sw.WriteLine("MinValue=" + min.ToString());
			//			//sw.WriteLine("MaxValue=" + max.ToString());
			//			//sw.Close();

			//		i++;
			//	}
			//	//sw.WriteLine("MinValue=" + min.ToString());
			//	//sw.WriteLine("MaxValue=" + max.ToString());
			//	//sw.WriteLine("AverageValue=" + (sum / 100).ToString());
			//	//sw.Close();

			//List<Ticker> ticker =  CoinMarketCapAPI.GetTickers(2, 9, Quote.KRW);
			//Ticker ticker = CoinMarketCapAPI.GetTicker("bitcoin", Quote.MYR);
			//GlobalData globalData = CoinMarketCapAPI.GetGlobalData(Quote.EUR);
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("Date", typeof(string));
			table.Columns.Add("Message", typeof(string));
			//table.Rows.Add(-1, "30.12.2017.20:09:02", "NO MESSAGE");
			//table.Rows.Add(-1, "30.12.2017.20:09:02", "NO MESSAGE");

			grdNotifications.DataSource = table;

			DataGridViewButtonColumn dismissBtnColumn = new DataGridViewButtonColumn();
			{
				dismissBtnColumn.Name = "dismissColumn";
				dismissBtnColumn.HeaderText = "Dismiss";
				dismissBtnColumn.Text = "Dismiss";
				dismissBtnColumn.UseColumnTextForButtonValue = true; //dont forget this line
				this.grdNotifications.Columns.Add(dismissBtnColumn);
			}
			grdNotifications.Columns["Id"].Visible = false;
			grdNotifications.Columns[1].Width = 125;
			grdNotifications.Columns[2].Width = grdNotifications.Width - grdNotifications.Columns[1].Width - 145;

			//DataGridViewButtonColumn uninstallButtonColumn = new DataGridViewButtonColumn();
			//uninstallButtonColumn.Name = "uninstall_column";
			//uninstallButtonColumn.Text = "Uninstall";
			//int columnIndex = 2;
			//if (grdNotifications.Columns["uninstall_column"] == null)
			//{
			//	grdNotifications.Columns.Insert(columnIndex, uninstallButtonColumn);
			//}
		}

		private async void MainForm_Load(object sender, EventArgs e)
		{
			Alert.LoadAlerts();
			foreach (var alert in Alert.AlertList)
			{
				ListViewItem listViewItem = new ListViewItem(alert.ToRow());
				lstView.Items.Add(listViewItem);
			}

			await Alert.UpdateTickerList();
			alert_timer.Enabled = true;
			AlertTimerTick(null, null);
		}

		private void grdNotifications_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			//if(grdNotifications.Columns[e.ColumnIndex].Name == "Action")
			//{
			//	if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			//		grdNotifications.Rows.RemoveAt(e.RowIndex);
			//}
			if (e.ColumnIndex == grdNotifications.Columns["dismissColumn"].Index)
			{
				Alert.AlertList[Convert.ToInt32(grdNotifications.Rows[e.RowIndex].Cells["Id"].Value)].StopNotifying();
				foreach (DataGridViewRow row in grdNotifications.Rows)
				{
					if(Convert.ToInt32(row.Cells["Id"].Value) > Convert.ToInt32(grdNotifications.Rows[e.RowIndex].Cells["Id"].Value))
					{
						row.Cells["Id"].Value = Convert.ToInt32(row.Cells["Id"].Value) - 1;
					}
				}
				grdNotifications.Rows.RemoveAt(e.RowIndex);
			}
		}

		// ****************************************************** Window related ****************************************************** //
		#region
		private void MainResize(object sender, EventArgs e)
        {
            // when you click minimize window it goes to system tray
            if (WindowState == FormWindowState.Minimized && show_baloonTip)
            {
                ShowInTaskbar = false;
                ntfyIconMinimized.Visible = true;
                ntfyIconMinimized.ShowBalloonTip(1000);
            }
            else 
                show_baloonTip = true;
        }
        #endregion
        // ****************************************************** Settings ************************************************************ //
#region
        private void chcBoxPlaySound_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.playSound = chcBoxPlaySound.Checked;
            Settings.Default.Save();
        }

        private void chcBoxAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            Settings.Default.lookForUpdates = chcBoxAutoUpdate.Checked;
            Settings.Default.Save();
            StreamWriter sw = new StreamWriter("SharedSettings.txt");
            sw.WriteLine(new JavaScriptSerializer().Serialize(Settings.Default.lookForUpdates));
            sw.Close();
        }

        private void LoadSettings()
        {
            chcBoxPlaySound.Checked = Settings.Default.playSound;
            if (!File.Exists("SharedSettings.txt"))
                return;
            StreamReader sr = new StreamReader("SharedSettings.txt");
            Settings.Default.lookForUpdates = (bool)new JavaScriptSerializer().Deserialize(sr.ReadLine(), typeof(bool));
            sr.Close();
            chcBoxAutoUpdate.Checked = Settings.Default.lookForUpdates;
        }
		#endregion
		// ****************************************************** Timers ************************************************************** //
		#region
			// TODO: availible allowance is updated on 55 min
        private async void AlertTimerTick(object sender, EventArgs e)
        {
			// if there aren't alerts we exit
			if (Alert.AlertList.Count == 0)
				return;
			// for all alerts we are testing condition and if condition is fullfilled we show message to notify user
			DateTime currentTime = DateTime.Now;
			for (int i = 0; i < Alert.AlertList.Count; i++)
			{
				if (Alert.AlertList[i].LastTriggered + new TimeSpan(0, 0, Alert.AlertList[i].notification.Interval) <= currentTime && !Alert.AlertList[i].IsNotifying && await Alert.AlertList[i].Test())
				{
					Alert.AlertList[i].Notify(currentTime);
				}
			}
			long maximumCosts = Alert.GetMaxCosts();
			double next_update_time;
			if (maximumCosts != 0)
			{
				// TODO: either use fixed amount or average costs for all functions that are triggered by user for reserve
				// we are keeping some costs for reserve that equals to all costs that is used to one timer tick
				double n_of_updates_left = (APIs.CryptowatchAPI.allowance.remaining - maximumCosts) / maximumCosts * 1.5;
				// cryptowat.ch allowance updates every hour on 55th minute
				int minutes_left = 55 - DateTime.Now.Minute;
				if (minutes_left <= 0)
				{
					minutes_left = 55 - minutes_left;
				}
				if (n_of_updates_left != 0)
					next_update_time = minutes_left / n_of_updates_left * 60000;
				else next_update_time = minutes_left * 60000;
				// limiting to  second interval
				if (next_update_time < 1000)
					next_update_time = 1000;
				Console.WriteLine($"Next update time: {next_update_time}, # updates left: {n_of_updates_left}, Remaining: {APIs.CryptowatchAPI.allowance.remaining}");
			}
			else next_update_time = 1000;
			alert_timer.Interval = (int)next_update_time;
			lblNextUpdate.Text = ((int)(next_update_time / 1000)).ToString() + "s";
			Alert.CurrentCosts = 0;
		}

		private void CMCStartingTimer_Tick(object sender, EventArgs e)
		{
			//CMCTimer_Tick(null, null);
			//CMCTimer.Enabled = true;
			//CMCTimer.Interval = 6000;
			//CMCStartingTimer.Enabled = false;
			if (DateTime.Now.Minute % 5 == 0)
			{
				CMCTimer_Tick(null, null);
				CMCTimer.Enabled = true;
				CMCStartingTimer.Enabled = false;
			}
		}

		private async void CMCTimer_Tick(object sender, EventArgs e)
		{
			await Alert.UpdateTickerList();
		}

		#endregion
		// ****************************************************** Notifications ******************************************************* //
		#region
		public void ShowWindow()
		{
			Activate();
			tabControl.SelectedTab = tabNotifications;
		}

		public void AddMessage(int id)
		{
			bool found = false;
			
			// preventing notification build up
			foreach (DataGridViewRow row in grdNotifications.Rows)
			{
				if (Convert.ToInt32(row.Cells["Id"].Value) == id)
				{
					row.Cells["Date"].Value = DateTime.Now.ToString();
					row.Cells["Message"].Value = Alert.AlertList[id].Message;
					row.Selected = true;
					found = true;
					break;
				}
			}

			// if we havent found notification we create it
			if (!found)
			{
				table.Rows.Add(id, DateTime.Now.ToString(), Alert.AlertList[id].Message);
				grdNotifications.Rows[grdNotifications.Rows.Count - 1].Selected = true;
			}
		}

		public void TryRemoveAlert(int id)
		{
			// if alert is one time only we delete it
			if (Alert.AlertList[id].notification.Interval == -1)
			{
				lstView.Items.RemoveAt(id);
				Alert.AlertList.RemoveAt(id);
				Alert.SaveAlerts();
			}
		}

		public void AddAlertToListView()
		{
			lstView.Items.Add(new ListViewItem(Alert.AlertList.Last().ToRow()));
		}

		// occurs when user clicks popup message
		public void Popup_Click(object sender, EventArgs e)
		{
			ShowWindow();
		}

		// occurs when you click on icon in system tray
		private void ntfy_DClick(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            ntfyIconMinimized.Visible = false;
			WindowState = FormWindowState.Normal;
			Visible = true;
			Activate();
        }
		#endregion
		// ****************************************************** Buttons ************************************************************* //
		#region
		private void btnNew_Click(object sender, EventArgs e)
		{
			customAlertForm.Show();
		}

		private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstView.SelectedItems.Count == 0)
            {
                return;
            }

			// removing in notification area
			foreach (DataGridViewRow row in grdNotifications.Rows)
			{
				foreach (ListViewItem eachItem in lstView.SelectedItems)
				{
					if (Convert.ToInt32(row.Cells["Id"].Value) == eachItem.Index)
					{
						grdNotifications.Rows.RemoveAt(row.Index);
						break;
					}
				}
			}

			// removing in alerts list view
			foreach (ListViewItem eachItem in lstView.SelectedItems)
            {
				Alert.AlertList[eachItem.Index].StopNotifying(false);
                Alert.AlertList.RemoveAt(eachItem.Index);
                lstView.Items.Remove(eachItem);
            }
            Alert.SaveAlerts();
        }
		#endregion
	}
}
