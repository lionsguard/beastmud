using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Configuration
{
    public class DirectoryElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DirectoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var dir = element as DirectoryElement;
            if (dir == null)
                return null;
            return dir.Path;
        }
    }
}
