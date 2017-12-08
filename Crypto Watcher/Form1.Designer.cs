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

namespace Crypto_watcher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.starting_timer = new System.Windows.Forms.Timer(this.components);
            this.alert_timer = new System.Windows.Forms.Timer(this.components);
            this.btnNew = new MetroFramework.Controls.MetroButton();
            this.btnRemove = new MetroFramework.Controls.MetroButton();
            this.lstView = new MetroFramework.Controls.MetroListView();
            this.CoinName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Condition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Market = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Site = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl = new MetroFramework.Controls.MetroTabControl();
            this.tabAlert = new MetroFramework.Controls.MetroTabPage();
            this.tabWatch = new MetroFramework.Controls.MetroTabPage();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.tabSettings = new MetroFramework.Controls.MetroTabPage();
            this.chcBoxAutoUpdate = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.chcBoxPlaySound = new MetroFramework.Controls.MetroCheckBox();
            this.tabControl.SuspendLayout();
            this.tabAlert.SuspendLayout();
            this.tabWatch.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.ntfy_Click);
            // 
            // starting_timer
            // 
            this.starting_timer.Enabled = true;
            this.starting_timer.Interval = 1;
            this.starting_timer.Tick += new System.EventHandler(this.StartingTimerTick);
            // 
            // alert_timer
            // 
            this.alert_timer.Interval = 300000;
            this.alert_timer.Tick += new System.EventHandler(this.AlertTimerTick);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(3, 380);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 9;
            this.btnNew.Text = "New";
            this.btnNew.UseSelectable = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(84, 380);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseSelectable = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lstView
            // 
            this.lstView.AutoArrange = false;
            this.lstView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CoinName,
            this.Condition,
            this.Value,
            this.Market,
            this.Site});
            this.lstView.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lstView.FullRowSelect = true;
            this.lstView.Location = new System.Drawing.Point(0, 3);
            this.lstView.Name = "lstView";
            this.lstView.OwnerDraw = true;
            this.lstView.Size = new System.Drawing.Size(993, 371);
            this.lstView.TabIndex = 11;
            this.lstView.UseCompatibleStateImageBehavior = false;
            this.lstView.UseSelectable = true;
            this.lstView.View = System.Windows.Forms.View.Details;
            // 
            // CoinName
            // 
            this.CoinName.Text = "Name";
            this.CoinName.Width = 197;
            // 
            // Condition
            // 
            this.Condition.Text = "Condition";
            this.Condition.Width = 198;
            // 
            // Value
            // 
            this.Value.Text = "Value";
            this.Value.Width = 198;
            // 
            // Market
            // 
            this.Market.Text = "Market";
            this.Market.Width = 198;
            // 
            // Site
            // 
            this.Site.Text = "Site";
            this.Site.Width = 198;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAlert);
            this.tabControl.Controls.Add(this.tabWatch);
            this.tabControl.Controls.Add(this.tabSettings);
            this.tabControl.Location = new System.Drawing.Point(23, 63);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 2;
            this.tabControl.Size = new System.Drawing.Size(1004, 448);
            this.tabControl.TabIndex = 12;
            this.tabControl.UseSelectable = true;
            // 
            // tabAlert
            // 
            this.tabAlert.Controls.Add(this.btnNew);
            this.tabAlert.Controls.Add(this.btnRemove);
            this.tabAlert.Controls.Add(this.lstView);
            this.tabAlert.HorizontalScrollbarBarColor = true;
            this.tabAlert.HorizontalScrollbarHighlightOnWheel = false;
            this.tabAlert.HorizontalScrollbarSize = 10;
            this.tabAlert.Location = new System.Drawing.Point(4, 38);
            this.tabAlert.Name = "tabAlert";
            this.tabAlert.Size = new System.Drawing.Size(996, 406);
            this.tabAlert.TabIndex = 0;
            this.tabAlert.Text = "Alerts";
            this.tabAlert.VerticalScrollbarBarColor = true;
            this.tabAlert.VerticalScrollbarHighlightOnWheel = false;
            this.tabAlert.VerticalScrollbarSize = 10;
            // 
            // tabWatch
            // 
            this.tabWatch.Controls.Add(this.metroLabel2);
            this.tabWatch.HorizontalScrollbarBarColor = true;
            this.tabWatch.HorizontalScrollbarHighlightOnWheel = false;
            this.tabWatch.HorizontalScrollbarSize = 10;
            this.tabWatch.Location = new System.Drawing.Point(4, 38);
            this.tabWatch.Name = "tabWatch";
            this.tabWatch.Size = new System.Drawing.Size(996, 406);
            this.tabWatch.TabIndex = 1;
            this.tabWatch.Text = "Watch";
            this.tabWatch.VerticalScrollbarBarColor = true;
            this.tabWatch.VerticalScrollbarHighlightOnWheel = false;
            this.tabWatch.VerticalScrollbarSize = 10;
            // 
            // metroLabel2
            // 
            this.metroLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel2.Location = new System.Drawing.Point(450, 180);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(119, 25);
            this.metroLabel2.TabIndex = 2;
            this.metroLabel2.Text = "Coming soon!";
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.chcBoxAutoUpdate);
            this.tabSettings.Controls.Add(this.metroLabel1);
            this.tabSettings.Controls.Add(this.chcBoxPlaySound);
            this.tabSettings.HorizontalScrollbarBarColor = true;
            this.tabSettings.HorizontalScrollbarHighlightOnWheel = false;
            this.tabSettings.HorizontalScrollbarSize = 10;
            this.tabSettings.Location = new System.Drawing.Point(4, 38);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(996, 406);
            this.tabSettings.TabIndex = 2;
            this.tabSettings.Text = "Settings";
            this.tabSettings.VerticalScrollbarBarColor = true;
            this.tabSettings.VerticalScrollbarHighlightOnWheel = false;
            this.tabSettings.VerticalScrollbarSize = 10;
            // 
            // chcBoxAutoUpdate
            // 
            this.chcBoxAutoUpdate.AutoSize = true;
            this.chcBoxAutoUpdate.Checked = true;
            this.chcBoxAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chcBoxAutoUpdate.Location = new System.Drawing.Point(3, 24);
            this.chcBoxAutoUpdate.Name = "chcBoxAutoUpdate";
            this.chcBoxAutoUpdate.Size = new System.Drawing.Size(87, 15);
            this.chcBoxAutoUpdate.TabIndex = 4;
            this.chcBoxAutoUpdate.Text = "AutoUpdate";
            this.chcBoxAutoUpdate.UseSelectable = true;
            this.chcBoxAutoUpdate.CheckedChanged += new System.EventHandler(this.chcBoxAutoUpdate_CheckedChanged);
            // 
            // metroLabel1
            // 
            this.metroLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.Location = new System.Drawing.Point(450, 180);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(124, 25);
            this.metroLabel1.TabIndex = 3;
            this.metroLabel1.Text = "More to come!";
            // 
            // chcBoxPlaySound
            // 
            this.chcBoxPlaySound.AutoSize = true;
            this.chcBoxPlaySound.Checked = true;
            this.chcBoxPlaySound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chcBoxPlaySound.Location = new System.Drawing.Point(3, 3);
            this.chcBoxPlaySound.Name = "chcBoxPlaySound";
            this.chcBoxPlaySound.Size = new System.Drawing.Size(79, 15);
            this.chcBoxPlaySound.TabIndex = 2;
            this.chcBoxPlaySound.Text = "PlaySound";
            this.chcBoxPlaySound.UseSelectable = true;
            this.chcBoxPlaySound.CheckedChanged += new System.EventHandler(this.chcBoxPlaySound_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 534);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Crypto Watcher";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.MainResize);
            this.tabControl.ResumeLayout(false);
            this.tabAlert.ResumeLayout(false);
            this.tabWatch.ResumeLayout(false);
            this.tabWatch.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer starting_timer;
        private System.Windows.Forms.Timer alert_timer;
        private MetroFramework.Controls.MetroButton btnNew;
        private MetroFramework.Controls.MetroButton btnRemove;
        private MetroFramework.Controls.MetroListView lstView;
        private MetroFramework.Controls.MetroTabControl tabControl;
        private MetroFramework.Controls.MetroTabPage tabAlert;
        private MetroFramework.Controls.MetroTabPage tabWatch;
        private MetroFramework.Controls.MetroTabPage tabSettings;
        private System.Windows.Forms.ColumnHeader CoinName;
        private System.Windows.Forms.ColumnHeader Market;
        private System.Windows.Forms.ColumnHeader Site;
        private System.Windows.Forms.ColumnHeader Condition;
        private System.Windows.Forms.ColumnHeader Value;
        private MetroFramework.Controls.MetroCheckBox chcBoxPlaySound;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroCheckBox chcBoxAutoUpdate;
    }
}

