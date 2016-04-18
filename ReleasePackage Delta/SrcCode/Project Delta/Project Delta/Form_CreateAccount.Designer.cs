namespace Project_Delta
{
    partial class Form_CreateAccount
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
            this.txt_password = new System.Windows.Forms.TextBox();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_password
            // 
            this.txt_password.Font = new System.Drawing.Font("Segoe UI Light", 14F);
            this.txt_password.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_password.Location = new System.Drawing.Point(57, 84);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(269, 32);
            this.txt_password.TabIndex = 0;
            this.txt_password.Text = "Password";
            this.txt_password.Click += new System.EventHandler(this.txt_password_Click);
            // 
            // txt_username
            // 
            this.txt_username.Font = new System.Drawing.Font("Segoe UI Light", 14F);
            this.txt_username.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_username.Location = new System.Drawing.Point(57, 36);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(269, 32);
            this.txt_username.TabIndex = 1;
            this.txt_username.Text = "User Name";
            this.txt_username.Click += new System.EventHandler(this.txt_username_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(57, 138);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(269, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "Sign up";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form_CreateAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 225);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_username);
            this.Controls.Add(this.txt_password);
            this.Name = "Form_CreateAccount";
            this.Text = "Create account";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.Button button1;
    }
}