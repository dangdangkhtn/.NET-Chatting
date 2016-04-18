using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Delta
{
    public partial class Form_CreateAccount : Form
    {
        private TCPNetDriver_Client netDriver = new TCPNetDriver_Client();

        bool usrIsInteracted = false;
        bool pwdIsInteracted = false;

        public Form_CreateAccount()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!usrIsInteracted || !pwdIsInteracted 
                || txt_username.Text == "" || txt_password.Text == "")
            {
                MessageBox.Show("Please enter username/password");
            }
            // TODO: Check username/password by RegularExpression
            else
            {
                string usrname = txt_username.Text;
                string pwd = txt_password.Text;
                try
                {
                    netDriver.Connect_toDeltaSE_Server();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }

                bool regStatus = netDriver.Register(txt_username.Text, txt_password.Text);
                if (regStatus == true)
                {
                    MessageBox.Show("Register Successful");
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("This username is used, please chose other name");
                }                                    
            }            
        }

        private void txt_username_Click(object sender, EventArgs e)
        {
            if(!usrIsInteracted)
            {
                txt_username.Text = "";
                txt_username.ForeColor = Color.Black;
            }
            this.usrIsInteracted = true;           
        }

        private void txt_password_Click(object sender, EventArgs e)
        {
            if (!pwdIsInteracted)
            {
                txt_password.Text = "";
                txt_password.ForeColor = Color.Black;
                txt_password.UseSystemPasswordChar = true;
            }
            this.pwdIsInteracted = true;
        }
    }
}
