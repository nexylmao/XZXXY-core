using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XZXXYapp.OnlineDatabase.Authentication;

namespace XZXXYapp
{
    // reprogram this later, now only for testing of this
    public class Start
    {
        Token mainToken;
        public Start()
        {
            mainToken = new Token();
        }

        public Start(string path)
        {
            AuthenticatorClient.Path = path;
            mainToken = new Token();
        }

        public Start(string path, string Keyword)
        {
            AuthenticatorClient.Path = path;
            mainToken = new Token(Keyword);
        }
    }
}
