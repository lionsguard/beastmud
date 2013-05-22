using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Serialization
{
    public static class JsonSerialization
    {
        private static readonly HashSet<Type> _types = new HashSet<Type>();
        private static readonly HashSet<Assembly> _assemblies = new HashSet<Assembly>();

        static JsonSerialization()
        {
            AddAssemblies(typeof(Application).Assembly);
        }

        public static void AddTypes(params Type[] types)
        {
            if (types != null && types.Length > 0)
            {
                foreach (var type in types)
                {
                    _types.Add(type);
                }
            }
        }

        public static void AddAssemblies(params Assembly[] assemblies)
        {
            if (assemblies != null && assemblies.Length > 0)
            {
                foreach (var asm in assemblies)
                {
                    _assemblies.Add(asm);
                }
            }
        }

        public static IEnumerable<Type> GetTypes()
        {
            return _types;
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            return _assemblies;
        }

        public static JsonSerializerSettings GetDefaultSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new FilteredCamelCasePropertyNamesContractResolver()
            };
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, GetDefaultSettings());
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, GetDefaultSettings());
        }
    }
}
