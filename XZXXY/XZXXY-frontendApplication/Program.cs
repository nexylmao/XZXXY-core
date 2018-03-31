using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XZXXY1911;
using XZXXY1911.Databasing.NodeJS.Authentication;

namespace XZXXY_frontendApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Authenticator auth = new Authenticator();
            Console.WriteLine(auth.Username);
            Console.WriteLine(AssemblyCalculator.Calculate());
            auth.Authenticate();

            Console.ReadKey(true);
        }
    }
}
