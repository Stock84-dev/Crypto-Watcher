namespace CryptoWatcher
{
    partial class CustomAlertForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomAlertForm));
			this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
			this.chcBoxWindowsMsg = new MetroFramework.Controls.MetroCheckBox();
			this.cBoxInterval = new MetroFramework.Controls.MetroComboBox();
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.btnAddAlert = new MetroFramework.Controls.MetroButton();
			this.tabControl = new MetroFramework.Controls.MetroTabControl();
			this.tabCondition = new MetroFramework.Controls.MetroTabPage();
			this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
			this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
			this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
			this.cboxExchange = new MetroFramework.Controls.MetroComboBox();
			this.cboxMarket = new MetroFramework.Controls.MetroComboBox();
			this.txtSymbol = new MetroFramework.Controls.MetroTextBox();
			this.cboxCondition = new MetroFramework.Controls.MetroComboBox();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.panel = new MetroFramework.Controls.MetroPanel();
			this.tabNotification = new MetroFramework.Controls.MetroTabPage();
			this.lblInterval = new MetroFramework.Controls.MetroLabel();
			this.txtInterval = new MetroFramework.Controls.MetroTextBox();
			this.chcBoxRepeatable = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
			this.cBoxSound = new MetroFramework.Controls.MetroComboBox();
			this.chcBoxShowWindow = new MetroFramework.Controls.MetroCheckBox();
			this.lblError = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tabCondition.SuspendLayout();
			this.metroPanel1.SuspendLayout();
			this.tabNotification.SuspendLayout();
			this.SuspendLayout();
			// 
			// metroToolTip1
			// 
			this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
			this.metroToolTip1.StyleManager = null;
			this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
			// 
			// chcBoxWindowsMsg
			// 
			this.chcBoxWindowsMsg.AutoSize = true;
			this.chcBoxWindowsMsg.Checked = true;
			this.chcBoxWindowsMsg.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chcBoxWindowsMsg.Location = new System.Drawing.Point(3, 38);
			this.chcBoxWindowsMsg.Name = "chcBoxWindowsMsg";
			this.chcBoxWindowsMsg.Size = new System.Drawing.Size(151, 15);
			this.chcBoxWindowsMsg.TabIndex = 7;
			this.chcBoxWindowsMsg.Text = "Show windows message";
			this.metroToolTip1.SetToolTip(this.chcBoxWindowsMsg, "Shows message");
			this.chcBoxWindowsMsg.UseSelectable = true;
			// 
			// cBoxInterval
			// 
			this.cBoxInterval.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cBoxInterval.FormattingEnabled = true;
			this.cBoxInterval.ItemHeight = 19;
			this.cBoxInterval.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days",
            "Weeks",
            "Months"});
			this.cBoxInterval.Location = new System.Drawing.Point(195, 70);
			this.cBoxInterval.Name = "cBoxInterval";
			this.cBoxInterval.Size = new System.Drawing.Size(75, 25);
			this.cBoxInterval.TabIndex = 23;
			this.metroToolTip1.SetToolTip(this.cBoxInterval, "Measuring unit.");
			this.cBoxInterval.UseSelectable = true;
			this.cBoxInterval.Visible = false;
			// 
			// metroLabel1
			// 
			this.metroLabel1.AutoSize = true;
			this.metroLabel1.Location = new System.Drawing.Point(2, 11);
			this.metroLabel1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(87, 19);
			this.metroLabel1.TabIndex = 31;
			this.metroLabel1.Text = "Coin Symbol:";
			this.metroToolTip1.SetToolTip(this.metroLabel1, "Use coin symbol from coinmarketcap.com");
			// 
			// btnAddAlert
			// 
			this.btnAddAlert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAddAlert.Location = new System.Drawing.Point(130, 233);
			this.btnAddAlert.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.btnAddAlert.Name = "btnAddAlert";
			this.btnAddAlert.Size = new System.Drawing.Size(124, 26);
			this.btnAddAlert.TabIndex = 5;
			this.btnAddAlert.Text = "Add";
			this.btnAddAlert.UseSelectable = true;
			this.btnAddAlert.Click += new System.EventHandler(this.btnAddAlert_Click);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabCondition);
			this.tabControl.Controls.Add(this.tabNotification);
			this.tabControl.Location = new System.Drawing.Point(21, 26);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(722, 204);
			this.tabControl.TabIndex = 19;
			this.tabControl.UseSelectable = true;
			// 
			// tabCondition
			// 
			this.tabCondition.Controls.Add(this.metroPanel1);
			this.tabCondition.Controls.Add(this.panel);
			this.tabCondition.HorizontalScrollbarBarColor = true;
			this.tabCondition.HorizontalScrollbarHighlightOnWheel = false;
			this.tabCondition.HorizontalScrollbarSize = 10;
			this.tabCondition.Location = new System.Drawing.Point(4, 38);
			this.tabCondition.Name = "tabCondition";
			this.tabCondition.Size = new System.Drawing.Size(714, 162);
			this.tabCondition.TabIndex = 0;
			this.tabCondition.Text = "Condition";
			this.tabCondition.VerticalScrollbarBarColor = true;
			this.tabCondition.VerticalScrollbarHighlightOnWheel = false;
			this.tabCondition.VerticalScrollbarSize = 10;
			// 
			// metroPanel1
			// 
			this.metroPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.metroPanel1.Controls.Add(this.metroLabel4);
			this.metroPanel1.Controls.Add(this.metroLabel5);
			this.metroPanel1.Controls.Add(this.cboxExchange);
			this.metroPanel1.Controls.Add(this.cboxMarket);
			this.metroPanel1.Controls.Add(this.metroLabel1);
			this.metroPanel1.Controls.Add(this.txtSymbol);
			this.metroPanel1.Controls.Add(this.cboxCondition);
			this.metroPanel1.Controls.Add(this.metroLabel2);
			this.metroPanel1.HorizontalScrollbarBarColor = true;
			this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
			this.metroPanel1.HorizontalScrollbarSize = 10;
			this.metroPanel1.Location = new System.Drawing.Point(4, 7);
			this.metroPanel1.Name = "metroPanel1";
			this.metroPanel1.Size = new System.Drawing.Size(268, 152);
			this.metroPanel1.TabIndex = 22;
			this.metroPanel1.VerticalScrollbarBarColor = true;
			this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
			this.metroPanel1.VerticalScrollbarSize = 10;
			// 
			// metroLabel4
			// 
			this.metroLabel4.AutoSize = true;
			this.metroLabel4.Location = new System.Drawing.Point(22, 128);
			this.metroLabel4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.metroLabel4.Name = "metroLabel4";
			this.metroLabel4.Size = new System.Drawing.Size(67, 19);
			this.metroLabel4.TabIndex = 33;
			this.metroLabel4.Text = "Exchange:";
			// 
			// metroLabel5
			// 
			this.metroLabel5.AutoSize = true;
			this.metroLabel5.Location = new System.Drawing.Point(36, 89);
			this.metroLabel5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.metroLabel5.Name = "metroLabel5";
			this.metroLabel5.Size = new System.Drawing.Size(53, 19);
			this.metroLabel5.TabIndex = 34;
			this.metroLabel5.Text = "Market:";
			// 
			// cboxExchange
			// 
			this.cboxExchange.FormattingEnabled = true;
			this.cboxExchange.ItemHeight = 23;
			this.cboxExchange.Location = new System.Drawing.Point(101, 118);
			this.cboxExchange.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.cboxExchange.Name = "cboxExchange";
			this.cboxExchange.Size = new System.Drawing.Size(124, 29);
			this.cboxExchange.TabIndex = 30;
			this.cboxExchange.UseSelectable = true;
			// 
			// cboxMarket
			// 
			this.cboxMarket.FormattingEnabled = true;
			this.cboxMarket.IntegralHeight = false;
			this.cboxMarket.ItemHeight = 23;
			this.cboxMarket.Location = new System.Drawing.Point(101, 79);
			this.cboxMarket.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.cboxMarket.Name = "cboxMarket";
			this.cboxMarket.Size = new System.Drawing.Size(124, 29);
			this.cboxMarket.TabIndex = 29;
			this.cboxMarket.UseSelectable = true;
			this.cboxMarket.SelectedIndexChanged += new System.EventHandler(this.cboxMarket_SelectedIndexChanged);
			// 
			// txtSymbol
			// 
			this.txtSymbol.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtSymbol.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			// 
			// 
			// 
			this.txtSymbol.CustomButton.Image = null;
			this.txtSymbol.CustomButton.Location = new System.Drawing.Point(96, 1);
			this.txtSymbol.CustomButton.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.txtSymbol.CustomButton.Name = "";
			this.txtSymbol.CustomButton.Size = new System.Drawing.Size(27, 27);
			this.txtSymbol.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.txtSymbol.CustomButton.TabIndex = 1;
			this.txtSymbol.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.txtSymbol.CustomButton.UseSelectable = true;
			this.txtSymbol.CustomButton.Visible = false;
			this.txtSymbol.Lines = new string[0];
			this.txtSymbol.Location = new System.Drawing.Point(101, 1);
			this.txtSymbol.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.txtSymbol.MaxLength = 32767;
			this.txtSymbol.Name = "txtSymbol";
			this.txtSymbol.PasswordChar = '\0';
			this.txtSymbol.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.txtSymbol.SelectedText = "";
			this.txtSymbol.SelectionLength = 0;
			this.txtSymbol.SelectionStart = 0;
			this.txtSymbol.ShortcutsEnabled = true;
			this.txtSymbol.Size = new System.Drawing.Size(124, 29);
			this.txtSymbol.TabIndex = 27;
			this.txtSymbol.UseSelectable = true;
			this.txtSymbol.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.txtSymbol.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			this.txtSymbol.TextChanged += new System.EventHandler(this.txtSymbol_TextChanged);
			// 
			// cboxCondition
			// 
			this.cboxCondition.FormattingEnabled = true;
			this.cboxCondition.ItemHeight = 23;
			this.cboxCondition.Location = new System.Drawing.Point(101, 40);
			this.cboxCondition.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.cboxCondition.Name = "cboxCondition";
			this.cboxCondition.Size = new System.Drawing.Size(124, 29);
			this.cboxCondition.TabIndex = 28;
			this.cboxCondition.UseSelectable = true;
			this.cboxCondition.SelectedIndexChanged += new System.EventHandler(this.cboxCondition_SelectedIndexChanged);
			// 
			// metroLabel2
			// 
			this.metroLabel2.AutoSize = true;
			this.metroLabel2.Location = new System.Drawing.Point(20, 50);
			this.metroLabel2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(69, 19);
			this.metroLabel2.TabIndex = 32;
			this.metroLabel2.Text = "Condition:";
			// 
			// panel
			// 
			this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel.HorizontalScrollbarBarColor = true;
			this.panel.HorizontalScrollbarHighlightOnWheel = false;
			this.panel.HorizontalScrollbarSize = 10;
			this.panel.Location = new System.Drawing.Point(278, 7);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(433, 152);
			this.panel.TabIndex = 21;
			this.panel.VerticalScrollbarBarColor = true;
			this.panel.VerticalScrollbarHighlightOnWheel = false;
			this.panel.VerticalScrollbarSize = 10;
			this.panel.Visible = false;
			// 
			// tabNotification
			// 
			this.tabNotification.Controls.Add(this.lblInterval);
			this.tabNotification.Controls.Add(this.cBoxInterval);
			this.tabNotification.Controls.Add(this.txtInterval);
			this.tabNotification.Controls.Add(this.chcBoxRepeatable);
			this.tabNotification.Controls.Add(this.metroLabel6);
			this.tabNotification.Controls.Add(this.cBoxSound);
			this.tabNotification.Controls.Add(this.chcBoxShowWindow);
			this.tabNotification.Controls.Add(this.chcBoxWindowsMsg);
			this.tabNotification.HorizontalScrollbarBarColor = true;
			this.tabNotification.HorizontalScrollbarHighlightOnWheel = false;
			this.tabNotification.HorizontalScrollbarSize = 10;
			this.tabNotification.Location = new System.Drawing.Point(4, 38);
			this.tabNotification.Name = "tabNotification";
			this.tabNotification.Size = new System.Drawing.Size(714, 162);
			this.tabNotification.TabIndex = 1;
			this.tabNotification.Text = "Notification";
			this.tabNotification.VerticalScrollbarBarColor = true;
			this.tabNotification.VerticalScrollbarHighlightOnWheel = false;
			this.tabNotification.VerticalScrollbarSize = 10;
			// 
			// lblInterval
			// 
			this.lblInterval.AutoSize = true;
			this.lblInterval.FontSize = MetroFramework.MetroLabelSize.Small;
			this.lblInterval.Location = new System.Drawing.Point(87, 80);
			this.lblInterval.Name = "lblInterval";
			this.lblInterval.Size = new System.Drawing.Size(46, 15);
			this.lblInterval.TabIndex = 24;
			this.lblInterval.Text = "Interval:";
			this.lblInterval.Visible = false;
			// 
			// txtInterval
			// 
			// 
			// 
			// 
			this.txtInterval.CustomButton.Image = null;
			this.txtInterval.CustomButton.Location = new System.Drawing.Point(26, 1);
			this.txtInterval.CustomButton.Name = "";
			this.txtInterval.CustomButton.Size = new System.Drawing.Size(23, 23);
			this.txtInterval.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.txtInterval.CustomButton.TabIndex = 1;
			this.txtInterval.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.txtInterval.CustomButton.UseSelectable = true;
			this.txtInterval.CustomButton.Visible = false;
			this.txtInterval.Lines = new string[0];
			this.txtInterval.Location = new System.Drawing.Point(139, 70);
			this.txtInterval.MaxLength = 32767;
			this.txtInterval.Name = "txtInterval";
			this.txtInterval.PasswordChar = '\0';
			this.txtInterval.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.txtInterval.SelectedText = "";
			this.txtInterval.SelectionLength = 0;
			this.txtInterval.SelectionStart = 0;
			this.txtInterval.ShortcutsEnabled = true;
			this.txtInterval.Size = new System.Drawing.Size(50, 25);
			this.txtInterval.TabIndex = 22;
			this.txtInterval.UseSelectable = true;
			this.txtInterval.Visible = false;
			this.txtInterval.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.txtInterval.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// chcBoxRepeatable
			// 
			this.chcBoxRepeatable.AutoSize = true;
			this.chcBoxRepeatable.Location = new System.Drawing.Point(3, 80);
			this.chcBoxRepeatable.Name = "chcBoxRepeatable";
			this.chcBoxRepeatable.Size = new System.Drawing.Size(81, 15);
			this.chcBoxRepeatable.TabIndex = 21;
			this.chcBoxRepeatable.Text = "Repeatable";
			this.chcBoxRepeatable.UseSelectable = true;
			this.chcBoxRepeatable.CheckedChanged += new System.EventHandler(this.chcBoxRepeatable_CheckedChanged);
			// 
			// metroLabel6
			// 
			this.metroLabel6.AutoSize = true;
			this.metroLabel6.Location = new System.Drawing.Point(3, 9);
			this.metroLabel6.Name = "metroLabel6";
			this.metroLabel6.Size = new System.Drawing.Size(49, 19);
			this.metroLabel6.TabIndex = 20;
			this.metroLabel6.Text = "Sound:";
			// 
			// cBoxSound
			// 
			this.cBoxSound.FontSize = MetroFramework.MetroComboBoxSize.Small;
			this.cBoxSound.FormattingEnabled = true;
			this.cBoxSound.ItemHeight = 19;
			this.cBoxSound.Items.AddRange(new object[] {
            "Loop Sound",
            "Play Sound",
            "No Sound"});
			this.cBoxSound.Location = new System.Drawing.Point(58, 3);
			this.cBoxSound.Name = "cBoxSound";
			this.cBoxSound.Size = new System.Drawing.Size(100, 25);
			this.cBoxSound.TabIndex = 9;
			this.cBoxSound.UseSelectable = true;
			// 
			// chcBoxShowWindow
			// 
			this.chcBoxShowWindow.AutoSize = true;
			this.chcBoxShowWindow.Checked = true;
			this.chcBoxShowWindow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chcBoxShowWindow.Location = new System.Drawing.Point(3, 59);
			this.chcBoxShowWindow.Name = "chcBoxShowWindow";
			this.chcBoxShowWindow.Size = new System.Drawing.Size(97, 15);
			this.chcBoxShowWindow.TabIndex = 8;
			this.chcBoxShowWindow.Text = "Show window";
			this.chcBoxShowWindow.UseSelectable = true;
			// 
			// lblError
			// 
			this.lblError.AutoSize = true;
			this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblError.ForeColor = System.Drawing.Color.Red;
			this.lblError.Location = new System.Drawing.Point(28, 244);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(0, 15);
			this.lblError.TabIndex = 20;
			// 
			// CustomAlertForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(764, 284);
			this.Controls.Add(this.lblError);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.btnAddAlert);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.Name = "CustomAlertForm";
			this.Padding = new System.Windows.Forms.Padding(18, 60, 18, 20);
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.VisibleChanged += new System.EventHandler(this.CustomAlertForm_VisibleChanged);
			this.tabControl.ResumeLayout(false);
			this.tabCondition.ResumeLayout(false);
			this.metroPanel1.ResumeLayout(false);
			this.metroPanel1.PerformLayout();
			this.tabNotification.ResumeLayout(false);
			this.tabNotification.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private MetroFramework.Controls.MetroButton btnAddAlert;
		private MetroFramework.Controls.MetroTabControl tabControl;
		private MetroFramework.Controls.MetroTabPage tabCondition;
		private MetroFramework.Controls.MetroTabPage tabNotification;
		private MetroFramework.Controls.MetroCheckBox chcBoxShowWindow;
		private MetroFramework.Controls.MetroCheckBox chcBoxWindowsMsg;
		private MetroFramework.Controls.MetroLabel metroLabel6;
		private MetroFramework.Controls.MetroComboBox cBoxSound;
		private MetroFramework.Controls.MetroComboBox cBoxInterval;
		private MetroFramework.Controls.MetroTextBox txtInterval;
		private MetroFramework.Controls.MetroCheckBox chcBoxRepeatable;
		private MetroFramework.Controls.MetroLabel lblInterval;
		private MetroFramework.Controls.MetroPanel panel;
		private MetroFramework.Controls.MetroPanel metroPanel1;
		private MetroFramework.Controls.MetroLabel metroLabel4;
		private MetroFramework.Controls.MetroLabel metroLabel5;
		private MetroFramework.Controls.MetroComboBox cboxExchange;
		private MetroFramework.Controls.MetroComboBox cboxMarket;
		private MetroFramework.Controls.MetroLabel metroLabel1;
		private MetroFramework.Controls.MetroTextBox txtSymbol;
		private MetroFramework.Controls.MetroComboBox cboxCondition;
		private MetroFramework.Controls.MetroLabel metroLabel2;
		private System.Windows.Forms.Label lblError;
	}
}