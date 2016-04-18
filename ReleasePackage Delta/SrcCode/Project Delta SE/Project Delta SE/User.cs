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
using System.Net.Sockets;


namespace Project_Delta_SE
{
    public class User
    {
        private TcpClient tcpClient;
        private string userName;


        public User(TcpClient tcpClient, string userName)
        {
            this.tcpClient = tcpClient;
            this.userName = userName;
        }

        public TcpClient TcpClient
        {
            get { return tcpClient; }
            set { tcpClient = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}
