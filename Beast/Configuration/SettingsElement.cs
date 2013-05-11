using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Configuration
{
    public class SettingsElement : ConfigurationElement
    {
        public const string KeyRootPath = "rootPath";
        public const string KeyUpdateInterval = "updateInterval";
        public const string KeyConnectionTimeout = "connectionTimeout";
        public const string KeyComponentDirectories = "componentDirectories";
        public const string KeyComponentAssemblies = "componentAssemblies";

        [ConfigurationProperty(KeyRootPath)]
        public string RootPath
        {
            get { return (string)base[KeyRootPath]; }
            set { base[KeyRootPath] = value; }
        }

        [ConfigurationProperty(KeyUpdateInterval)]
        public string UpdateInterval
        {
            get { return (string)base[KeyUpdateInterval]; }
            set { base[KeyUpdateInterval] = value; }
        }

        [ConfigurationProperty(KeyConnectionTimeout)]
        public string ConnectionTimeout
        {
            get { return (string)base[KeyConnectionTimeout]; }
            set { base[KeyConnectionTimeout] = value; }
        }

        [ConfigurationProperty(KeyComponentDirectories)]
        public DirectoryElementCollection ComponentDirectories
        {
            get { return (DirectoryElementCollection)base[KeyComponentDirectories]; }
            set { base[KeyComponentDirectories] = value; }
        }

        [ConfigurationProperty(KeyComponentAssemblies)]
        public AssemblyElementCollection ComponentAssemblies
        {
            get { return (AssemblyElementCollection)base[KeyComponentAssemblies]; }
            set { base[KeyComponentAssemblies] = value; }
        }
    }
}
