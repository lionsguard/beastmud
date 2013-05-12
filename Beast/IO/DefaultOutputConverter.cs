using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.IO
{
    public class DefaultOutputConverter : IOutputConverter
    {
        public object ConvertOutput(IOutput output)
        {
            return output.ToString();
        }
    }
}
