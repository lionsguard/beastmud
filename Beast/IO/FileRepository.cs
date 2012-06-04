
using System.IO;
using Beast.Commands;
using Newtonsoft.Json;

namespace Beast.IO
{
	public class FileRepository : IRepository
	{
		public const string FileExt = ".txt";
		public const string HelpDirectory = "help";

		public string DirectoryPath { get; set; }

		public FileRepository(string path)
		{
			DirectoryPath = path;
		}

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

		#region CommandDefinition
		public CommandDefinition GetCommandDefinition(string name)
		{
			return Load<CommandDefinition>(HelpDirectory, name);
		}

		public void SaveCommandDefinition(CommandDefinition definition)
		{
			Save(HelpDirectory, definition.Name, definition);
		}
		#endregion
	}
}
