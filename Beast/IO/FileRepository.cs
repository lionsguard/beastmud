using System.Configuration;
using System.IO;
using Beast.Configuration;
using Beast.Data;
using Newtonsoft.Json;

namespace Beast.IO
{
	public class FileRepository : IRepository
	{
		public static readonly ConfigurationProperty ConfigKeyPath = new ConfigurationProperty("path", typeof(string));

		public const string FileExt = ".txt";
		public const string HelpDirectory = "help";

		public string DirectoryPath { get; set; }

		public override string ToString()
		{
			return string.Format("FileRepository: PATH={0}", DirectoryPath);
		}

		private string GetFileName(string directory, string name)
		{
			return Path.Combine(Path.Combine(DirectoryPath, directory), string.Concat(name, FileExt));
		}

		private T Load<T>(string directory, string name)
		{
			return JsonConvert.DeserializeObject<T>(File.ReadAllText(GetFileName(directory, name)));
		}
		private void Save<T>(string directory, string name, T obj)
		{
			File.WriteAllText(GetFileName(directory, name), JsonConvert.SerializeObject(obj));
		}

		public void Initialize()
		{
		}

		public RepositoryElement ToConfig()
		{
			var config = new RepositoryElement
			             	{
								Type = GetType().AssemblyQualifiedName
			             	};
			config[ConfigKeyPath] = DirectoryPath;
			return config;
		}

		public void FromConfig(RepositoryElement config)
		{
			DirectoryPath = (string)config[ConfigKeyPath];
		}
	}
}
