namespace MaxAdsl_PP_Net
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
            this.lblResponse = new System.Windows.Forms.Label();
            this.btnCheckTraffic = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTraffic = new System.Windows.Forms.TabPage();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.lblCheckTrafficOnStartup = new System.Windows.Forms.Label();
            this.cboCheckTrafficOnStartup = new System.Windows.Forms.CheckBox();
            this.lblWebParserType = new System.Windows.Forms.Label();
            this.cboWebType = new System.Windows.Forms.ComboBox();
            this.lblSettingsResponse = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabTraffic.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(6, 3);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(65, 13);
            this.lblResponse.TabIndex = 0;
            this.lblResponse.Text = "lblResponse";
            // 
            // btnCheckTraffic
            // 
            this.btnCheckTraffic.Location = new System.Drawing.Point(422, 205);
            this.btnCheckTraffic.Name = "btnCheckTraffic";
            this.btnCheckTraffic.Size = new System.Drawing.Size(75, 23);
            this.btnCheckTraffic.TabIndex = 1;
            this.btnCheckTraffic.Text = "Check";
            this.btnCheckTraffic.UseVisualStyleBackColor = true;
            this.btnCheckTraffic.Click += new System.EventHandler(this.btnCheckTraffic_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTraffic);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(511, 262);
            this.tabControl1.TabIndex = 7;
            // 
            // tabTraffic
            // 
            this.tabTraffic.Controls.Add(this.lblResponse);
            this.tabTraffic.Controls.Add(this.btnCheckTraffic);
            this.tabTraffic.Location = new System.Drawing.Point(4, 22);
            this.tabTraffic.Name = "tabTraffic";
            this.tabTraffic.Padding = new System.Windows.Forms.Padding(3);
            this.tabTraffic.Size = new System.Drawing.Size(503, 236);
            this.tabTraffic.TabIndex = 0;
            this.tabTraffic.Text = "Traffic";
            this.tabTraffic.UseVisualStyleBackColor = true;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.lblCheckTrafficOnStartup);
            this.tabSettings.Controls.Add(this.cboCheckTrafficOnStartup);
            this.tabSettings.Controls.Add(this.lblWebParserType);
            this.tabSettings.Controls.Add(this.cboWebType);
            this.tabSettings.Controls.Add(this.lblSettingsResponse);
            this.tabSettings.Controls.Add(this.lblUsername);
            this.tabSettings.Controls.Add(this.btnSaveSettings);
            this.tabSettings.Controls.Add(this.txtUsername);
            this.tabSettings.Controls.Add(this.lblPassword);
            this.tabSettings.Controls.Add(this.txtPassword);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(503, 236);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // lblCheckTrafficOnStartup
            // 
            this.lblCheckTrafficOnStartup.AutoSize = true;
            this.lblCheckTrafficOnStartup.Location = new System.Drawing.Point(6, 89);
            this.lblCheckTrafficOnStartup.Name = "lblCheckTrafficOnStartup";
            this.lblCheckTrafficOnStartup.Size = new System.Drawing.Size(91, 13);
            this.lblCheckTrafficOnStartup.TabIndex = 16;
            this.lblCheckTrafficOnStartup.Text = "Check on startup:";
            // 
            // cboCheckTrafficOnStartup
            // 
            this.cboCheckTrafficOnStartup.AutoSize = true;
            this.cboCheckTrafficOnStartup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cboCheckTrafficOnStartup.Location = new System.Drawing.Point(116, 89);
            this.cboCheckTrafficOnStartup.Name = "cboCheckTrafficOnStartup";
            this.cboCheckTrafficOnStartup.Size = new System.Drawing.Size(15, 14);
            this.cboCheckTrafficOnStartup.TabIndex = 15;
            this.cboCheckTrafficOnStartup.UseVisualStyleBackColor = true;
            // 
            // lblWebParserType
            // 
            this.lblWebParserType.AutoSize = true;
            this.lblWebParserType.Location = new System.Drawing.Point(8, 62);
            this.lblWebParserType.Name = "lblWebParserType";
            this.lblWebParserType.Size = new System.Drawing.Size(102, 13);
            this.lblWebParserType.TabIndex = 14;
            this.lblWebParserType.Text = "Data from web type:";
            // 
            // cboWebType
            // 
            this.cboWebType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWebType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboWebType.FormattingEnabled = true;
            this.cboWebType.Location = new System.Drawing.Point(116, 59);
            this.cboWebType.Name = "cboWebType";
            this.cboWebType.Size = new System.Drawing.Size(100, 21);
            this.cboWebType.TabIndex = 13;
            // 
            // lblSettingsResponse
            // 
            this.lblSettingsResponse.AutoSize = true;
            this.lblSettingsResponse.Location = new System.Drawing.Point(8, 210);
            this.lblSettingsResponse.Name = "lblSettingsResponse";
            this.lblSettingsResponse.Size = new System.Drawing.Size(103, 13);
            this.lblSettingsResponse.TabIndex = 12;
            this.lblSettingsResponse.Text = "lblSettingsResponse";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(8, 9);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.TabIndex = 8;
            this.lblUsername.Text = "Username:";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(420, 205);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSettings.TabIndex = 11;
            this.btnSaveSettings.Text = "Save settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(116, 6);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 7;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(8, 35);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(116, 32);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 262);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabTraffic.ResumeLayout(false);
            this.tabTraffic.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.Button btnCheckTraffic;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTraffic;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblSettingsResponse;
        private System.Windows.Forms.Label lblWebParserType;
        private System.Windows.Forms.ComboBox cboWebType;
        private System.Windows.Forms.Label lblCheckTrafficOnStartup;
        private System.Windows.Forms.CheckBox cboCheckTrafficOnStartup;
    }
}

