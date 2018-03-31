using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace XZXXY.ObjectStoring.NodeJS.Authentication
{
    internal static class AssemblyChanges
    {
        #region Fields
        readonly static string directory = Directory.GetCurrentDirectory() + @"\Auth";
        readonly static string path = directory + @"\_assembly";
        #endregion
        #region Methods
        public static bool HasUpdated()
        {
            try
            {
                using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
                {
                    if(br.ReadString() == File)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message); // replace when message system shows up
            }
            // write all the catches, and one unspecified one
            catch
            {

            }
            return false;
        }
        #endregion

        #region Integrated Assembly
        readonly static string File = @"using System.Reflection;
        using System.Runtime.CompilerServices;
        using System.Runtime.InteropServices;

        // General Information about an assembly is controlled through the following
        // set of attributes. Change these attribute values to modify the information
        // associated with an assembly.
        [assembly: AssemblyTitle(""XZXXY"")]
        [assembly: AssemblyDescription("""")]
        [assembly: AssemblyConfiguration("""")]
        [assembly: AssemblyCompany("""")]
        [assembly: AssemblyProduct(""XZXXY"")]
        [assembly: AssemblyCopyright(""Copyright ©  2018"")]
        [assembly: AssemblyTrademark("""")]
        [assembly: AssemblyCulture("""")]

        // Setting ComVisible to false makes the types in this assembly not visible
        // to COM components.  If you need to access a type in this assembly from
        // COM, set the ComVisible attribute to true on that type.
        [assembly: ComVisible(false)]

        // The following GUID is for the ID of the typelib if this project is exposed to COM
        [assembly: Guid(""ae7cc118-b5a3-41e5-918a-212da5070c01"")]

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
        [assembly: AssemblyVersion(""0.0.1"")]
        [assembly: AssemblyFileVersion(""0.0.1"")";
        #endregion
    }
}
