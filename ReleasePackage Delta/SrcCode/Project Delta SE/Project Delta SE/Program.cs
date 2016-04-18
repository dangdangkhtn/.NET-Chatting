using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project_Delta_SE
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestFunc01();
            Server server = new Server();
            server.startListener();
            Console.WriteLine("--> DeltaSE Started :");         
            server.OnclientConnected += new Server.PropertyChangeHandler(server_OnclientConnected);

            while (true)
            {
            }
        }

        static void server_OnclientConnected(object sender, EventArgs args)
        {
            User usr = (User)sender;
            Console.WriteLine(usr.UserName + " Joined to Global Room");
        }

        static void TestFunc01()
        {
            AuthenticationDSE authModule = new AuthenticationDSE();
            authModule.Register("usrTestFunc02", "changeit");
        }
        static void TestFunc02()
        {
            AuthenticationDSE authModule = new AuthenticationDSE();
            authModule.Authenticate("usrTestFunc02", "changeit");
        }
    }
}
