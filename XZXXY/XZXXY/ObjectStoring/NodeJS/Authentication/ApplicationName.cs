using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

namespace XZXXY.ObjectStoring.NodeJS.Authentication
{
    internal static class ApplicationName
    {
        #region Fields
        readonly static string directory = Directory.GetCurrentDirectory() + @"\Auth";
        readonly static string file = @"\_appname";
        #endregion
        #region Methods
        static string makeUUID()
        {
            return Guid.NewGuid().ToString();
        }
        static string getUUID()
        {
            try
            {
                using (BinaryReader br = new BinaryReader(new FileStream(directory + file, FileMode.Open)))
                {
                    return br.ReadString();
                }
            }
            catch
            {
                try
                {
                    using (BinaryWriter bw = new BinaryWriter(new FileStream(directory + file, FileMode.CreateNew)))
                    {
                        string newword = makeUUID();
                        bw.Write(newword);
                        return newword;
                    }
                }
                catch
                {
                    Directory.CreateDirectory(directory);
                    using (BinaryWriter bw = new BinaryWriter(new FileStream(directory + file, FileMode.CreateNew)))
                    {
                        string newword = makeUUID();
                        bw.Write(newword);
                        return newword;
                    }
                }
            }
        }
        static string getMAC()
        {
            return
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
        }
        #endregion
        #region Internal
        internal class AppName : JsonSerializable
        {
            public string UUID;
            public string MAC;
        }
        #endregion
        #region Public
        public static JsonSerializable getAppName()
        {
            return new AppName()
            {
                UUID = getUUID(),
                MAC = getMAC()
            };
        }
        #endregion
    }
}
