using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Project_Delta
{
    public partial class Form1 : Form
    {
        private TCPNetDriver_Client netDriver = new TCPNetDriver_Client();
        // ref http://stackoverflow.com/questions/837488/how-can-i-get-the-applications-path-in-a-net-console-application
        string appDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private Dictionary<string, string> Form_Credentials = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFromCache();
        }

        //Start of Classes for JSON: "Credential", "RootObject"---------------------------------------------------------
        //Generated Automatically by: http://json2csharp.com/
        public class Credential
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class JSONRootObject
        {
            public List<Credential> Credentials { get; set; }
        }
        //End of Classes for JSON: "Credential", "RootObject"---------------------------------------------------------

        //ref https://msdn.microsoft.com/en-us/library/system.windows.forms.combobox.selectionchangecommitted.aspx
        private void comboBoxLogin_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;     
            txt_password.Text = Form_Credentials[senderComboBox.SelectedItem.ToString()]; 
        }

        private void lab_createAcc_Click(object sender, EventArgs e)
        {
            Form createAccFrm = new Form_CreateAccount();
            createAccFrm.Show();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            String userName = cb_login.Text;
            String password = txt_password.Text;

            if (userName == "")
            {
                MessageBox.Show("Please enter username");
            }
            else
            {                                 
                try
                {
                    // TODO: Create an notification for logging in process here
                    netDriver.Connect_toDeltaSE_Server();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                                
                if(RunningMode.Default.Mode == "Development")
                {
                    netDriver.Send_LoginRequest(userName);
                    Form_Client formClient = new Form_Client(userName, netDriver);
                    formClient.Show();
                }
                else // Production mode
                {
                    bool temp = netDriver.Send_LoginRequest(userName, password);
                    if (temp == true)
                    {
                        this.Hide();
                        Form_Client formClient = new Form_Client(userName, netDriver);
                        formClient.Show();
                        if (cheb_rememberAcc.Checked == true)
                        {
                            SaveToCache(userName,password);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Credential");
                    }
                }      
            }
        }

        private void lab_createAcc_MouseHover(object sender, EventArgs e)
        {
            this.lab_createAcc.ForeColor = Color.Navy;
            this.lab_createAcc.Font = new Font(this.lab_createAcc.Font, FontStyle.Italic);
        }

        private void lab_createAcc_MouseLeave(object sender, EventArgs e)
        {
            this.lab_createAcc.ForeColor = Color.Black;
            this.lab_createAcc.Font = new Font(this.lab_createAcc.Font, FontStyle.Regular);
        }

        private void LoadFromCache()
        {
            string directoryPath = "Cache";
            string jsonPath = directoryPath + @"\Credentials.json";             
            if (System.IO.Directory.Exists(directoryPath))
            {

                JSONRootObject rootObj = JsonConvert.DeserializeObject<JSONRootObject>(
                                           File.ReadAllText(jsonPath));
                if (rootObj != null)
                {
                    foreach (Credential cre in rootObj.Credentials)
                    {
                        this.cb_login.Items.Add(cre.username);
                        this.Form_Credentials.Add(cre.username, cre.password);
                    }
                }
            }                    
        }

        private void SaveToCache(string username, string password)
        {
            string directoryPath = "Cache";
            string jsonPath = directoryPath + @"\Credentials.json";
            // 1. Check if Cache folder exist or not, if not exitst, create "Cache\Credentials.json"
            try
            {
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                    System.IO.FileStream fs = new System.IO.FileStream(jsonPath, System.IO.FileMode.Create);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }

            // 2. Update Json file"
            Form1.JSONRootObject rootObj = JsonConvert.DeserializeObject<Form1.JSONRootObject>(
                                                           File.ReadAllText(jsonPath));
            string result = "Successful";
            if (rootObj != null)
            {
                Form1.Credential newUser = new Form1.Credential();
                newUser.username = username;
                newUser.password = password;
                if (!rootObj.Credentials.Contains(newUser))
                {
                    rootObj.Credentials.Add(newUser);
                    File.WriteAllText(jsonPath, JsonConvert.SerializeObject(rootObj));
                }
            }
        }


    }       
}

