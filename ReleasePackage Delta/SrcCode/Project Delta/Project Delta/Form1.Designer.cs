namespace Project_Delta
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
            this.btn_login = new System.Windows.Forms.Button();
            this.cb_login = new System.Windows.Forms.ComboBox();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.cheb_rememberAcc = new System.Windows.Forms.CheckBox();
            this.lab_createAcc = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(16, 244);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(75, 23);
            this.btn_login.TabIndex = 0;
            this.btn_login.Text = "Login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // cb_login
            // 
            this.cb_login.FormattingEnabled = true;
            this.cb_login.Location = new System.Drawing.Point(16, 86);
            this.cb_login.Name = "cb_login";
            this.cb_login.Size = new System.Drawing.Size(194, 21);
            this.cb_login.TabIndex = 1;
            this.cb_login.SelectionChangeCommitted += new System.EventHandler(this.comboBoxLogin_SelectionChangeCommitted);
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(16, 128);
            this.txt_password.Name = "txt_password";
            this.txt_password.PasswordChar = '*';
            this.txt_password.Size = new System.Drawing.Size(194, 20);
            this.txt_password.TabIndex = 2;
            // 
            // cheb_rememberAcc
            // 
            this.cheb_rememberAcc.AutoSize = true;
            this.cheb_rememberAcc.Location = new System.Drawing.Point(16, 168);
            this.cheb_rememberAcc.Name = "cheb_rememberAcc";
            this.cheb_rememberAcc.Size = new System.Drawing.Size(138, 17);
            this.cheb_rememberAcc.TabIndex = 3;
            this.cheb_rememberAcc.Text = "Remember this account";
            this.cheb_rememberAcc.UseVisualStyleBackColor = true;
            // 
            // lab_createAcc
            // 
            this.lab_createAcc.AutoSize = true;
            this.lab_createAcc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_createAcc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lab_createAcc.Location = new System.Drawing.Point(13, 306);
            this.lab_createAcc.Name = "lab_createAcc";
            this.lab_createAcc.Size = new System.Drawing.Size(95, 13);
            this.lab_createAcc.TabIndex = 4;
            this.lab_createAcc.Text = "Create an account";
            this.lab_createAcc.Click += new System.EventHandler(this.lab_createAcc_Click);
            this.lab_createAcc.MouseLeave += new System.EventHandler(this.lab_createAcc_MouseLeave);
            this.lab_createAcc.MouseHover += new System.EventHandler(this.lab_createAcc_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "User Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 328);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lab_createAcc);
            this.Controls.Add(this.cheb_rememberAcc);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.cb_login);
            this.Controls.Add(this.btn_login);
            this.Name = "Form1";
            this.Text = "Delta";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.ComboBox cb_login;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.CheckBox cheb_rememberAcc;
        private System.Windows.Forms.Label lab_createAcc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

