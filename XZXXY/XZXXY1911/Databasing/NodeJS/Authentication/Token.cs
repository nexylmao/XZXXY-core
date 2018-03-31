using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace XZXXY1911.Databasing.NodeJS.Authentication
{
    // do this later, write some logs and stuff like that
    internal static class OutputMethods
    {

    }


    internal class Token
    {
        #region Fields
        string APPHASH, ASSHASH, KEYWORD;
        bool ADMIN;
        #endregion
        #region Constructors
        public Token(string ApplicationHash, string AssemblyHash)
        {
            APPHASH = ApplicationHash;
            ASSHASH = AssemblyHash;
            ADMIN = false;
            KEYWORD = "";
        }
        public Token(string ApplicationHash, string AssemblyHash, bool IsAdmin)
        {
            APPHASH = ApplicationHash;
            ASSHASH = AssemblyHash;
            ADMIN = IsAdmin;
            KEYWORD = "";
        }
        #endregion
        #region Properties
        [JsonIgnore]
        public string ApplicationHash
        {
            get
            { return APPHASH; }
        }
        [JsonIgnore]
        public string AssemblyHash
        {
            get
            { return ASSHASH; }
        }
        [JsonIgnore]
        public bool IsAdmin
        {
            get
            { return ADMIN; }
        }
        [JsonIgnore]
        public string Keyword
        {
            set
            { KEYWORD = value; }
        }
        #endregion
        #region Methods
        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }

    public static class SHA256Calculator
    {
        public static string Compute(string value)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(value));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }

    public static class AssemblyCalculator
    {
        public static string Calculate()
        {
            object obj = Assembly.GetAssembly(typeof(Token));
            return SHA256Calculator.Compute(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }

    public static class ApplicationNameCalculator
    {
        internal struct ApplicationName
        {
            public string UUID, MAC;
        }

        internal static string GetMAC()
        {
            return
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
        }

        internal static string UUIDPath = NJSDBHandler.AuthFiles + "uuid.txt";

        internal static string GetUUID()
        {
            string UUID = "";
            for(int i = 0; i < 6; i++)
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(new FileStream(UUIDPath, FileMode.Open)))
                    {
                        UUID = reader.ReadString();
                    }
                    return UUID;
                }
                catch(DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(NJSDBHandler.AuthFiles);
                }
                catch(FileNotFoundException)
                {
                    UUID = Guid.NewGuid().ToString();
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(UUIDPath, FileMode.CreateNew)))
                    {
                        writer.Write(UUID);
                    }
                    return UUID;
                }
                catch(Exception)
                {
                    Console.WriteLine("Something unexpected happend, sorry!");
                }
            }
            return UUID;
        }

        internal static ApplicationName CreateName()
        {
            return new ApplicationName()
            {
                UUID = GetUUID(),
                MAC = GetMAC()
            };
        }

        internal static string ToJSON(ApplicationName x)
        {
            return JsonConvert.SerializeObject(x);
        }

        internal static string ToJSON()
        {
            return JsonConvert.SerializeObject(CreateName());
        }

        public static string Calculate()
        {
            return SHA256Calculator.Compute(ToJSON(CreateName()));
        }
    }

    public class Authenticator
    {
        #region Fields
        Token mainToken;
        bool isAuthenticated;
        #endregion
        #region Properties
        public bool Status
        {
            get
            { return isAuthenticated; }
        }
        public string Username
        {
            get
            { return mainToken.ApplicationHash; }
        }
        #endregion
        #region Constructors
        public Authenticator()
        {
            CreateToken();
            isAuthenticated = false;
        }
        #endregion
        #region Methods
        public void CreateToken()
        {
            mainToken = new Token(ApplicationNameCalculator.Calculate(), AssemblyCalculator.Calculate());
        }
        public void CreateToken(bool Admin)
        {
            mainToken = new Token(ApplicationNameCalculator.Calculate(), AssemblyCalculator.Calculate(), Admin);
        }
        public void CreateCustomToken(string ApplicationName, string AssemblyHash, bool AdminMode, string Keyword)
        {
            mainToken = new Token(ApplicationName, AssemblyHash, AdminMode);
            mainToken.Keyword = Keyword;
        }
        public void Authenticate()
        {
            if(mainToken != null)
            {
                for(int i = 0; i < 6; i++)
                {
                    try
                    {
                        using (HttpClient x = new HttpClient())
                        {
                            HttpResponseMessage msg = x.PostAsync(NJSDBHandler.getAuthenticationPath() + "FirstStep", new StringContent(mainToken.ToJSON())).Result;
                            string response = msg.Content.ReadAsStringAsync().Result;
                            if (response.Contains("already exist"))
                            {
                                isAuthenticated = true;
                                Console.WriteLine(response);
                                return;
                            }
                            if (response.Contains("error") || response.Contains("Error") || response.Contains("denied"))
                            {
                                isAuthenticated = false;
                                Console.WriteLine(response);
                                return;
                            }
                            else
                            {
                                mainToken.Keyword = response;
                                msg = x.PostAsync(NJSDBHandler.getAuthenticationPath() + "SecondStep", new StringContent(mainToken.ToJSON())).Result;
                                string scndresponse = msg.Content.ReadAsStringAsync().Result;
                                if(response.Contains("already exist"))
                                {
                                    isAuthenticated = true;
                                    Console.WriteLine(scndresponse);
                                    return;
                                }
                                if (response.Contains("error") || response.Contains("Error") || response.Contains("denied"))
                                {
                                    isAuthenticated = false;
                                    Console.WriteLine(scndresponse);
                                    return;
                                }
                                if (response.Contains("is not correct"))
                                {
                                    Console.WriteLine(scndresponse);
                                    throw new Exception();
                                }
                                if (response.Contains("successfully registered"))
                                {
                                    Console.WriteLine(scndresponse);
                                    isAuthenticated = true;
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("The keyword has probably already changed, let's try again!");
                    }
                }
            }
        }
        #endregion
    }
}
