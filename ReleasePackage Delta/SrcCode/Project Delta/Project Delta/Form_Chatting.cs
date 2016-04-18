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
    public partial class Form_Chatting : Form
    {
        TCPNetDriver_Client netDriver;
        Form_Client parentForm;
        string typingMessage = "";

        /// <summary>
        /// This contructor will be called by 'Form_Client' when:
        /// DoubleClick from 'Form_Client' to open chatting form
        /// </summary>
        /// <param name="destinationUserName"></param>
        /// <param name="mainDriver"></param>
        public Form_Chatting(string destinationUserName, Form_Client parent, TCPNetDriver_Client mainDriver)
        {
            InitializeComponent();
            netDriver = mainDriver;
            parentForm = parent;
            this.Text = destinationUserName;
        }

        /// <summary>
        /// This contructor will be called by 'Form_Client.frm_WhenSomeOneContacts'
        /// </summary>
        /// <param name="srcUserName">The user who contacts to me</param>
        /// <param name="msg">messageBody</param>
        /// <param name="parent">'form_Chatting' will get message automatically from this param by using delegate</param>
        /// <param name="mainDriver">'form_Chatting' can send message via this param</param>
        public Form_Chatting(string srcUserName, string msg, Form_Client parent, TCPNetDriver_Client mainDriver)
        {
            InitializeComponent();
            netDriver = mainDriver;
            parentForm = parent;
            this.Text = srcUserName;
        }        

        private void Form_Chatting_Load(object sender, EventArgs e)
        {            
            parentForm.MessageReceivedFromNetDriver += new Form_Client.FormChattingHandler(frm_WhenReceiveAMessage);
        }

  
        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.listBox1.Items.Add(this.richTextBox2.Text);
                netDriver.Send_MessageTo(this.Text, this.richTextBox2.Text);
                this.richTextBox2.Clear();
                richTextBox2.SelectionStart = 0;
                richTextBox2.Focus();                
            }
        }
        // --------- Cross-Thread Processing------------------------------------------------------
        // ref: http://www.perceler.com/articles1.php?art=crossthreads1
        public delegate void ControlStringConsumer(string msg);  // defines a delegate type
        public void AddToChatLog(string msg)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new ControlStringConsumer(AddToChatLog), new object[] { msg });  // invoking itself
            }
            else
            {
                // the "functional part", executing only on the main thread
                listBox1.Items.Add(msg);
            }
        }

        public void frm_WhenReceiveAMessage(object sender, EventArgs args)
        {
            string msg = (string)sender;
            AddToChatLog(msg);            
        }
    }
}
