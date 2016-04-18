using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

namespace Project_Delta_SE
{
    class AuthenticationDSE
    {
        // ref: http://www.csharp-examples.net/get-application-directory/
        private string programPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).CodeBase);
        private string jsonPath = "Credentials.json";
        
        //Class for JSON: "Credential", "RootObject"---------------------------------------------------------
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
        //Class for JSON: "Credential", "RootObject"---------------------------------------------------------

        /// <summary>
        /// Register an account if the 'username' is not registered.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>'Successful' if register successful</returns>
        public string Register(string username, string password)
        {
            JSONRootObject rootObj = JsonConvert.DeserializeObject<JSONRootObject>(
                File.ReadAllText(this.jsonPath));
            string result = "Successful";
            if (rootObj != null)
            {
                Credential newUser = new Credential();
                newUser.username = username;
                newUser.password = password;
                // TODO: if username is registered, don't allow to register again, should check username here
                if (rootObj.Credentials.Contains(newUser))
                {
                    result = "This username is registered: '" + username + "'";                
                }
                else
                {
                    rootObj.Credentials.Add(newUser);
                    File.WriteAllText(this.jsonPath, JsonConvert.SerializeObject(rootObj));
                }                
            }
            else
            {
                result = "Read file 'Credentials.json' uncuccessful";
            }
            return result;
        }

        /// <summary>
        /// Return false if input wrong credential
        /// </summary>
        public bool Authenticate(string username, string password)
        {
            bool result = false;          
            JSONRootObject rootObj = JsonConvert.DeserializeObject<JSONRootObject>(
                File.ReadAllText(jsonPath));
            if (rootObj != null)
            {
                foreach (Credential cre in rootObj.Credentials)
                {
                    if (cre.username == username && cre.password == password)
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Read file 'Credentials.json' uncuccessful");
            }
            return result;
        }        
    }
}
