using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Beast.Serialization
{
    // http://blogs.msdn.com/b/stuartleeks/archive/2012/09/10/automatic-camel-casing-of-properties-with-signalr-hubs.aspx

    public class FilteredCamelCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public HashSet<Assembly> AssembliesToInclude { get; set; } // Identifies assemblies to include from camel-casing 
        public HashSet<Type> TypesToInclude { get; set; } // Identifies types to include from camel-casing 

        public FilteredCamelCasePropertyNamesContractResolver()
        {
            AssembliesToInclude = new HashSet<Assembly>();
            TypesToInclude = new HashSet<Type>();

            foreach (var type in JsonSerialization.GetTypes())
            {
                TypesToInclude.Add(type);
            }
            foreach (var asm in JsonSerialization.GetAssemblies())
            {
                AssembliesToInclude.Add(asm);
            }
        }
        public FilteredCamelCasePropertyNamesContractResolver(params Type[] types)
            : this(types, null)
        {
        }
        public FilteredCamelCasePropertyNamesContractResolver(params Assembly[] assemblies)
            : this(null, assemblies)
        {
        }
        public FilteredCamelCasePropertyNamesContractResolver(Type[] types, Assembly[] assemblies)
            : this()
        {
            if (types != null && types.Length > 0)
            {
                foreach (var type in types)
                {
                    TypesToInclude.Add(type);
                }
            }
            if (assemblies != null && assemblies.Length > 0)
            {
                foreach (var asm in assemblies)
                {
                    AssembliesToInclude.Add(asm);
                }
            }
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            Type declaringType = member.DeclaringType;
            if (
                TypesToInclude.Contains(declaringType)
                || AssembliesToInclude.Contains(declaringType.Assembly))
            {
                jsonProperty.PropertyName = ToCamelCase(jsonProperty.PropertyName);
            }
            return jsonProperty;
        }

        public static string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            var firstChar = value[0];
            if (char.IsLower(firstChar))
            {
                return value;
            }
            firstChar = char.ToLowerInvariant(firstChar);
            return firstChar + value.Substring(1);
        }
    }
}
