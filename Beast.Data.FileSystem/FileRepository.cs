using System;
using System.Configuration;
using System.IO;
using Beast.Configuration;
using Beast.Security;
using Newtonsoft.Json;

namespace Beast.Data
{
	public class FileRepository : IRepository
	{
		public static readonly ConfigurationProperty ConfigKeyPath = new ConfigurationProperty("path", typeof(string));
		private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.Objects};

		public const string Places = "places";
		public const string Templates = "templates";
		public const string Characters = "characters";
		public const string Users = "users";
		public const string Logins = Users + "\\logins";
		public const string FileExt = ".beast";
		public const string IndexFileName = "index";

		public string DirectoryPath { get; set; }

		private readonly object _userIndexLock = new object();

		public override string ToString()
		{
			return string.Format("FileRepository: PATH={0}", DirectoryPath);
		}

		private string GetDirectory(string directory)
		{
			return Path.Combine(DirectoryPath, directory);
		}

		private string GetFileName(string directory, string name)
		{
			return Path.Combine(GetDirectory(directory), string.Concat(name, FileExt));
		}

		private T Load<T>(string directory, string name)
		{
			return File.ReadAllText(GetFileName(directory, name)).FromJson<T>(JsonSettings);
		}
		private void Save<T>(string directory, string name, T obj)
		{
			if (!Directory.Exists(GetDirectory(directory)))
				Directory.CreateDirectory(GetDirectory(directory));
			File.WriteAllText(GetFileName(directory, name), obj.ToJson(JsonSettings));
		}

		public void Initialize()
		{
		}

		public IGameObject GetTemplate(string templateName)
		{
			return Load<GameObject>(Templates, templateName);
		}

		public void SaveTemplate(IGameObject obj)
		{
			if (string.IsNullOrEmpty(obj.Id))
				obj.Id = Guid.NewGuid().ToString();
			Save(Templates, obj.Name, obj);
		}

		public long GetUserCount()
		{
			return Directory.GetFiles(GetDirectory(Users), string.Concat("*", FileExt)).Length;
		}

		public User GetUser(Login login)
		{
			var loginData = Load<string>(Logins, login.UserName);
			return loginData == null ? null : Load<User>(Users, loginData);
		}

		public void SaveUser(User user)
		{
			if (string.IsNullOrEmpty(user.Id))
				user.Id = Guid.NewGuid().ToString();
			Save(Users, user.Id, user);
			foreach (var login in user.Logins)
			{
				Save(Logins, login.UserName, user.Id);
			}
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
