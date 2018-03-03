namespace CryptoWatcher.Launcher
{
    partial class LauncherForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
			this.lbl = new System.Windows.Forms.Label();
			this.btnNo = new MetroFramework.Controls.MetroButton();
			this.btnYes = new MetroFramework.Controls.MetroButton();
			this.txtBox = new MetroFramework.Controls.MetroTextBox();
			this.lbl1 = new MetroFramework.Controls.MetroLabel();
			this.lblCurrentVersion = new MetroFramework.Controls.MetroLabel();
			this.lblNewVersion = new MetroFramework.Controls.MetroLabel();
			this.SuspendLayout();
			// 
			// lbl
			// 
			this.lbl.AutoSize = true;
			this.lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lbl.Location = new System.Drawing.Point(23, 29);
			this.lbl.Name = "lbl";
			this.lbl.Size = new System.Drawing.Size(430, 18);
			this.lbl.TabIndex = 1;
			this.lbl.Text = "New version is avalible, do you want to update Crypto Watcher ?";
			// 
			// btnNo
			// 
			this.btnNo.Location = new System.Drawing.Point(386, 212);
			this.btnNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(75, 22);
			this.btnNo.TabIndex = 4;
			this.btnNo.Text = "No";
			this.btnNo.UseSelectable = true;
			this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// 
			// btnYes
			// 
			this.btnYes.Location = new System.Drawing.Point(21, 212);
			this.btnYes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(75, 22);
			this.btnYes.TabIndex = 3;
			this.btnYes.Text = "Yes";
			this.btnYes.UseSelectable = true;
			this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
			// 
			// txtBox
			// 
			// 
			// 
			// 
			this.txtBox.CustomButton.Image = null;
			this.txtBox.CustomButton.Location = new System.Drawing.Point(358, 2);
			this.txtBox.CustomButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.txtBox.CustomButton.Name = "";
			this.txtBox.CustomButton.Size = new System.Drawing.Size(79, 79);
			this.txtBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.txtBox.CustomButton.TabIndex = 1;
			this.txtBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.txtBox.CustomButton.UseSelectable = true;
			this.txtBox.CustomButton.Visible = false;
			this.txtBox.Lines = new string[0];
			this.txtBox.Location = new System.Drawing.Point(21, 123);
			this.txtBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.txtBox.MaxLength = 32767;
			this.txtBox.Multiline = true;
			this.txtBox.Name = "txtBox";
			this.txtBox.PasswordChar = '\0';
			this.txtBox.ReadOnly = true;
			this.txtBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.txtBox.SelectedText = "";
			this.txtBox.SelectionLength = 0;
			this.txtBox.SelectionStart = 0;
			this.txtBox.ShortcutsEnabled = true;
			this.txtBox.Size = new System.Drawing.Size(440, 84);
			this.txtBox.TabIndex = 5;
			this.txtBox.UseSelectable = true;
			this.txtBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.txtBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// lbl1
			// 
			this.lbl1.AutoSize = true;
			this.lbl1.Location = new System.Drawing.Point(20, 100);
			this.lbl1.Name = "lbl1";
			this.lbl1.Size = new System.Drawing.Size(76, 19);
			this.lbl1.TabIndex = 6;
			this.lbl1.Text = "Changelog:";
			// 
			// lblCurrentVersion
			// 
			this.lblCurrentVersion.AutoSize = true;
			this.lblCurrentVersion.Location = new System.Drawing.Point(20, 62);
			this.lblCurrentVersion.Name = "lblCurrentVersion";
			this.lblCurrentVersion.Size = new System.Drawing.Size(132, 19);
			this.lblCurrentVersion.TabIndex = 7;
			this.lblCurrentVersion.Text = "Current version: 1.0.0";
			// 
			// lblNewVersion
			// 
			this.lblNewVersion.AutoSize = true;
			this.lblNewVersion.Location = new System.Drawing.Point(20, 81);
			this.lblNewVersion.Name = "lblNewVersion";
			this.lblNewVersion.Size = new System.Drawing.Size(114, 19);
			this.lblNewVersion.TabIndex = 8;
			this.lblNewVersion.Text = "New version: 1.1.0";
			// 
			// LauncherForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(488, 256);
			this.Controls.Add(this.lblNewVersion);
			this.Controls.Add(this.lblCurrentVersion);
			this.Controls.Add(this.lbl1);
			this.Controls.Add(this.txtBox);
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.btnYes);
			this.Controls.Add(this.lbl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "LauncherForm";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl;
        private MetroFramework.Controls.MetroButton btnNo;
        private MetroFramework.Controls.MetroButton btnYes;
        private MetroFramework.Controls.MetroTextBox txtBox;
        private MetroFramework.Controls.MetroLabel lbl1;
        private MetroFramework.Controls.MetroLabel lblCurrentVersion;
        private MetroFramework.Controls.MetroLabel lblNewVersion;
    }
}

