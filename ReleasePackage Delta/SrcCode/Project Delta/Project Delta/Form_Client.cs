using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Delta
{
    public partial class Form_Client : Form
    {
        TCPNetDriver_Client netDriver;
        List<String> chattingUsers = new List<String>();

        public delegate void FormChattingHandler(object sender, EventArgs args);
        public event FormChattingHandler MessageReceivedFromNetDriver;

        public Form_Client(String userName, TCPNetDriver_Client mainDriver)
        {
            InitializeComponent();
            this.lab_username.Text = userName;
            netDriver = mainDriver;
            netDriver.MessageReceived += new TCPNetDriver_Client.FormClientHandler(frm_WhenSomeOneContacts);
            netDriver.NewUserLoggedIn += new TCPNetDriver_Client.FormClientNewUserOnlineHandler(frm_WhenSomeOneLoggedIn);
        }

        private void Form_Client_Load(object sender, EventArgs e)
        {
            RefreshGlobalRoom();
        }

        //Load List Of User And Refresh The Visualization
        public void RefreshGlobalRoom()
        {
            netDriver.Send_GetListRequest(this.lab_username.Text);
            if (netDriver.listUserOnline.Count != 0)
            {
                this.listBox1.Items.Clear();
                foreach (String usr in netDriver.listUserOnline)
                {
                    this.listBox1.Items.Add(usr);
                }
            }
        }

        //ref: http://stackoverflow.com/questions/4454423/c-sharp-listbox-item-double-click-event
        void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                string selectedName = listBox1.GetItemText(listBox1.SelectedItem);
                Form_Chatting formChat = new Form_Chatting(selectedName, this, netDriver);
                this.chattingUsers.Add(selectedName);
                formChat.Show();
            }
        }

        // --------- Cross-Thread Processing------------------------------------------------------
        public delegate void CallmehtodbyClientFormThread(ListBox control, int index, string item, string firstmsg);
        public void Listbox1_OpenChatSessionWith(ListBox control, int index, string item,string firstmsg)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new CallmehtodbyClientFormThread(Listbox1_OpenChatSessionWith),
                    new object[] { control, index, item, firstmsg});  // invoking itself
            }
            else
            {
                if (index != System.Windows.Forms.ListBox.NoMatches)
                {
                    string selectedName = item;
                    Form_Chatting formChat = new Form_Chatting(selectedName, this, netDriver);
                    this.chattingUsers.Add(selectedName);
                    formChat.listBox1.Items.Add(firstmsg);
                    formChat.Show();                    
                }
                else {
                    RefreshGlobalRoom();
                }                
            }                
        } 

        // ref: http://www.perceler.com/articles1.php?art=crossthreads1
        public delegate string ControlStringProducer(ListBox control);  // defines a delegate type
        public string GetText(ListBox control)
        {
            if (control.InvokeRequired)
            {
                return (string)control.Invoke(new ControlStringProducer(GetText), new object[] { control });  // invoking itself
            }
            else
            {
                string result = control.GetItemText(control.SelectedItem);
                return result; // the "functional part", executing only on the main thread
            }
        }

        public delegate int ControlIndexProducer(ListBox control, string item);  // defines a delegate type
        public int GetIndex(ListBox control, string item)
        {
            if (control.InvokeRequired)
            {
                return (int)control.Invoke(new ControlIndexProducer(GetIndex), new object[] { control , item });  // invoking itself
            }
            else
            {
                int result = control.Items.IndexOf(item);
                return result; // the "functional part", executing only on the main thread
            }
        }

        public delegate void ListBoxProcessor(ListBox control, string username);  // defines a delegate type
        public void AddNewUserToListBox(ListBox control, string username)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new ListBoxProcessor(AddNewUserToListBox), new object[] { control, username});  // invoking itself
            }
            else
            {
                this.listBox1.Items.Add(username); // the "functional part", executing only on the main thread
            }
        }

        //Called when received a message from someone
        //Called from thread of netDriver. So, use invoke to make cross-thread call;
        // ref: https://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k(EHInvalidOperation.WinForms.IllegalCrossThreadCall);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5.2);k(DevLang-csharp)&rd=true
        // ref: http://stackoverflow.com/questions/14703698/invokedelegate        
        //Called by delegate handler of netDriver

        public void frm_WhenSomeOneContacts(object sender,object sender2, EventArgs args)
        {

            string srcUser = (string)sender;
            string msg = (string)sender2;
            // if srcUser doesn't have session with me yet                        
            if (!this.chattingUsers.Contains(srcUser))
            {
                this.chattingUsers.Add(srcUser);      
                int index = GetIndex(this.listBox1, srcUser);
                string newMessage = "'" + srcUser + "' says: " + msg;
                Listbox1_OpenChatSessionWith(listBox1, index ,srcUser,newMessage);
                Thread.Sleep(300);
            }
            else //srcUser have session with me already
            {
                string newMessage = "'" + srcUser + "' says: "+ msg;
                // Make the chatting form which is having session get new message;
                MessageReceivedFromNetDriver(newMessage, new EventArgs());
            }
        }

        public void frm_WhenSomeOneLoggedIn(string username, EventArgs args)
        {
            AddNewUserToListBox(listBox1, username);
        }
    }
}
