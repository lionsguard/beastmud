using System;
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
