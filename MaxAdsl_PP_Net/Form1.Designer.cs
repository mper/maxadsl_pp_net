namespace MaxAdsl_PP_Net
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
            this.lblResponse = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(13, 33);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(0, 13);
            this.lblResponse.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(272, 8);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(37, 10);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 2;
            this.txtUsername.Text = "test";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(13, 13);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(18, 13);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "U:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(143, 13);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(17, 13);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "P:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(166, 10);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Text = "test1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 262);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblResponse);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
    }
}

