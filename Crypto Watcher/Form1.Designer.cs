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

namespace CryptoWatcher
{
    partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.ntfyIconMinimized = new System.Windows.Forms.NotifyIcon(this.components);
			this.btnNew = new MetroFramework.Controls.MetroButton();
			this.btnRemove = new MetroFramework.Controls.MetroButton();
			this.tabControl = new MetroFramework.Controls.MetroTabControl();
			this.tabAlert = new MetroFramework.Controls.MetroTabPage();
			this.grdAlerts = new MetroFramework.Controls.MetroGrid();
			this.btnEdit = new MetroFramework.Controls.MetroButton();
			this.tabNotifications = new MetroFramework.Controls.MetroTabPage();
			this.grdNotifications = new MetroFramework.Controls.MetroGrid();
			this.tabSettings = new MetroFramework.Controls.MetroTabPage();
			this.chcBoxAutoUpdate = new MetroFramework.Controls.MetroCheckBox();
			this.chcBoxPlaySound = new MetroFramework.Controls.MetroCheckBox();
			this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
			this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Currency = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Exchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AlertType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Condition = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Timeframe = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.IndicatorValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabControl.SuspendLayout();
			this.tabAlert.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdAlerts)).BeginInit();
			this.tabNotifications.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdNotifications)).BeginInit();
			this.tabSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// ntfyIconMinimized
			// 
			this.ntfyIconMinimized.Icon = ((System.Drawing.Icon)(resources.GetObject("ntfyIconMinimized.Icon")));
			this.ntfyIconMinimized.Text = "notifyIcon1";
			this.ntfyIconMinimized.Visible = true;
			this.ntfyIconMinimized.DoubleClick += new System.EventHandler(this.ntfy_DClick);
			// 
			// btnNew
			// 
			this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnNew.Location = new System.Drawing.Point(3, 439);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(75, 23);
			this.btnNew.TabIndex = 9;
			this.btnNew.Text = "New";
			this.btnNew.UseSelectable = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnRemove.Location = new System.Drawing.Point(165, 439);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 10;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseSelectable = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabAlert);
			this.tabControl.Controls.Add(this.tabNotifications);
			this.tabControl.Controls.Add(this.tabSettings);
			this.tabControl.Location = new System.Drawing.Point(23, 63);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(1004, 504);
			this.tabControl.TabIndex = 12;
			this.tabControl.UseSelectable = true;
			// 
			// tabAlert
			// 
			this.tabAlert.Controls.Add(this.grdAlerts);
			this.tabAlert.Controls.Add(this.btnEdit);
			this.tabAlert.Controls.Add(this.btnNew);
			this.tabAlert.Controls.Add(this.btnRemove);
			this.tabAlert.HorizontalScrollbarBarColor = true;
			this.tabAlert.HorizontalScrollbarHighlightOnWheel = false;
			this.tabAlert.HorizontalScrollbarSize = 10;
			this.tabAlert.Location = new System.Drawing.Point(4, 38);
			this.tabAlert.Name = "tabAlert";
			this.tabAlert.Size = new System.Drawing.Size(996, 462);
			this.tabAlert.TabIndex = 0;
			this.tabAlert.Text = "Alerts";
			this.tabAlert.VerticalScrollbarBarColor = true;
			this.tabAlert.VerticalScrollbarHighlightOnWheel = false;
			this.tabAlert.VerticalScrollbarSize = 10;
			// 
			// grdAlerts
			// 
			this.grdAlerts.AllowUserToAddRows = false;
			this.grdAlerts.AllowUserToDeleteRows = false;
			this.grdAlerts.AllowUserToOrderColumns = true;
			this.grdAlerts.AllowUserToResizeRows = false;
			this.grdAlerts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grdAlerts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.grdAlerts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.grdAlerts.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.grdAlerts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.grdAlerts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdAlerts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.grdAlerts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdAlerts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Currency,
            this.Exchange,
            this.AlertType,
            this.Condition,
            this.Timeframe,
            this.Price,
            this.IndicatorValue});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdAlerts.DefaultCellStyle = dataGridViewCellStyle2;
			this.grdAlerts.EnableHeadersVisualStyles = false;
			this.grdAlerts.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdAlerts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.grdAlerts.Location = new System.Drawing.Point(3, 3);
			this.grdAlerts.Name = "grdAlerts";
			this.grdAlerts.ReadOnly = true;
			this.grdAlerts.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdAlerts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.grdAlerts.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.grdAlerts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdAlerts.Size = new System.Drawing.Size(997, 430);
			this.grdAlerts.TabIndex = 13;
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnEdit.Location = new System.Drawing.Point(84, 439);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(75, 23);
			this.btnEdit.TabIndex = 12;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseSelectable = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// tabNotifications
			// 
			this.tabNotifications.Controls.Add(this.grdNotifications);
			this.tabNotifications.HorizontalScrollbarBarColor = true;
			this.tabNotifications.HorizontalScrollbarHighlightOnWheel = false;
			this.tabNotifications.HorizontalScrollbarSize = 10;
			this.tabNotifications.Location = new System.Drawing.Point(4, 38);
			this.tabNotifications.Name = "tabNotifications";
			this.tabNotifications.Size = new System.Drawing.Size(996, 462);
			this.tabNotifications.TabIndex = 3;
			this.tabNotifications.Text = "Notifications";
			this.tabNotifications.VerticalScrollbarBarColor = true;
			this.tabNotifications.VerticalScrollbarHighlightOnWheel = false;
			this.tabNotifications.VerticalScrollbarSize = 10;
			// 
			// grdNotifications
			// 
			this.grdNotifications.AllowUserToAddRows = false;
			this.grdNotifications.AllowUserToDeleteRows = false;
			this.grdNotifications.AllowUserToResizeRows = false;
			this.grdNotifications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grdNotifications.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.grdNotifications.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.grdNotifications.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.grdNotifications.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdNotifications.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.grdNotifications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdNotifications.DefaultCellStyle = dataGridViewCellStyle5;
			this.grdNotifications.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.grdNotifications.EnableHeadersVisualStyles = false;
			this.grdNotifications.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdNotifications.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.grdNotifications.Location = new System.Drawing.Point(3, 3);
			this.grdNotifications.Name = "grdNotifications";
			this.grdNotifications.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdNotifications.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.grdNotifications.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.grdNotifications.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdNotifications.Size = new System.Drawing.Size(993, 456);
			this.grdNotifications.TabIndex = 2;
			this.grdNotifications.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdNotifications_CellContentClick);
			// 
			// tabSettings
			// 
			this.tabSettings.Controls.Add(this.chcBoxAutoUpdate);
			this.tabSettings.Controls.Add(this.chcBoxPlaySound);
			this.tabSettings.HorizontalScrollbarBarColor = true;
			this.tabSettings.HorizontalScrollbarHighlightOnWheel = false;
			this.tabSettings.HorizontalScrollbarSize = 10;
			this.tabSettings.Location = new System.Drawing.Point(4, 38);
			this.tabSettings.Name = "tabSettings";
			this.tabSettings.Size = new System.Drawing.Size(996, 462);
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
			this.chcBoxAutoUpdate.Size = new System.Drawing.Size(112, 15);
			this.chcBoxAutoUpdate.TabIndex = 4;
			this.chcBoxAutoUpdate.Text = "Look for updates";
			this.metroToolTip1.SetToolTip(this.chcBoxAutoUpdate, "Looks for updates when program starts.");
			this.chcBoxAutoUpdate.UseSelectable = true;
			this.chcBoxAutoUpdate.CheckedChanged += new System.EventHandler(this.chcBoxAutoUpdate_CheckedChanged);
			// 
			// chcBoxPlaySound
			// 
			this.chcBoxPlaySound.AutoSize = true;
			this.chcBoxPlaySound.Checked = true;
			this.chcBoxPlaySound.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chcBoxPlaySound.Location = new System.Drawing.Point(3, 3);
			this.chcBoxPlaySound.Name = "chcBoxPlaySound";
			this.chcBoxPlaySound.Size = new System.Drawing.Size(81, 15);
			this.chcBoxPlaySound.TabIndex = 2;
			this.chcBoxPlaySound.Text = "Play sound";
			this.metroToolTip1.SetToolTip(this.chcBoxPlaySound, "Plays sound on notification.");
			this.chcBoxPlaySound.UseSelectable = true;
			this.chcBoxPlaySound.CheckedChanged += new System.EventHandler(this.chcBoxPlaySound_CheckedChanged);
			// 
			// metroToolTip1
			// 
			this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
			this.metroToolTip1.StyleManager = null;
			this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
			// 
			// Id
			// 
			this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.Id.HeaderText = "Id";
			this.Id.Name = "Id";
			this.Id.ReadOnly = true;
			this.Id.Width = 40;
			// 
			// Currency
			// 
			this.Currency.HeaderText = "Currency";
			this.Currency.Name = "Currency";
			this.Currency.ReadOnly = true;
			// 
			// Exchange
			// 
			this.Exchange.HeaderText = "Exchange";
			this.Exchange.Name = "Exchange";
			this.Exchange.ReadOnly = true;
			// 
			// AlertType
			// 
			this.AlertType.HeaderText = "Alert type";
			this.AlertType.Name = "AlertType";
			this.AlertType.ReadOnly = true;
			// 
			// Condition
			// 
			this.Condition.HeaderText = "Condition";
			this.Condition.Name = "Condition";
			this.Condition.ReadOnly = true;
			// 
			// Timeframe
			// 
			this.Timeframe.HeaderText = "Timeframe";
			this.Timeframe.Name = "Timeframe";
			this.Timeframe.ReadOnly = true;
			// 
			// Price
			// 
			this.Price.HeaderText = "Price";
			this.Price.Name = "Price";
			this.Price.ReadOnly = true;
			// 
			// IndicatorValue
			// 
			this.IndicatorValue.HeaderText = "Indicator Value";
			this.IndicatorValue.Name = "IndicatorValue";
			this.IndicatorValue.ReadOnly = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1050, 590);
			this.Controls.Add(this.tabControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "Crypto Watcher";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Resize += new System.EventHandler(this.MainResize);
			this.tabControl.ResumeLayout(false);
			this.tabAlert.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdAlerts)).EndInit();
			this.tabNotifications.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdNotifications)).EndInit();
			this.tabSettings.ResumeLayout(false);
			this.tabSettings.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ntfyIconMinimized;
        private MetroFramework.Controls.MetroButton btnNew;
        private MetroFramework.Controls.MetroButton btnRemove;
        private MetroFramework.Controls.MetroTabControl tabControl;
        private MetroFramework.Controls.MetroTabPage tabAlert;
        private MetroFramework.Controls.MetroTabPage tabSettings;
        private MetroFramework.Controls.MetroCheckBox chcBoxPlaySound;
        private MetroFramework.Controls.MetroCheckBox chcBoxAutoUpdate;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
		private MetroFramework.Controls.MetroTabPage tabNotifications;
		private MetroFramework.Controls.MetroGrid grdNotifications;
		private MetroFramework.Controls.MetroButton btnEdit;
		private MetroFramework.Controls.MetroGrid grdAlerts;
		private System.Windows.Forms.DataGridViewTextBoxColumn Id;
		private System.Windows.Forms.DataGridViewTextBoxColumn Currency;
		private System.Windows.Forms.DataGridViewTextBoxColumn Exchange;
		private System.Windows.Forms.DataGridViewTextBoxColumn AlertType;
		private System.Windows.Forms.DataGridViewTextBoxColumn Condition;
		private System.Windows.Forms.DataGridViewTextBoxColumn Timeframe;
		private System.Windows.Forms.DataGridViewTextBoxColumn Price;
		private System.Windows.Forms.DataGridViewTextBoxColumn IndicatorValue;
	}
}

