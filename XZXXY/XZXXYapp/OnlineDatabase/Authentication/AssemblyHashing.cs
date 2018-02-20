using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;

namespace XZXXYapp.OnlineDatabase.Authentication
{
    internal static class AssemblyHashing
    {
        #region Properties
        public static string AssemblyHash
        {
            get
            {

                return getSHA256(SerializedAssembly);
            }
        }
        public static string SerializedAssembly
        {
            get
            {
                return JsonConvert.SerializeObject(typeof(AssemblyHashing).Assembly);
            }
        }
        #endregion
        #region Methods
        public static string getSHA256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
        #endregion
    }

    internal static class AssemblyHashingPlus
    {
        #region Fields
        readonly static string RealPath = Directory.GetCurrentDirectory() + @"\Auth\AssemblyInfo.cs";
        readonly static string BackupPath = Directory.GetCurrentDirectory() + @"\Auth\!AssemblyInfo.cs";
        static string newHash;
        #endregion
        #region Properties
        public static bool HasChanged
        {
            get
            {
                return Check();
            }
        }
        public static string NewHash
        {
            get
            {
                return newHash;
            }
        }
        #endregion
        #region Methods
        public static bool Check()
        {
            try
            {
                TextReader realFile = new StreamReader(RealPath);
                TextReader backupFile = null;
                TextWriter writeBackupFile = null;
                try
                {
                    backupFile = new StreamReader( BackupPath);
                    if (backupFile.ReadToEnd() == realFile.ReadToEnd())
                    {
                        realFile.Close();
                        backupFile.Close();
                        // replace with message Console.WriteLine("File hasn't changed!");
                        Console.WriteLine("File hasn't changed!");
                        return false;
                    }
                    else
                    {
                        backupFile.Close();
                        realFile.Close();
                        realFile = new StreamReader( RealPath);
                        writeBackupFile = new StreamWriter(new FileStream( BackupPath, FileMode.CreateNew));
                        writeBackupFile.Write(realFile.ReadToEnd());
                        writeBackupFile.Close();
                        realFile.Close();
                        realFile = new StreamReader( RealPath);
                        // replace with message Console.WriteLine("File has changed! -- HASH THE ASSEMBLY AND SEND");
                        Console.WriteLine("File has changed! -- HASH THE ASSEMBLY AND SEND");
                        newHash = AssemblyHashing.getSHA256(AssemblyHashing.SerializedAssembly);
                        realFile.Close();
                        return true;
                    }
                }
                catch
                {
                    writeBackupFile = new StreamWriter( BackupPath);
                    writeBackupFile.Write(realFile.ReadToEnd());
                    writeBackupFile.Close();
                    realFile.Close();
                    realFile = new StreamReader( RealPath);
                    // replace with message Console.WriteLine("File has changed! -- HASH THE ASSEMBLY AND SEND");
                    Console.WriteLine("File has changed! -- HASH THE ASSEMBLY AND SEND");
                    newHash = AssemblyHashing.getSHA256(AssemblyHashing.SerializedAssembly);
                    realFile.Close();
                    return true;
                }
            }
            catch (FileNotFoundException x)
            {
                // making the file from last hard-coded backup
                TextWriter writeRealFile = new StreamWriter(new FileStream(RealPath, FileMode.CreateNew));
                TextWriter writeBackupFile = new StreamWriter(new FileStream(BackupPath, FileMode.CreateNew));
                string AssemblyFile = @"
                using System.Reflection;
                using System.Runtime.CompilerServices;
                using System.Runtime.InteropServices;

                // General Information about an assembly is controlled through the following
                // set of attributes. Change these attribute values to modify the information
                // associated with an assembly.
                [assembly: AssemblyTitle(""XZXXYapp"")]
                [assembly: AssemblyDescription(""Application base library, to be used in future for creating applications/games"")]
                [assembly: AssemblyConfiguration("""")]
                [assembly: AssemblyCompany("""")]
                [assembly: AssemblyProduct(""XZXXYapp"")]
                [assembly: AssemblyCopyright(""Copyright © Nenad Vuletic 2018"")]
                [assembly: AssemblyTrademark("""")]
                [assembly: AssemblyCulture("""")]

                // Setting ComVisible to false makes the types in this assembly not visible
                // to COM components.  If you need to access a type in this assembly from
                // COM, set the ComVisible attribute to true on that type.
                [assembly: ComVisible(false)]

                // The following GUID is for the ID of the typelib if this project is exposed to COM
                [assembly: Guid(""67de31c0-b29d-48b2-b2cb-32ee94d6514d"")]

                // Version information for an assembly consists of the following four values:
                //
                //      Major Version
                //      Minor Version
                //      Build Number
                //      Revision
                //
                // You can specify all the values or you can default the Build and Revision Numbers
                // by using the '*' as shown below:
                // [assembly: AssemblyVersion(""1.0.*"")]
                [assembly: AssemblyVersion(""0.1.1"")]
                [assembly: AssemblyFileVersion(""0.1.1"")]
                ";
                writeRealFile.Write(AssemblyFile);
                writeBackupFile.Write(AssemblyFile);
                writeRealFile.Close();
                writeBackupFile.Close();
                // replace with message Console.WriteLine("The file we were looking for doesn't exist!");
                Console.WriteLine("The file we were looking for doesn't exist! But both were created");
            }
            catch (UnauthorizedAccessException x)
            {
                // replace with message 
                Console.WriteLine("Couldn't access the directory due to permission issues!");
            }
            catch (NotSupportedException x)
            {
                // replace with message Console.WriteLine("Oh, you find yourself doing this on a platform that doesn't support this!");
                Console.WriteLine("Oh, you find yourself doing this on a platform that doesn't support this!");
            }
            catch (DirectoryNotFoundException x)
            {
                // replace with message Console.WriteLine("The directory the stream is looking for doesn't exist!");
                Console.WriteLine("The directory the stream is looking for doesn't exist!");
            }
            catch
            {
                // replace with message Console.WriteLine("Some problem occured, couldn't specify what!");
                Console.WriteLine("Some problem occured, couldn't specify what!");
            }
            return false;
        } 
        #endregion
    }
}
