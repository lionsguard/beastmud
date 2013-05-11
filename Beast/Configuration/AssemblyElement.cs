using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Configuration
{
    public class AssemblyElement : ConfigurationElement
    {
        public const string KeyFile = "file";

        [ConfigurationProperty(KeyFile)]
        public string File
        {
            get { return (string)base[KeyFile]; }
            set { base[KeyFile] = value; }
        }
    }
}
