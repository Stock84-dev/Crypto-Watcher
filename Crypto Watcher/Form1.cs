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
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CryptoWatcher.Properties;
using CryptoWatcher.Alert;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;


// performance impact: https://www.youtube.com/watch?v=-H5oEgOdO6U
/* 1. no exception is always faster than throwing an exception
 * 2. string builder is faster than string, but char array with pointers is even faster 
 * 3. 1D array is faster than nD array if you use formula that maps nd array into 1d array, if you just increment id it's even faster
 * 4. for is faster than foreach, what about list.foreach()?
 * 5. struct is faster than class
 * 6. array.CopyTo() is faster than manually copying
 */
// shortcuts:
/* alt+w+m=close all tabs
 * ctrl+,=navigate to
 * CPU,GPU,RAM... prifiling = alt+F2
 * ctrl+m+o = collapse declarations
 * ctrl+m+l=expand declarations
 * */

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
// TODO: integrate email notification
// TODO: add fund calculator use coinmarketcap for total amount of currencies e.g. 1000 download 100 then 200 then 400 coins... with max 10 downloads
// TDOD: when user selects absolute alert and conditions are met tell him that alert would imidetly be hit
// TODO: maybe add price and values of other indicators while adding alert so user can see it?
// TODO: get graph for watch section got to coinmarketcap.com - view source - line 1027 - generated sparkline link and download sparkline
// TODO: hitBTC graph https://hitbtc.com/chart/IPLBTC
// TODO: portfolio calculator prototype
// TODO: create screensaver witch will display currency graphs
// nice tutorial for setup http://www.c-sharpcorner.com/UploadFile/1492b1/creating-an-msi-package-for-C-Sharp-windows-application-using-a-v/
// TODO: change price type to decimal because some exchanges use better precision
// TODO: include licence in setup project
// TODO: replace custom alert panel function with user control
// TODO: metroframework has bugs and is no longer being developed so make your own UI
// TODO: reactivate alert after condition is no longer met
// TODO: merge DLLs with EXE
// TODO: make your own settings class

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
        private bool _showBaloonTip = true;
		private DataTable _notificationTable = new DataTable();
		private CustomAlertForm _customAlertForm = new CustomAlertForm();

		public MainForm()
		{
			InitializeComponent();
			ntfyIconMinimized.BalloonTipTitle = "Crypto Watcher";
			ntfyIconMinimized.BalloonTipText = "Application minimized.";
			LoadSettings();
			tabControl.SelectedTab = tabAlert;
			Notification.MainForm = this;
			CustomAlertForm.MainForm = this;
			AbstractAlert.PriceChanged += AbstractAlert_PriceChanged;

			_notificationTable.Columns.Add("Id", typeof(int));
			_notificationTable.Columns.Add("Date", typeof(string));
			_notificationTable.Columns.Add("Message", typeof(string));

			grdNotifications.DataSource = _notificationTable;
			//grdNotifications.Columns["Id"].Visible = false;
			grdNotifications.AllowUserToResizeColumns = true;

			DataGridViewButtonColumn dismissBtnColumn = new DataGridViewButtonColumn();
			{
				dismissBtnColumn.Name = "dismissColumn";
				dismissBtnColumn.HeaderText = "Dismiss";
				dismissBtnColumn.Text = "Dismiss";
				dismissBtnColumn.UseColumnTextForButtonValue = true; //dont forget this line
				grdNotifications.Columns.Add(dismissBtnColumn);
			}
			grdNotifications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			//grdAlerts.Columns[0].Visible = false;
			// hiding selector column
			grdAlerts.RowHeadersVisible = false;
			grdNotifications.RowHeadersVisible = false;
			grdNotifications.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			grdNotifications.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
		}

		private void AbstractAlert_PriceChanged(object sender, PriceUpdateEventArgs e)
		{
			foreach (DataGridViewRow row in grdAlerts.Rows)
			{
				if (Convert.ToInt32(row.Cells["Id"].Value) == e.AlertId)
				{
					if (grdAlerts.InvokeRequired)
						grdAlerts.BeginInvoke(new Action(() => row.SetValues(e.Row)));
					else row.SetValues(e.Row);
					break;
				}
			}
		}

		private async void MainForm_Load(object sender, EventArgs e)
		{
			await AbstractAlert.LoadAlertsAsync();

			for (int i = 0; i < AbstractAlert.Alerts.Count; i++)
			{
				grdAlerts.Rows.Add(AbstractAlert.Alerts[i].Row);
				grdAlerts.Rows[i].Cells["IndicatorValue"].ToolTipText = AbstractAlert.Alerts[i].IndicatorValueToolTip;
			}

			grdAlerts.ClearSelection();
		}

		private void grdNotifications_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == grdNotifications.Columns["dismissColumn"].Index)
			{
				AbstractAlert.Alerts[Convert.ToInt32(grdNotifications.Rows[e.RowIndex].Cells["Id"].Value)].StopNotifying();
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
            if (WindowState == FormWindowState.Minimized && _showBaloonTip)
            {
                ShowInTaskbar = false;
                ntfyIconMinimized.Visible = true;
                ntfyIconMinimized.ShowBalloonTip(1000);
            }
            else 
                _showBaloonTip = true;
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
            chcBoxAutoUpdate.Checked = Settings.Default.LookForUpdates;
        }
		#endregion

		// ****************************************************** Notifications ******************************************************* //
		#region
		public void ShowWindow()
		{
			if (InvokeRequired)
				BeginInvoke((Action)ShowWindow);
			else
			{
				Activate();
				tabControl.SelectedTab = tabNotifications;
			}
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
					row.Cells["Message"].Value = AbstractAlert.Alerts[id].Message;
					row.Selected = true;
					found = true;
					break;
				}
			}

			// if we havent found notification we create it
			if (!found)
				_notificationTable.Rows.Add(id, DateTime.Now.ToString(), AbstractAlert.Alerts[id].Message);
		}

		public void TryRemoveAlert(int id)
		{
			// if alert is one time only we delete it
			if (AbstractAlert.Alerts[id].Notification.Interval == -1)
			{
				RemoveAlertFromGridView(id);
				AbstractAlert.Alerts[id].Destroy();
			}
		}

		public void AddAlertToGridView(int id)
		{
			if (id == -1)
				grdAlerts.Rows.Add(AbstractAlert.Alerts.Last().Row);
			else
				grdAlerts.Rows[id].SetValues(AbstractAlert.Alerts[id].Row);
		}

		public void RemoveNotification(int alertId)
		{
			// removing notifications that are left if alert was edited
			foreach (DataGridViewRow row in grdNotifications.Rows)
			{
				if (Convert.ToInt32(row.Cells["Id"].Value) == alertId)
				{
					grdNotifications.Rows.RemoveAt(row.Index);
					break;
				}
			}
		}

		public void RemoveAlertFromGridView(int id)
		{
			foreach (DataGridViewRow row in grdAlerts.Rows)
			{
				if (Convert.ToInt32(row.Cells["Id"].Value) == id)
				{
					grdAlerts.Rows.RemoveAt(row.Index);
					break;
				}
			}
			foreach (DataGridViewRow row in grdAlerts.Rows)
			{
				if (Convert.ToInt32(row.Cells["Id"].Value) > id)
					row.Cells["Id"].Value = Convert.ToInt32(row.Cells["Id"].Value) - 1;
			}
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
			_customAlertForm.Show();
		}

		private void btnRemove_Click(object sender, EventArgs e)
        {
            if (grdAlerts.SelectedRows.Count == 0)
            {
                return;
            }

			// removing in notification area
			foreach (DataGridViewRow notfRow in grdNotifications.Rows)
			{
				foreach (DataGridViewRow alertRow in grdAlerts.SelectedRows)
				{
					if (Convert.ToInt32(notfRow.Cells["Id"].Value) == Convert.ToInt32(alertRow.Cells["Id"].Value))
					{
						grdNotifications.Rows.RemoveAt(notfRow.Index);
						break;
					}
				}
			}

			// removing in alerts list view
			foreach (DataGridViewRow alertRow in grdAlerts.SelectedRows)
			{
				AbstractAlert.Alerts[Convert.ToInt32(alertRow.Cells["Id"].Value)].Destroy();
				// matching alert ids with gridview
				int i = grdAlerts.Rows.IndexOf(alertRow) + 1;
				if (i != grdAlerts.Rows.Count)
				{
					for (; i < grdAlerts.Rows.Count; i++)
					{
						grdAlerts.Rows[i].Cells["Id"].Value = i - 1;
					}
				}
				grdAlerts.Rows.RemoveAt(alertRow.Index);
			}
        }
		#endregion

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			AbstractAlert.UnsubscribeAll();
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (grdAlerts.SelectedRows.Count == 0)
				return;

			foreach (DataGridViewRow alertRow in grdAlerts.SelectedRows)
			{
				int alertId = Convert.ToInt32(alertRow.Cells["Id"].Value);
				_customAlertForm.ShowEdit(alertId);
			}
		}
	}
}
