namespace CryptoWatcher.Alerts
{
	partial class AlertFormEditor

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertFormEditor));
			this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.chcBoxWindowsMsg = new MetroFramework.Controls.MetroCheckBox();
			this.cBoxInterval = new MetroFramework.Controls.MetroComboBox();
			this.pBox5Min = new System.Windows.Forms.PictureBox();
			this.btnAddAlert = new MetroFramework.Controls.MetroButton();
			this.txtSymbol = new MetroFramework.Controls.MetroTextBox();
			this.cboxCondition = new MetroFramework.Controls.MetroComboBox();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.cboxExchange = new MetroFramework.Controls.MetroComboBox();
			this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
			this.cboxMarket = new MetroFramework.Controls.MetroComboBox();
			this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
			this.tabControl = new MetroFramework.Controls.MetroTabControl();
			this.tabCondition = new MetroFramework.Controls.MetroTabPage();
			this.panel = new MetroFramework.Controls.MetroPanel();
			this.txtD = new MetroFramework.Controls.MetroTextBox();
			this.lblD = new MetroFramework.Controls.MetroLabel();
			this.txtK = new MetroFramework.Controls.MetroTextBox();
			this.lblK = new MetroFramework.Controls.MetroLabel();
			this.lblTimeframe = new MetroFramework.Controls.MetroLabel();
			this.cBoxTimeframe = new MetroFramework.Controls.MetroComboBox();
			this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
			this.cBoxType = new MetroFramework.Controls.MetroComboBox();
			this.txtValue = new MetroFramework.Controls.MetroTextBox();
			this.cBoxCondition1 = new MetroFramework.Controls.MetroComboBox();
			this.lblValue = new MetroFramework.Controls.MetroLabel();
			this.lblCondition = new MetroFramework.Controls.MetroLabel();
			this.tabNotification = new MetroFramework.Controls.MetroTabPage();
			this.lblInterval = new MetroFramework.Controls.MetroLabel();
			this.txtInterval = new MetroFramework.Controls.MetroTextBox();
			this.chcBoxRepeatable = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
			this.cBoxSound = new MetroFramework.Controls.MetroComboBox();
			this.chcBoxShowWindow = new MetroFramework.Controls.MetroCheckBox();
			this.lblError = new MetroFramework.Controls.MetroLabel();
			this.metroComboBox1 = new MetroFramework.Controls.MetroComboBox();
			this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
			((System.ComponentModel.ISupportInitialize)(this.pBox5Min)).BeginInit();
			this.tabControl.SuspendLayout();
			this.tabCondition.SuspendLayout();
			this.panel.SuspendLayout();
			this.tabNotification.SuspendLayout();
			this.SuspendLayout();
			// 
			// metroToolTip1
			// 
			this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
			this.metroToolTip1.StyleManager = null;
			this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
			// 
			// metroLabel1
			// 
			this.metroLabel1.AutoSize = true;
			this.metroLabel1.Location = new System.Drawing.Point(6, 17);
			this.metroLabel1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(87, 19);
			this.metroLabel1.TabIndex = 9;
			this.metroLabel1.Text = "Coin Symbol:";
			this.metroToolTip1.SetToolTip(this.metroLabel1, "Use coin symbol from coinmarketcap.com");
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
			// pBox5Min
			// 
			this.pBox5Min.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.pBox5Min.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pBox5Min.Image = global::CryptoWatcher.Properties.Resources._5min;
			this.pBox5Min.Location = new System.Drawing.Point(238, 124);
			this.pBox5Min.Name = "pBox5Min";
			this.pBox5Min.Size = new System.Drawing.Size(34, 29);
			this.pBox5Min.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pBox5Min.TabIndex = 19;
			this.pBox5Min.TabStop = false;
			this.metroToolTip1.SetToolTip(this.pBox5Min, "Prices update every 5 minutes.");
			this.pBox5Min.Visible = false;
			// 
			// btnAddAlert
			// 
			this.btnAddAlert.Location = new System.Drawing.Point(130, 233);
			this.btnAddAlert.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.btnAddAlert.Name = "btnAddAlert";
			this.btnAddAlert.Size = new System.Drawing.Size(124, 26);
			this.btnAddAlert.TabIndex = 5;
			this.btnAddAlert.Text = "Add";
			this.btnAddAlert.UseSelectable = true;
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
			this.txtSymbol.Location = new System.Drawing.Point(105, 7);
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
			this.txtSymbol.TabIndex = 0;
			this.txtSymbol.UseSelectable = true;
			this.txtSymbol.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.txtSymbol.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// cboxCondition
			// 
			this.cboxCondition.FormattingEnabled = true;
			this.cboxCondition.ItemHeight = 23;
			this.cboxCondition.Items.AddRange(new object[] {
            "Higher than",
            "Higher or equal than",
            "Lower than",
            "Lower or equal than",
            "Overbought RSI",
            "Oversold RSI"});
			this.cboxCondition.Location = new System.Drawing.Point(105, 46);
			this.cboxCondition.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.cboxCondition.Name = "cboxCondition";
			this.cboxCondition.Size = new System.Drawing.Size(124, 29);
			this.cboxCondition.TabIndex = 1;
			this.cboxCondition.UseSelectable = true;
			// 
			// metroLabel2
			// 
			this.metroLabel2.AutoSize = true;
			this.metroLabel2.Location = new System.Drawing.Point(24, 56);
			this.metroLabel2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(69, 19);
			this.metroLabel2.TabIndex = 11;
			this.metroLabel2.Text = "Condition:";
			// 
			// cboxExchange
			// 
			this.cboxExchange.FormattingEnabled = true;
			this.cboxExchange.ItemHeight = 23;
			this.cboxExchange.Location = new System.Drawing.Point(105, 124);
			this.cboxExchange.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.cboxExchange.Name = "cboxExchange";
			this.cboxExchange.Size = new System.Drawing.Size(124, 29);
			this.cboxExchange.TabIndex = 4;
			this.cboxExchange.UseSelectable = true;
			// 
			// metroLabel4
			// 
			this.metroLabel4.AutoSize = true;
			this.metroLabel4.Location = new System.Drawing.Point(26, 134);
			this.metroLabel4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.metroLabel4.Name = "metroLabel4";
			this.metroLabel4.Size = new System.Drawing.Size(67, 19);
			this.metroLabel4.TabIndex = 16;
			this.metroLabel4.Text = "Exchange:";
			// 
			// cboxMarket
			// 
			this.cboxMarket.FormattingEnabled = true;
			this.cboxMarket.IntegralHeight = false;
			this.cboxMarket.ItemHeight = 23;
			this.cboxMarket.Location = new System.Drawing.Point(105, 85);
			this.cboxMarket.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.cboxMarket.Name = "cboxMarket";
			this.cboxMarket.Size = new System.Drawing.Size(124, 29);
			this.cboxMarket.TabIndex = 2;
			this.cboxMarket.UseSelectable = true;
			// 
			// metroLabel5
			// 
			this.metroLabel5.AutoSize = true;
			this.metroLabel5.Location = new System.Drawing.Point(40, 95);
			this.metroLabel5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.metroLabel5.Name = "metroLabel5";
			this.metroLabel5.Size = new System.Drawing.Size(53, 19);
			this.metroLabel5.TabIndex = 18;
			this.metroLabel5.Text = "Market:";
			// 
			// tabControl
			// 
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
			this.tabCondition.Controls.Add(this.panel);
			this.tabCondition.Controls.Add(this.pBox5Min);
			this.tabCondition.Controls.Add(this.metroLabel4);
			this.tabCondition.Controls.Add(this.metroLabel5);
			this.tabCondition.Controls.Add(this.cboxExchange);
			this.tabCondition.Controls.Add(this.cboxMarket);
			this.tabCondition.Controls.Add(this.metroLabel1);
			this.tabCondition.Controls.Add(this.txtSymbol);
			this.tabCondition.Controls.Add(this.cboxCondition);
			this.tabCondition.Controls.Add(this.metroLabel2);
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
			// panel
			// 
			this.panel.Controls.Add(this.metroLabel3);
			this.panel.Controls.Add(this.metroComboBox1);
			this.panel.Controls.Add(this.txtD);
			this.panel.Controls.Add(this.lblD);
			this.panel.Controls.Add(this.txtK);
			this.panel.Controls.Add(this.lblK);
			this.panel.Controls.Add(this.lblTimeframe);
			this.panel.Controls.Add(this.cBoxTimeframe);
			this.panel.Controls.Add(this.metroLabel8);
			this.panel.Controls.Add(this.cBoxType);
			this.panel.Controls.Add(this.txtValue);
			this.panel.Controls.Add(this.cBoxCondition1);
			this.panel.Controls.Add(this.lblValue);
			this.panel.Controls.Add(this.lblCondition);
			this.panel.HorizontalScrollbarBarColor = true;
			this.panel.HorizontalScrollbarHighlightOnWheel = false;
			this.panel.HorizontalScrollbarSize = 10;
			this.panel.Location = new System.Drawing.Point(278, 7);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(433, 146);
			this.panel.TabIndex = 21;
			this.panel.VerticalScrollbarBarColor = true;
			this.panel.VerticalScrollbarHighlightOnWheel = false;
			this.panel.VerticalScrollbarSize = 10;
			// 
			// txtD
			// 
			// 
			// 
			// 
			this.txtD.CustomButton.Image = null;
			this.txtD.CustomButton.Location = new System.Drawing.Point(96, 1);
			this.txtD.CustomButton.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.txtD.CustomButton.Name = "";
			this.txtD.CustomButton.Size = new System.Drawing.Size(27, 27);
			this.txtD.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.txtD.CustomButton.TabIndex = 1;
			this.txtD.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.txtD.CustomButton.UseSelectable = true;
			this.txtD.CustomButton.Visible = false;
			this.txtD.Lines = new string[0];
			this.txtD.Location = new System.Drawing.Point(309, 78);
			this.txtD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.txtD.MaxLength = 32767;
			this.txtD.Name = "txtD";
			this.txtD.PasswordChar = '\0';
			this.txtD.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.txtD.SelectedText = "";
			this.txtD.SelectionLength = 0;
			this.txtD.SelectionStart = 0;
			this.txtD.ShortcutsEnabled = true;
			this.txtD.Size = new System.Drawing.Size(124, 29);
			this.txtD.TabIndex = 27;
			this.txtD.UseSelectable = true;
			this.txtD.Visible = false;
			this.txtD.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.txtD.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// lblD
			// 
			this.lblD.AutoSize = true;
			this.lblD.Location = new System.Drawing.Point(221, 88);
			this.lblD.Name = "lblD";
			this.lblD.Size = new System.Drawing.Size(79, 19);
			this.lblD.TabIndex = 26;
			this.lblD.Text = "Slow length:";
			// 
			// txtK
			// 
			// 
			// 
			// 
			this.txtK.CustomButton.Image = null;
			this.txtK.CustomButton.Location = new System.Drawing.Point(96, 1);
			this.txtK.CustomButton.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.txtK.CustomButton.Name = "";
			this.txtK.CustomButton.Size = new System.Drawing.Size(27, 27);
			this.txtK.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.txtK.CustomButton.TabIndex = 1;
			this.txtK.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.txtK.CustomButton.UseSelectable = true;
			this.txtK.CustomButton.Visible = false;
			this.txtK.Lines = new string[0];
			this.txtK.Location = new System.Drawing.Point(309, 39);
			this.txtK.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.txtK.MaxLength = 32767;
			this.txtK.Name = "txtK";
			this.txtK.PasswordChar = '\0';
			this.txtK.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.txtK.SelectedText = "";
			this.txtK.SelectionLength = 0;
			this.txtK.SelectionStart = 0;
			this.txtK.ShortcutsEnabled = true;
			this.txtK.Size = new System.Drawing.Size(124, 29);
			this.txtK.TabIndex = 25;
			this.txtK.UseSelectable = true;
			this.txtK.Visible = false;
			this.txtK.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.txtK.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// lblK
			// 
			this.lblK.AutoSize = true;
			this.lblK.Location = new System.Drawing.Point(225, 49);
			this.lblK.Name = "lblK";
			this.lblK.Size = new System.Drawing.Size(75, 19);
			this.lblK.TabIndex = 23;
			this.lblK.Text = "Fast length:";
			// 
			// lblTimeframe
			// 
			this.lblTimeframe.AutoSize = true;
			this.lblTimeframe.Location = new System.Drawing.Point(0, 49);
			this.lblTimeframe.Name = "lblTimeframe";
			this.lblTimeframe.Size = new System.Drawing.Size(76, 19);
			this.lblTimeframe.TabIndex = 21;
			this.lblTimeframe.Text = "Timeframe:";
			// 
			// cBoxTimeframe
			// 
			this.cBoxTimeframe.FormattingEnabled = true;
			this.cBoxTimeframe.ItemHeight = 23;
			this.cBoxTimeframe.Items.AddRange(new object[] {
            "placeholder1",
            "2"});
			this.cBoxTimeframe.Location = new System.Drawing.Point(85, 39);
			this.cBoxTimeframe.Name = "cBoxTimeframe";
			this.cBoxTimeframe.Size = new System.Drawing.Size(124, 29);
			this.cBoxTimeframe.TabIndex = 22;
			this.cBoxTimeframe.UseSelectable = true;
			// 
			// metroLabel8
			// 
			this.metroLabel8.AutoSize = true;
			this.metroLabel8.Location = new System.Drawing.Point(36, 10);
			this.metroLabel8.Name = "metroLabel8";
			this.metroLabel8.Size = new System.Drawing.Size(40, 19);
			this.metroLabel8.TabIndex = 13;
			this.metroLabel8.Text = "Type:";
			// 
			// cBoxType
			// 
			this.cBoxType.FormattingEnabled = true;
			this.cBoxType.ItemHeight = 23;
			this.cBoxType.Items.AddRange(new object[] {
            "Crossing",
            "Higher",
            "Higher or equal",
            "Lower",
            "Lower or equal"});
			this.cBoxType.Location = new System.Drawing.Point(85, 0);
			this.cBoxType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.cBoxType.Name = "cBoxType";
			this.cBoxType.Size = new System.Drawing.Size(124, 29);
			this.cBoxType.TabIndex = 12;
			this.cBoxType.UseSelectable = true;
			// 
			// txtValue
			// 
			// 
			// 
			// 
			this.txtValue.CustomButton.Image = null;
			this.txtValue.CustomButton.Location = new System.Drawing.Point(96, 1);
			this.txtValue.CustomButton.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.txtValue.CustomButton.Name = "";
			this.txtValue.CustomButton.Size = new System.Drawing.Size(27, 27);
			this.txtValue.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.txtValue.CustomButton.TabIndex = 1;
			this.txtValue.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.txtValue.CustomButton.UseSelectable = true;
			this.txtValue.CustomButton.Visible = false;
			this.txtValue.Lines = new string[0];
			this.txtValue.Location = new System.Drawing.Point(85, 117);
			this.txtValue.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.txtValue.MaxLength = 32767;
			this.txtValue.Name = "txtValue";
			this.txtValue.PasswordChar = '\0';
			this.txtValue.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.txtValue.SelectedText = "";
			this.txtValue.SelectionLength = 0;
			this.txtValue.SelectionStart = 0;
			this.txtValue.ShortcutsEnabled = true;
			this.txtValue.Size = new System.Drawing.Size(124, 29);
			this.txtValue.TabIndex = 10;
			this.txtValue.UseSelectable = true;
			this.txtValue.Visible = false;
			this.txtValue.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.txtValue.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// cBoxCondition1
			// 
			this.cBoxCondition1.FormattingEnabled = true;
			this.cBoxCondition1.ItemHeight = 23;
			this.cBoxCondition1.Items.AddRange(new object[] {
            "Crossing",
            "Higher",
            "Higher or equal",
            "Lower",
            "Lower or equal"});
			this.cBoxCondition1.Location = new System.Drawing.Point(85, 78);
			this.cBoxCondition1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.cBoxCondition1.Name = "cBoxCondition1";
			this.cBoxCondition1.Size = new System.Drawing.Size(124, 29);
			this.cBoxCondition1.TabIndex = 11;
			this.cBoxCondition1.UseSelectable = true;
			this.cBoxCondition1.Visible = false;
			// 
			// lblValue
			// 
			this.lblValue.AutoSize = true;
			this.lblValue.Location = new System.Drawing.Point(32, 127);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(44, 19);
			this.lblValue.TabIndex = 9;
			this.lblValue.Text = "Value:";
			this.lblValue.Visible = false;
			// 
			// lblCondition
			// 
			this.lblCondition.AutoSize = true;
			this.lblCondition.Location = new System.Drawing.Point(7, 88);
			this.lblCondition.Name = "lblCondition";
			this.lblCondition.Size = new System.Drawing.Size(69, 19);
			this.lblCondition.TabIndex = 8;
			this.lblCondition.Text = "Condition:";
			this.lblCondition.Visible = false;
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
			this.lblError.Location = new System.Drawing.Point(21, 293);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(0, 0);
			this.lblError.TabIndex = 20;
			// 
			// metroComboBox1
			// 
			this.metroComboBox1.FormattingEnabled = true;
			this.metroComboBox1.ItemHeight = 23;
			this.metroComboBox1.Items.AddRange(new object[] {
            "placeholder1",
            "2"});
			this.metroComboBox1.Location = new System.Drawing.Point(309, 0);
			this.metroComboBox1.Name = "metroComboBox1";
			this.metroComboBox1.Size = new System.Drawing.Size(124, 29);
			this.metroComboBox1.TabIndex = 30;
			this.metroComboBox1.UseSelectable = true;
			// 
			// metroLabel3
			// 
			this.metroLabel3.AutoSize = true;
			this.metroLabel3.Location = new System.Drawing.Point(270, 10);
			this.metroLabel3.Name = "metroLabel3";
			this.metroLabel3.Size = new System.Drawing.Size(30, 19);
			this.metroLabel3.TabIndex = 31;
			this.metroLabel3.Text = "On:";
			// 
			// AlertFormEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(764, 284);
			this.Controls.Add(this.lblError);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.btnAddAlert);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.Name = "AlertFormEditor";
			this.Padding = new System.Windows.Forms.Padding(18, 60, 18, 20);
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			((System.ComponentModel.ISupportInitialize)(this.pBox5Min)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.tabCondition.ResumeLayout(false);
			this.tabCondition.PerformLayout();
			this.panel.ResumeLayout(false);
			this.panel.PerformLayout();
			this.tabNotification.ResumeLayout(false);
			this.tabNotification.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private MetroFramework.Components.MetroToolTip metroToolTip1;
		private MetroFramework.Controls.MetroButton btnAddAlert;
		private MetroFramework.Controls.MetroTextBox txtSymbol;
		private MetroFramework.Controls.MetroLabel metroLabel1;
		private MetroFramework.Controls.MetroComboBox cboxCondition;
		private MetroFramework.Controls.MetroLabel metroLabel2;
		private MetroFramework.Controls.MetroComboBox cboxExchange;
		private MetroFramework.Controls.MetroLabel metroLabel4;
		private MetroFramework.Controls.MetroComboBox cboxMarket;
		private MetroFramework.Controls.MetroLabel metroLabel5;
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
		private MetroFramework.Controls.MetroLabel lblError;
		private System.Windows.Forms.PictureBox pBox5Min;
		private MetroFramework.Controls.MetroPanel panel;
		private MetroFramework.Controls.MetroTextBox txtValue;
		private MetroFramework.Controls.MetroComboBox cBoxCondition1;
		private MetroFramework.Controls.MetroLabel lblValue;
		private MetroFramework.Controls.MetroLabel lblCondition;
		private MetroFramework.Controls.MetroLabel metroLabel8;
		private MetroFramework.Controls.MetroComboBox cBoxType;
		private MetroFramework.Controls.MetroLabel lblTimeframe;
		private MetroFramework.Controls.MetroComboBox cBoxTimeframe;
		private MetroFramework.Controls.MetroTextBox txtK;
		private MetroFramework.Controls.MetroLabel lblK;
		private MetroFramework.Controls.MetroTextBox txtD;
		private MetroFramework.Controls.MetroLabel lblD;
		private MetroFramework.Controls.MetroLabel metroLabel3;
		private MetroFramework.Controls.MetroComboBox metroComboBox1;
	}
}