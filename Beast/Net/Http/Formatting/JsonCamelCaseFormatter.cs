using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Beast.Net.Http.Formatting
{
    public class JsonCamelCaseFormatter : MediaTypeFormatter
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonCamelCaseFormatter()
        {
            _jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            // Fill out the mediatype and encoding we support
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(new UTF8Encoding(false, true));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);

            return Task.Factory.StartNew(() =>
                {
                    using (var sr = new StreamReader(readStream))
                    {
                        using (var reader = new JsonTextReader(sr))
                        {
                            return serializer.Deserialize(reader, type);
                        }
                    }
                });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);

            return Task.Factory.StartNew(() =>
                {
                    using (var sw = new StreamWriter(writeStream))
                    {
                        using (var writer = new JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer, value);
                            writer.Flush();
                        }
                    }
                });
        }
    }
}
