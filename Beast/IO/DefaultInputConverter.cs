using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.IO
{
    public class DefaultInputConverter : IInputConverter
    {
        public IInput ConvertInput<T>(T data)
        {
            return new BasicInput(data.ToString());
        }
    }
}
