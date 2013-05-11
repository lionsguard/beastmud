using Beast.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Json
{
    public class JsonInput : InputBase
    {
        public JsonInput()
        {
        }

        public JsonInput(IDictionary<string, object> values)
            : base(values)
        {
        }

        public JsonInput(string json)
        {
            Load(JsonConvert.DeserializeObject<Dictionary<string, object>>(json).ToArray());
        }

        public override IOutput CreateOutput()
        {
            throw new NotImplementedException();
        }
    }
}
