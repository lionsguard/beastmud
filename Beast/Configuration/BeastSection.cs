using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Configuration
{
    public class BeastSection : ConfigurationSection
    {
        public const string KeySettings = "settings";

        [ConfigurationProperty(KeySettings)]
        public SettingsElement Settings
        {
            get { return (SettingsElement)base[KeySettings]; }
            set { base[KeySettings] = value; }
        }
    }
}
