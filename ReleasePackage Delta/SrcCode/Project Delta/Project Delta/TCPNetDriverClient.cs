using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Project_Delta
{
    public class TCPNetDriver_Client
    {
        private int port = 49512;
        private TcpClient socketTCP;
        private Thread incomingMessageHandler;        
        private StreamWriter strWritter;
        private StreamReader strReader;
        
        public bool isConnectedToServer;        
        public List<String> listUserOnline;

        public delegate void FormClientHandler(object sender,object sender2, EventArgs args);
        public event FormClientHandler MessageReceived;

        public delegate void FormClientNewUserOnlineHandler(string username, EventArgs args);
        public event FormClientNewUserOnlineHandler NewUserLoggedIn;

        public TCPNetDriver_Client()
        {
            this.isConnectedToServer = false;
            this.socketTCP = new TcpClient(AddressFamily.InterNetwork);            
            this.listUserOnline = new List<string>();
        }

        /// <summary>
        /// Connect to DeltaSE server if netdriver is not connected;
        /// </summary>
        public void Connect_toDeltaSE_Server()
        {
            if (!isConnectedToServer)
            {
                try
                {
                    socketTCP.Connect(Server.Default.ServerDeltaSE, port);
                    strWritter = new StreamWriter(socketTCP.GetStream());
                }
                // TODO: define 3rd Exception Here for driverException.
                catch (Exception driverException)
                {
                    throw new Exception("Cannot connect to server, Please check file 'Server.settings'");
                }
                isConnectedToServer = true;
            }
            else { }
        }

        public void Send_MessageTo(string destinationUser, string message)
        {
            string package = "SEND_MSG;" + destinationUser + ";" + message;
            strWritter.WriteLine(package);
            strWritter.Flush();
            Thread.Sleep(300);
        }

        /// <summary>
        /// This method use for Development mode
        /// </summary>
        /// <param name="userName"></param>
        public void Send_LoginRequest(String userName)
        {
            string connectionEstablish;
            connectionEstablish = "LOGIN_REQUEST;" + userName;
            this.incomingMessageHandler = new Thread(() => ReceiveMessages());
            this.incomingMessageHandler.IsBackground = true;
            this.incomingMessageHandler.Start();
            strWritter.WriteLine(connectionEstablish);
            strWritter.Flush();
            isConnectedToServer = true;
        }

        /// <summary>
        /// This method use for Production mode
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public bool Send_LoginRequest(String userName, String password)
        {
            bool result = false;
            string connectionEstablish;
            connectionEstablish = "LOGIN_AUTH_REQUEST;" + userName + ";" + password;
            strWritter.WriteLine(connectionEstablish);
            strWritter.Flush();
            strReader = new StreamReader(this.socketTCP.GetStream());
            string serverResponse = strReader.ReadLine();
            if( serverResponse == "LOGIN_SUCCESSFUL")
            {
                result = true;
                this.incomingMessageHandler = new Thread(() => ReceiveMessages());
                this.incomingMessageHandler.IsBackground = true;
                this.incomingMessageHandler.Start();
                isConnectedToServer = true;
            }
            return result;          
        }

        /// <summary>
        /// Only call this method when netDriver is connected;
        /// </summary>
        public bool Register(String userName, String password)
        {
            bool result = false;
            string registerRequest = "REGISTER_REQUEST;" + userName + ";" + password;
            strWritter.WriteLine(registerRequest);
            strWritter.Flush();
            strReader = new StreamReader(this.socketTCP.GetStream());
            string serverResponse = strReader.ReadLine();
            if (serverResponse == "REGISTER_SUCCESSFUL")
            {
                result = true;
            }
            return result;
        }  

        public void Send_GetListRequest(String userName)
        {
            string getListRequest = "GETLIST_REQUEST;" + userName;
            strWritter.WriteLine(getListRequest);
            strWritter.Flush();
            // Wait for response "LIST_USR" from server
            // TODO: Use wait explicit instead of wait implicit
            // ( that is, wait until 'this.listUserOnline' received respone from server
            Thread.Sleep(300);
        }

        private void ReceiveMessages()
        {
            strReader = new StreamReader(this.socketTCP.GetStream());

            // While we are successfully connected, read incoming lines from the server
            while (this.isConnectedToServer)
            {
                string serverResponse = strReader.ReadLine();
                string[] data = serverResponse.Split(';');

                // TODO: Use Switch - Case instead of if
                if (data[0].Equals("INCOMING_MSG"))
                {
                    string source = data[1];
                    string message = data[2];                   
                    string temp = source + " Says:" + " " + message;
                    // To call Form_Client.WhenSomeOneContacts()
                    MessageReceived(source, message, new EventArgs());                    
                }

                if (data[0].Equals("LIST_USR"))
                {
                    this.listUserOnline.Clear();
                    for (int i = 1; i < data.Length; i++)
                    {
                        listUserOnline.Add(data[i]);
                    }
                }

                if (data[0].Equals("NEW_USER_ONLINE"))
                {
                    string username = data[1];
                    NewUserLoggedIn(username, new EventArgs());
                }
            }
        }

        ~TCPNetDriver_Client()
        {
            if (this.socketTCP.Connected)
            {
                socketTCP.Close();
                strWritter.Close();
                strReader.Close();
            }
        }
    }
}
