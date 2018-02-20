using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace XZXXYapp.OnlineDatabase.Authentication
{
    internal static class AuthenticatorClient
    {
        #region Fields
        readonly static HttpClient client = new HttpClient();
        static string path;
        static bool canUse = false;
        static string userName = "";
        #endregion
        #region Properties
        public static string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        static void setPath(string value)
        {
            if (value.Contains("http") && !value.Contains("https"))
            {
                // send msg that server is hosted on an unsecure non-SSL address
            }
            else if (value.Contains("localhost"))
            {
                // send msg that server is hosted locally
            }
            path = value;
            string response = client.GetAsync(path + "/XZXXY-AUTH/WriteKeyword").Result.Content.ReadAsStringAsync().Result;
            if (!(response.Contains("Wrote the keyword to server console!")))
            {
                // send msg that path is bad
                path = "bad path!";
            }
        }
        public static bool CanUse
        {
            get
            {
                return canUse;
            }
        }
        public static string Username
        {
            get
            {
                return userName;
            }
        }
        #endregion
        #region AsyncTasks(Turn'd sync)
        public static bool PostHash(Token obj)
        {
            Console.WriteLine(obj.toJSON());
            string response = client.PostAsync(path + "XZXXY-AUTH/Hash", new StringContent(obj.toJSON(), Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
            if (response.Contains("Successfully added the hash to database!"))
            {
                // write msg that we added the hash
                Console.WriteLine("Successfully added the hash!");
                return true;
            }
            else
            {
                // write msg that we didn't add the hash
                Console.WriteLine("Couldn't add the hash!");
                return false;
            }
        }
        public static bool PostFirstStep(Token obj)
        {
            string response = client.PostAsync(path + "XZXXY-AUTH/FirstStep", new StringContent(obj.toJSON(), Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
            if (response.Contains("Successfully"))
            {
                obj.KEYWORD = response;
                // write msg that we did the first step
                Console.WriteLine("Successfully done the first step for registration!");
                return true;
            }
            else
            {
                // write msg that we didnt do the first step!
                Console.WriteLine("The first step wasn't done!");
                return false;
            }
        }
        public static bool PostSecondStep(Token obj)
        {
            string response = client.PostAsync(path + "XZXXY-AUTH/SecondStep", new StringContent(obj.toJSON(), Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
            if (response.Contains("Successfully"))
            {
                // write msg that we did the first step
                Console.WriteLine("Successfully done the second step for registration! App ready to use online database");
                canUse = true;
                userName = obj.APPHASH;
                return true;
            }
            else
            {
                // write msg that we didnt do the second step!
                Console.WriteLine("The second step wasn't done!");
                return false;
            }
        }
        #endregion
    }
}
