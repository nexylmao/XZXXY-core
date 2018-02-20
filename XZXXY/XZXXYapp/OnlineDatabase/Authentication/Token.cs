using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;

namespace XZXXYapp.OnlineDatabase.Authentication
{
    internal class Token
    {
        #region Fields
        readonly static string KeywordPath = Directory.GetCurrentDirectory() + @"\Auth\Keyword.txt";
        public string KEYWORD;
        public string ASSHASH;
        public string APPHASH;
        static Token main;
        #endregion
        #region Properties
        public static Token mainToken
        {
            get
            {
                return main;
            }
            set
            {
                main = value;
            }
        }
        #endregion
        #region Constructors
        public Token()
        {
            main = this;
            APPHASH = new ApplicationName().Hash;
            ASSHASH = AssemblyHashing.AssemblyHash;
            KEYWORD = LoadKeyword();
            if (AssemblyHashingPlus.Check())
            {
                AttemptHashUpload();
            }
            if (AuthenticatorClient.PostFirstStep(this))
            {
                AuthenticatorClient.PostSecondStep(this);
            }
        }
        public Token(string Keyword)
        {
            main = this;
            APPHASH = new ApplicationName().Hash;
            ASSHASH = AssemblyHashing.AssemblyHash;
            KEYWORD = Keyword;
            if (AssemblyHashingPlus.Check())
            {
                AttemptHashUpload();
            }
            if (AuthenticatorClient.PostFirstStep(this))
            {
                AuthenticatorClient.PostSecondStep(this);
            }
        }
        #endregion
        #region Methods
        string LoadKeyword()
        {
            try
            {
                TextReader tr = new StreamReader(new FileStream(KeywordPath, FileMode.OpenOrCreate));
                string Keyword = tr.ReadToEnd();
                tr.Close();
                return Keyword;
            }
            catch
            {
                // should be nothing - comes here in default cases, Token has no keyword, gets from first step
                return "";
            }
        }
        public string toJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
        void AttemptHashUpload()
        {
            AuthenticatorClient.PostHash(this);
        }
        #endregion
    }
}
