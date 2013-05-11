using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Configuration
{
    public class AssemblyElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var asm = element as AssemblyElement;
            if (asm == null)
                return null;
            return asm.File;
        }
    }
}
