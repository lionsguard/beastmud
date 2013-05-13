using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Beast.Data
{
    public abstract class DefaultDataProvider : IInitializable
    {
        protected Application App { get; private set; }
        protected string RootPath { get; set; }

        public void Initialize(Application app)
        {
            App = app;
            RootPath = Path.Combine(App.Settings.RootPath, App.Settings.GetValue(DataSettingsKeys.DataDirectoryKey, "App_Data"));
            OnInitialized();
        }
        protected virtual void OnInitialized()
        {
        }

        public void Shutdown()
        {
            OnShutttingDown();
        }
        protected virtual void OnShutttingDown()
        {
        }

        protected string GetPath(params string[] paths)
        {
            var all = new List<string>(paths);
            all.Insert(0, RootPath);
            return Path.Combine(all.ToArray());
        }

        protected virtual T GetObject<T>(string fileName)
        {
            if (!File.Exists(fileName))
                return default(T);

            using (var reader = new StreamReader(fileName))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
        }

        protected virtual void SaveObject<T>(T obj, string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            using (var writer = new StreamWriter(fileName, false))
            {
                writer.Write(JsonConvert.SerializeObject(obj));
            }
        }
    }
}
