using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using System;
using System.Linq;
using System.IO;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace XZXXYapp.OnlineDatabase.Authentication
{
    internal sealed class ApplicationName
    {
        #region Fields
        readonly static string NamePath = Directory.GetCurrentDirectory() + @"\Auth\appname.txt";
        public string Name;
        public string MacAddress;
        #endregion
        #region Properties
        [JsonIgnore]
        public string Hash
        {
            get
            {
                return AssemblyHashing.getSHA256(JsonConvert.SerializeObject(this));
            }
        }
        #endregion
        #region Constructor
        public ApplicationName()
        {
            MacAddress = getMacAddress();
            Name = readName();
        }
        #endregion
        #region Methods
        public static string getMacAddress()
        {
            return (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
        }
        public string readName()
        {
            try
            {
                // read and return or
                TextReader appNameReader = new StreamReader(new FileStream(NamePath,FileMode.Open));
                string Name = appNameReader.ReadToEnd();
                appNameReader.Close();
                Console.WriteLine("Found the file, read the keyword!");
                return Name;
            }
            catch
            {
                // generate and write and return
                Random r = new Random(DateTime.Now.Second);
                string Name = getRandomString(r.Next(12, 189));
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Auth");
                TextWriter appNameWriter = new StreamWriter(new FileStream(NamePath,FileMode.CreateNew));
                appNameWriter.Write(Name);
                appNameWriter.Close();
                Console.WriteLine("Didn't find the file, generated the keyword and wrote it!");
                return Name;
            }
        }
        public static string getRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
}
