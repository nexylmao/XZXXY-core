using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using Newtonsoft.Json;

namespace XZXXY.ObjectStoring.NodeJS.Authentication
{
    public abstract class JsonSerializable
    {
        public virtual string toJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    internal class Token
    {
        #region Fields
        [JsonProperty]
        protected bool ADMIN;
        [JsonProperty]
        protected string ASSHASH;
        [JsonProperty]
        protected string APPHASH;
        [JsonProperty]
        protected string KEYWORD;
        protected static Token mainToken;
        #endregion
        #region Properties
        [JsonIgnore]
        public Token Main
        {
            get
            {
                return mainToken;
            }
        }
        [JsonIgnore]
        public Func<JsonSerializable> ApplicationName
        {
            get
            {
                return Authentication.ApplicationName.getAppName;
            }
        }
        [JsonIgnore]
        public Func<JsonSerializable> Assembly
        {
            get
            {
                return GetAssembly.getAssembly;
            }
        }
        #endregion
        #region Constructors
        public Token()
        {
            mainToken = this;
            ADMIN = false;
            ASSHASH = getSHA256(Assembly().toJSON());
            APPHASH = getSHA256(ApplicationName().toJSON());
        }
        public Token(bool adminMode)
        {
            mainToken = this;
            ADMIN = adminMode;
            ASSHASH = getSHA256(Assembly().toJSON());
            APPHASH = getSHA256(ApplicationName().toJSON());
        }
        #endregion
        #region Methods
        public static string getSHA256(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        #endregion
    }

    sealed class CustomToken : Token
    {
        #region Constructors
        public CustomToken(bool adminMode, string ApplicationNameHash, string AssemblyHash)
        {
            mainToken = this;
            ADMIN = adminMode;
            APPHASH = ApplicationNameHash;
            ASSHASH = AssemblyHash;
        }
        public CustomToken(bool adminMode, string ApplicationNameHash, string AssemblyHash, string Keyword)
        {
            mainToken = this;
            ADMIN = adminMode;
            APPHASH = ApplicationNameHash;
            ASSHASH = AssemblyHash;
            KEYWORD = Keyword;
        }
        #endregion
    }
}
