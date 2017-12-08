namespace Crypto_watcher
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
            this.btnAddAlert = new MetroFramework.Controls.MetroButton();
            this.txtSymbol = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.cboxCondition = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.txtPrice = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.cboxBtcPrice = new MetroFramework.Controls.MetroCheckBox();
            this.SuspendLayout();
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // btnAddAlert
            // 
            this.btnAddAlert.Location = new System.Drawing.Point(123, 139);
            this.btnAddAlert.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnAddAlert.Name = "btnAddAlert";
            this.btnAddAlert.Size = new System.Drawing.Size(78, 26);
            this.btnAddAlert.TabIndex = 7;
            this.btnAddAlert.Text = "Add";
            this.btnAddAlert.UseSelectable = true;
            this.btnAddAlert.Click += new System.EventHandler(this.btnAddAlert_Click);
            // 
            // txtSymbol
            // 
            // 
            // 
            // 
            this.txtSymbol.CustomButton.Image = null;
            this.txtSymbol.CustomButton.Location = new System.Drawing.Point(78, 2);
            this.txtSymbol.CustomButton.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.txtSymbol.CustomButton.Name = "";
            this.txtSymbol.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtSymbol.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtSymbol.CustomButton.TabIndex = 1;
            this.txtSymbol.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSymbol.CustomButton.UseSelectable = true;
            this.txtSymbol.CustomButton.Visible = false;
            this.txtSymbol.Lines = new string[0];
            this.txtSymbol.Location = new System.Drawing.Point(123, 20);
            this.txtSymbol.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.txtSymbol.MaxLength = 32767;
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.PasswordChar = '\0';
            this.txtSymbol.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSymbol.SelectedText = "";
            this.txtSymbol.SelectionLength = 0;
            this.txtSymbol.SelectionStart = 0;
            this.txtSymbol.ShortcutsEnabled = true;
            this.txtSymbol.Size = new System.Drawing.Size(102, 26);
            this.txtSymbol.TabIndex = 8;
            this.txtSymbol.UseSelectable = true;
            this.txtSymbol.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSymbol.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(24, 27);
            this.metroLabel1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(87, 19);
            this.metroLabel1.TabIndex = 9;
            this.metroLabel1.Text = "Coin Symbol:";
            this.metroToolTip1.SetToolTip(this.metroLabel1, "Use coin symbol from coinmarketcap.com");
            // 
            // cboxCondition
            // 
            this.cboxCondition.FormattingEnabled = true;
            this.cboxCondition.ItemHeight = 23;
            this.cboxCondition.Items.AddRange(new object[] {
            "Higher than",
            "Lower than"});
            this.cboxCondition.Location = new System.Drawing.Point(123, 60);
            this.cboxCondition.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.cboxCondition.Name = "cboxCondition";
            this.cboxCondition.Size = new System.Drawing.Size(124, 29);
            this.cboxCondition.TabIndex = 10;
            this.cboxCondition.UseSelectable = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(42, 70);
            this.metroLabel2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(69, 19);
            this.metroLabel2.TabIndex = 11;
            this.metroLabel2.Text = "Condition:";
            // 
            // txtPrice
            // 
            // 
            // 
            // 
            this.txtPrice.CustomButton.Image = null;
            this.txtPrice.CustomButton.Location = new System.Drawing.Point(69, 2);
            this.txtPrice.CustomButton.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.txtPrice.CustomButton.Name = "";
            this.txtPrice.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtPrice.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPrice.CustomButton.TabIndex = 1;
            this.txtPrice.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPrice.CustomButton.UseSelectable = true;
            this.txtPrice.CustomButton.Visible = false;
            this.txtPrice.Lines = new string[0];
            this.txtPrice.Location = new System.Drawing.Point(123, 100);
            this.txtPrice.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.txtPrice.MaxLength = 32767;
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.PasswordChar = '\0';
            this.txtPrice.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPrice.SelectedText = "";
            this.txtPrice.SelectionLength = 0;
            this.txtPrice.SelectionStart = 0;
            this.txtPrice.ShortcutsEnabled = true;
            this.txtPrice.Size = new System.Drawing.Size(93, 26);
            this.txtPrice.TabIndex = 12;
            this.txtPrice.UseSelectable = true;
            this.txtPrice.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPrice.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(70, 107);
            this.metroLabel3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(41, 19);
            this.metroLabel3.TabIndex = 13;
            this.metroLabel3.Text = "Price:";
            // 
            // cboxBtcPrice
            // 
            this.cboxBtcPrice.AutoSize = true;
            this.cboxBtcPrice.Location = new System.Drawing.Point(228, 111);
            this.cboxBtcPrice.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.cboxBtcPrice.Name = "cboxBtcPrice";
            this.cboxBtcPrice.Size = new System.Drawing.Size(85, 15);
            this.cboxBtcPrice.TabIndex = 14;
            this.cboxBtcPrice.Text = "BTC Market";
            this.cboxBtcPrice.UseSelectable = true;
            // 
            // CustomAlertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 185);
            this.Controls.Add(this.cboxBtcPrice);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.cboxCondition);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.txtSymbol);
            this.Controls.Add(this.btnAddAlert);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Name = "CustomAlertForm";
            this.Padding = new System.Windows.Forms.Padding(18, 60, 18, 20);
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
        private MetroFramework.Controls.MetroTextBox txtPrice;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroCheckBox cboxBtcPrice;
    }
}