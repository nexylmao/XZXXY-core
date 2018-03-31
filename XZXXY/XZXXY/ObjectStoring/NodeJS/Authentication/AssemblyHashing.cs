using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using Newtonsoft.Json;

namespace XZXXY.ObjectStoring.NodeJS.Authentication
{
    internal static class GetAssembly
    {
        #region Internal
        internal class SerializableAssembly : JsonSerializable
        {
            #region Fields
            [JsonProperty]
            Assembly assembly;
            #endregion
            #region Constructor
            public SerializableAssembly()
            {
                assembly = Assembly.GetAssembly(typeof(SerializableAssembly));
            }
            #endregion
            #region Methods
            public override string toJSON()
            {
                return JsonConvert.SerializeObject(assembly);
            }
            #endregion
        }
        #endregion
        #region Public
        public static JsonSerializable getAssembly()
        {
            return new SerializableAssembly();
        }
        #endregion
    }
}
