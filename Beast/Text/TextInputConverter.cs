using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beast.IO;

namespace Beast.Text
{
    public class TextInputConverter : IInputConverter
    {
        public IInput ConvertInput<T>(T data)
        {
            if (typeof(T) == typeof(byte[]))
                return new TextInput((byte[])Convert.ChangeType(data, typeof(byte[])));

            return new TextInput(data.ToString());
        }
    }
}
