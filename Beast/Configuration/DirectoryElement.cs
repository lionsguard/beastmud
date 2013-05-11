using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Configuration
{
    public class DirectoryElement : ConfigurationElement
    {
        public const string KeyPath = "path";

        [ConfigurationProperty(KeyPath)]
        public string Path
        {
            get { return (string)base[KeyPath]; }
            set { base[KeyPath] = value; }
        }
    }
}
