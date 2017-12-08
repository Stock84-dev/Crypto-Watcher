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

// TODO: change site to cryptowatcher (if coin isn't found in cryptowatcher then take it from coinmarketcap 5min update)
// TODO: integrate email notification
// TODO: add edit alert button

// nice tutorial for setup http://www.c-sharpcorner.com/UploadFile/1492b1/creating-an-msi-package-for-C-Sharp-windows-application-using-a-v/
namespace Crypto_watcher
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        private Notification notification;
        private bool show_baloonTip = true;
       
        public Form1()
        {
            InitializeComponent();
            notification = new Notification();
            notifyIcon1.BalloonTipTitle = "Crypto Watcher";
            notifyIcon1.BalloonTipText = "Application minimized.";
            LoadSettings();
            tabControl.SelectedTab = tabAlert;
        }

        // ****************************************************** Window related ****************************************************** //
#region
        private void MainResize(object sender, EventArgs e)
        {
            // when you click minimize window it goes to system tray
            if (WindowState == FormWindowState.Minimized && show_baloonTip)
            {
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
            else 
                show_baloonTip = true;
        }

        // gets called after the form is loaded
        private void Form1_Shown(object sender, EventArgs e)
        {
            Alert.Load();
            
            foreach (var alert in Alert.alerts)
            {
                ListViewItem listViewItem = new ListViewItem(alert.ToRow());
                lstView.Items.Add(listViewItem);
            }
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
        // runs on application load to start main timer 
        private void StartingTimerTick(object sender, EventArgs e) // TODO: when starting timer starts test alerts
        {
            int time = DateTime.Now.Minute;
            // API calls are updated every 5 minutes
            if (time % 5 == 0)
            {
                starting_timer.Stop();
                AlertTimerTick(null, null);
                alert_timer.Start();
            }
        }
        // every 5 minutes we see if some conditions are met
        private void AlertTimerTick(object sender, EventArgs e)
        {
           
            foreach (var alert in Alert.alerts)
            {
                if(alert.Test())
                {
                    ShowNotification("Alert triggered: " + alert.coin.name + 
                        ", price of " + (alert.condition.market_type == (int)MarketType.USD ? alert.coin.price_usd : alert.coin.price_btc)
                        + " passed alert point of " + alert.condition.value + ".");
                    
                }
            }
        }
        #endregion
        // ****************************************************** Notifications ******************************************************* //
#region
        private void ShowNotification(string message)
        {
            // showing window on top
            show_baloonTip = false;
            WindowState = FormWindowState.Minimized;
            Show();
            WindowState = FormWindowState.Normal;

            notification.Notify(message);
        }
        // occurs when you click on icon in system tray
        private void ntfy_Click(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }
#endregion
        // ****************************************************** Buttons ************************************************************* //
#region
        private void btnNew_Click(object sender, EventArgs e)
        {
            // creating new window
            CustomAlertForm customAlertForm = new CustomAlertForm();
            customAlertForm.ShowDialog();

            if (!customAlertForm.added_alert)
                return;

            customAlertForm.added_alert = false;
            lstView.Items.Add(new ListViewItem(Alert.alerts.Last().ToRow()));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select item in box to remove it.");
                return;
            }

            foreach (ListViewItem eachItem in lstView.SelectedItems)
            {
                Alert.alerts.RemoveAt(eachItem.Index);
                lstView.Items.Remove(eachItem);
            }
            Alert.Save();
        }
#endregion
    }
}
