using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XZXXY1911.Databasing.NodeJS
{
    public static class NJSDBHandler // : IDatabaseHandler
    {
        public readonly static string FilePath = "./DB/NJS/";
        public readonly static string AuthFiles = FilePath;

        static string BasePath = "https://xzxxy-backend.herokuapp.com/";
        static string DatabasePath = BasePath + "XZXXY-DATABASE/";
        static string AuthenticationPath = BasePath + "XZXXY-AUTH/";

        public static void setPath(string Path)
        {
            BasePath = Path;
        }
        public static string getPath()
        {
            return BasePath;
        }
        public static string getDatabasePath()
        {
            return DatabasePath;
        }
        public static string getAuthenticationPath()
        {
            return AuthenticationPath;
        }
    }
}
