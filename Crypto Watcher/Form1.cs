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
using System.Media;
using System.Configuration;
using CryptoWatcher.Properties;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using CryptoWatcher.Alerts;


//https://cryptocoincharts.info/tools/api
//https://www.iconfinder.com

// API 
// http://coincap.io/
// https://www.cryptocompare.com/api/#-api-data-price 
// https://cryptocoincharts.info/tools/api
// https://www.cryptonator.com/api
// https://icowatchlist.com/ // ico watch list

// coinmarketcap
// cryptowatch


// search for apis: https://www.programmableweb.com/category/all/apis?keyword=cryptocurrency
// reducing file size https://stackoverflow.com/questions/5397070/how-to-save-large-data-to-file
// documentation // https://docs.microsoft.com/en-us/dotnet/csharp/codedoc

// TODO: open tradingview chart = https://www.tradingview.com/chart/?symbol=BITSTAMP:ETHUSD
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
// TODO: what happens to alerts when user puts wrong input and then tries again, does it create a new alert or modifies old one(possible corrupted data).
// TODO: integrate email notification
// TODO: add edit alert button
// TODO: add fund calculator use coinmarketcap for total amount of currencies e.g. 1000 download 100 then 200 then 400 coins... with max 10 downloads
// TDOD: when user selects absolute alert and conditions are met tell him that alert would imidetly be hit
// TODO: maybe add price and values of other indicators whne adding alert so user can see it?
// TODO: get graph for watch section got to coinmarketcap.com - view source - line 1027 - generated sparkline link and download sparkline
// TODO: hitBTC graph https://hitbtc.com/chart/IPLBTC
// TODO: portfolio calculator prototype
// TODO: create screensaver witch will display currency graphs
// nice tutorial for setup http://www.c-sharpcorner.com/UploadFile/1492b1/creating-an-msi-package-for-C-Sharp-windows-application-using-a-v/
// TODO: change price type to decimal because some exchanges use better precision
// TODO: include licence in setup project

/* Auto updater project
 * merge projects to one exe file
 * use json serialization
 * have one file in which there are file names that version requires
 *
 */
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
			APIs.AbstractAPI.Init();
			ntfyIconMinimized.BalloonTipTitle = "Crypto Watcher";
			ntfyIconMinimized.BalloonTipText = "Application minimized.";
			LoadSettings();
			tabControl.SelectedTab = tabAlert;
			Notification.MainForm = this;
			CustomAlertForm.MainForm = this;
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("Date", typeof(string));
			table.Columns.Add("Message", typeof(string));
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
		}

		private async void MainForm_Load(object sender, EventArgs e)
		{
			await AbstractAlert.LoadAlertsAsync();
			foreach (var alert in AbstractAlert.alerts)
			{
				ListViewItem listViewItem = new ListViewItem(alert.ToRow());
				lstView.Items.Add(listViewItem);
			}
		}

		private void grdNotifications_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == grdNotifications.Columns["dismissColumn"].Index)
			{
				AbstractAlert.alerts[Convert.ToInt32(grdNotifications.Rows[e.RowIndex].Cells["Id"].Value)].StopNotifying();
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
            Settings.Default.PlaySound = chcBoxPlaySound.Checked;
            Settings.Default.Save();
        }

        private void chcBoxAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.LookForUpdates = chcBoxAutoUpdate.Checked;
            Settings.Default.Save();
        }

        private void LoadSettings()
        {
            chcBoxPlaySound.Checked = Settings.Default.PlaySound;
            if (!File.Exists("SharedSettings.txt"))
                return;
            StreamReader sr = new StreamReader("SharedSettings.txt");
            Settings.Default.LookForUpdates = (bool)new JavaScriptSerializer().Deserialize(sr.ReadLine(), typeof(bool));
            sr.Close();
            chcBoxAutoUpdate.Checked = Settings.Default.LookForUpdates;
        }
		#endregion

		// ****************************************************** Notifications ******************************************************* //
		#region
		public void ShowWindow()
		{
			if (InvokeRequired)
				BeginInvoke((Action)(() => {
					Activate();
					tabControl.SelectedTab = tabNotifications;
				}));
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
					row.Cells["Message"].Value = AbstractAlert.alerts[id].Message;
					row.Selected = true;
					found = true;
					break;
				}
			}

			// if we havent found notification we create it
			if (!found)
			{
				table.Rows.Add(id, DateTime.Now.ToString(), AbstractAlert.alerts[id].Message);
				grdNotifications.Rows[grdNotifications.Rows.Count - 1].Selected = true;
			}
		}

		public void TryRemoveAlert(int id)
		{
			// if alert is one time only we delete it
			if (AbstractAlert.alerts[id].notification.Interval == -1)
			{
				lstView.Items.RemoveAt(id);
				AbstractAlert.alerts[id].Destroy();
			}
		}

		public void AddAlertToListView()
		{
			lstView.Items.Add(new ListViewItem(AbstractAlert.alerts.Last().ToRow()));
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
				AbstractAlert.alerts[eachItem.Index].Destroy();
                lstView.Items.Remove(eachItem);
            }
        }
		#endregion

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			AbstractAlert.UnsubscribeAll();
		}
	}
}
