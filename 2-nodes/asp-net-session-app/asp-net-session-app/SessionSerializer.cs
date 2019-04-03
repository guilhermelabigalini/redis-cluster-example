using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Reflection;

namespace asp_net_session_app
{
    public class SessionSerializer : Microsoft.Web.Redis.ISerializer
    {
        private static readonly JsonSerializerSettings Settings;

        static SessionSerializer()
        {
            Settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ContractResolver = SuppressItemTypeNameContractResolver.Instance,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
        }

        public byte[] Serialize(object data)
        {
            string s = JsonConvert.SerializeObject(data, Settings);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(s);

#if DEBUG
            //try
            //{
            //    byte[] binaryBytes;
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        IFormatter formatter = new BinaryFormatter();
            //        formatter.Serialize(stream, data);

            //        stream.Seek(0, SeekOrigin.Begin);
            //        binaryBytes = stream.ToArray();
            //    }


            //    Debug.WriteLine("========================================");
            //    Debug.WriteLine($"Serialized session: {s.Length} json len / {jsonBytes.Length} json bytes / {binaryBytes.Length} binary bytes");
            //    Debug.WriteLine(s);
            //    Debug.WriteLine("========================================");
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e.Message);
            //}
#endif
            return jsonBytes;
        }

        public object Deserialize(byte[] data)
        {
            return data == null ? null : JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Settings);
        }
    }

    public class SuppressItemTypeNameContractResolver : DefaultContractResolver
    {
        // As of 7.0.1, Json.NET suggests using a static instance for "stateless" contract resolvers, for performance reasons.
        // http://www.newtonsoft.com/json/help/html/ContractResolver.htm
        // http://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_Serialization_DefaultContractResolver__ctor_1.htm
        // "Use the parameterless constructor and cache instances of the contract resolver within your application for optimal performance."
        static SuppressItemTypeNameContractResolver instance;

        // Using a static constructor enables fairly lazy initialization.  http://csharpindepth.com/Articles/General/Singleton.aspx
        static SuppressItemTypeNameContractResolver() { instance = new SuppressItemTypeNameContractResolver(); }

        public static SuppressItemTypeNameContractResolver Instance { get { return instance; } }

        private Dictionary<string, string> PropertyAlias = new Dictionary<string, string>()
        {
            { "InstanceID", "inid" },
            { "ArtifactID",  "atid" },
            { "AccountID", "acid" }
        };

        protected SuppressItemTypeNameContractResolver() : base() { }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            string alias;
            if (PropertyAlias.TryGetValue(property.PropertyName, out alias))
            {
                property.PropertyName = alias;
            }
            return property;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);
            var containerContract = contract as JsonContainerContract;
            if (containerContract != null && containerContract.ItemTypeNameHandling == null)
            {
                containerContract.ItemTypeNameHandling = TypeNameHandling.None;
            }

            return contract;
        }
    }
}