using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Text
{
    public class BasicTextParser : ITextParser
    {
        public IEnumerable<KeyValuePair<string, object>> Parse(string input)
        {
            var values = new Dictionary<string, object>();

            var words = input.Split(' ');
            if (words.Length > 0)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    values.Add(i.ToString(), words[i]);
                }
            }

            return values;
        }
    }
}
