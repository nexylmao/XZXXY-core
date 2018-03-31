using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XZXXY1911.Databasing
{
    interface IDatabaseHandler
    {
        string getPath();
        void setPath(string Path);
        string getDatabasePath();
        string getAuthenticationPath();
    }
}
