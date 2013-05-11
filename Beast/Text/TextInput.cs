using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Text
{
    public class TextInput : InputBase
    {
        public TextInput()
        {
        }

        public TextInput(string input)
        {
            Parse(input);
        }

        public void Parse(string input)
        {
            var words = input.Split(' ');
            if (words.Length > 0)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    Add(i.ToString(), words[i]);
                }
            }
        }

        public override IOutput CreateOutput()
        {
            throw new NotImplementedException();
        }
    }
}
