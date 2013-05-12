using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Text
{
    public class TextInput : BasicInput
    {
        public TextInput()
        {
        }

        public TextInput(string input)
        {
            Parse(input);
        }

        public TextInput(byte[] input)
        {
            FromBytes(input);
        }

        public void Parse(string input)
        {
            var parser = DependencyResolver.Resolve<ITextParser>() ?? new BasicTextParser();

            var values = parser.Parse(input);
            foreach (var item in values)
            {
                Add(item.Key, item.Value);
            }
        }

        public override IOutput CreateOutput()
        {
            throw new NotImplementedException();
        }

        public override void FromBytes(byte[] data)
        {
            Parse(Encoding.ASCII.GetString(data));
        }

        public override void FromString(string data)
        {
            Parse(data);
        }

        public override void FromJson(string json)
        {
            Parse(json);
        }
    }
}
