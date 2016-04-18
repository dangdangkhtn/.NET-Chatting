/*
Author: kalana thejitha
Ref: https://www.youtube.com/watch?v=XVVdaqRKHJA
Modified by: THDANG
Mod Date: Mar 27, 2016
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Project_Delta_SE
{
    public class Server
    {
       
        private int port = 49512;
        private bool isServerRunning = false;

        private TcpListener serverListener;
        private Thread backgroundListner;
        private Thread connectionThread;
        private AuthenticationDSE authModule = new AuthenticationDSE();

        List<User> tcpClients = new List<User>();

        public delegate void PropertyChangeHandler(object sender, EventArgs args);

        public event PropertyChangeHandler OnclientConnected;

        public List<User> TcpClients
        {
            get { return tcpClients; }
            set { tcpClients = value; }
        }

        public void startListener()
        {
            this.serverListener = new TcpListener(new IPEndPoint(IPAddress.Any, this.port));
            this.serverListener.Start();
            this.isServerRunning = true;

            backgroundListner = new Thread(() => KeepListening());
            backgroundListner.IsBackground = true;
            backgroundListner.Start();
        }

        private void KeepListening()
        {
            // While the server is running
            while (isServerRunning == true)
            {
                TcpClient tcpClient = this.serverListener.AcceptTcpClient(); // Otherwise this will block the UI // Called when a Client Connect
                connectionThread = new Thread(() => connectionHandler(tcpClient));
                connectionThread.IsBackground = true;
                connectionThread.Start();
            }
        }

        private void connectionHandler(TcpClient client)
        {
            TcpClient this_client = client;
            Stream clientStream = this_client.GetStream();

            StreamWriter strWritter = new StreamWriter(clientStream);
            StreamReader strReader = new StreamReader(clientStream);
            string clientMessage;

            // Keep waiting for a message from the user
            while (this.isServerRunning == true)
            {
                clientMessage = strReader.ReadLine();

                string[] response = clientMessage.Split(';');

                // TODO: Use "swicht-case" instead of "if" here
                if (response[0] == "LOGIN_REQUEST")
                {
                    User usr = new User(client, response[1]);
                    this.tcpClients.Add(usr);
                    OnclientConnected(usr, new EventArgs());                                 
                }

                if (response[0] == "LOGIN_AUTH_REQUEST")
                {
                    string username = response[1];
                    string password = response[2];

                    User usr = new User(client, username);
                    Stream desStream = usr.TcpClient.GetStream();
                    StreamWriter wr = new StreamWriter(desStream);
                    bool auth = this.authModule.Authenticate(username, password);
                    if (auth == true)
                    {                        
                        
                        OnclientConnected(usr, new EventArgs());                  
                        wr.WriteLine("LOGIN_SUCCESSFUL");
                        wr.Flush();
                        Thread.Sleep(300); // Let Client load something before send next message
                        // 1. Broadcast to all user in room there is new user joined
                        foreach (User temp in TcpClients)
                        {
                            Stream outStream = temp.TcpClient.GetStream();
                            StreamWriter oWr = new StreamWriter(outStream);
                            oWr.WriteLine("NEW_USER_ONLINE;" + username);
                            oWr.Flush();
                        }
                        // 2.Then add this user to list of Clients is online.
                        this.tcpClients.Add(usr);
                    }
                    else
                    {            
                        wr.WriteLine("INVALID_CREDENTIAL");
                        wr.Flush();
                    }
                }

                // Message template: "SEND_MSG;<target user>;<message body>"
                if (response[0] == "SEND_MSG")
                {
                    string to = response[1];
                    string msgBody = response[2];
                    
                    User source = this.TcpClients.Where(c => c.TcpClient.Equals(this_client)).Select(c => c).FirstOrDefault();
                    User destination = this.TcpClients.Where(c => c.UserName == to).Select(c => c).FirstOrDefault();
            
                    string client_chat = "INCOMING_MSG;" + source.UserName + ";" + msgBody;

                    Stream desStream = destination.TcpClient.GetStream();

                    StreamWriter wr = new StreamWriter(desStream);
                    wr.WriteLine(client_chat);
                    wr.Flush();
                    Console.WriteLine(source.UserName + " -> " + destination.UserName + ":" + msgBody);
                    Console.WriteLine("Full Mess: " + client_chat);
                }

                // Message template: "REGISTER_REQUEST;<username>;<password>"
                if (response[0] == "REGISTER_REQUEST")
                {
                    string username = response[1];
                    string password = response[2];
                    Console.WriteLine("Received: REGISTER_REQUEST;" + username +";" + password);                    
                    AuthenticationDSE authmodule = new AuthenticationDSE();
                    string regResult = authmodule.Register(username, password);

                    User usr = new User(client, username);
                    Stream desStream = usr.TcpClient.GetStream();
                    StreamWriter wr = new StreamWriter(desStream);
                    if (regResult == "Successful")
                    {
                        wr.WriteLine("REGISTER_SUCCESSFUL");
                        wr.Flush();
                        Console.WriteLine("User :" + username + "is registered");
                    }                    
                }

                // Message template: "GETLIST_REQUEST;<username>"
                if (response[0] == "GETLIST_REQUEST")
                {
                    // username of the user who send this request
                    string username = response[1];
                    Console.WriteLine("Received: GETLIST_REQUEST by '" + username + "'");
                    User destination = this.TcpClients.Where(c => c.UserName == username).Select(c => c).FirstOrDefault();
                    Stream desStream = destination.TcpClient.GetStream();
                    StreamWriter wr = new StreamWriter(desStream);

                    // produce list user as message like this : ";<username1>;<username2>;<username3>"
                    string temp = "";
                    foreach (User usr in TcpClients)
                    {
                        temp += ";";
                        temp += usr.UserName;                        
                    }

                    string listUser = "LIST_USR" + temp;
                    Console.WriteLine("Return: " + listUser);

                    try
                    {
                        wr.WriteLine(listUser);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Action Failed: 'Send List User'");
                        Console.WriteLine(e.ToString());
                    }
                    wr.Flush();
                }
            }
        }
    }
}
