using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Beast.Configuration;
using Beast.Mobiles;
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
		public const string CharactersUsers = Characters + "\\users";
		public const string Users = "users";
		public const string Logins = Users + "\\logins";
		public const string Game = "game";
		public const string FileExt = ".beast";
		public const string IndexFileExt = ".index";
		public const string TerrainFileName = "terrain";
		public const string PlaceFileNameFormat = "{0}_{1}_{2}";
		public const string CharacterIndexFileName = "characters";

		public string DirectoryPath { get; set; }

		private List<Terrain> _terrain; 

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
			var path = GetFileName(directory, name);
			if (!File.Exists(path))
				return default(T);

			return File.ReadAllText(path).FromJson<T>(JsonSettings);
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

		public IEnumerable<Terrain> GetTerrain()
		{
			if (_terrain == null)
			{
				if (!File.Exists(GetFileName(Game, TerrainFileName)))
				{
					Save(Game, TerrainFileName, new List<Terrain>());
				}
				_terrain = Load<List<Terrain>>(Game, TerrainFileName);
			}
			return _terrain;
		}

		public void SaveTerrain(Terrain terrain)
		{
			if (_terrain == null)
				GetTerrain();

			var existing = _terrain.FirstOrDefault(t => t.Id == terrain.Id);
			if (existing != null)
				_terrain.Remove(existing);

			_terrain.Add(terrain);
			Save(Game, TerrainFileName, _terrain);
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

		public User GetUser(string username)
		{
			var loginData = Load<string>(Logins, username);
			return loginData == null ? null : Load<User>(Users, loginData);
		}

		public User GetUserById(string id)
		{
			return Load<User>(Users, id);
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

		public Place GetPlace(Unit location)
		{
			return Load<Place>(Places, GetPlaceFileName(location));
		}

		public void SavePlace(Place place)
		{
			if (string.IsNullOrEmpty(place.Id))
				place.Id = Guid.NewGuid().ToString();
			Save(Places, GetPlaceFileName(place.Location), place);
		}

		public long GetCharacterCount()
		{
			return Directory.GetFiles(GetDirectory(Characters), string.Concat("*", FileExt)).Count();
		}

		public IEnumerable<Character> GetCharacters(string userId)
		{
			var userInfo = Load<UserCharacterInfo>(CharactersUsers, userId) ?? new UserCharacterInfo();
			return userInfo.Chars.Select(c => Load<Character>(Characters, c));
		}

		public Character GetCharacter(string id)
		{
			return Load<Character>(Characters, id);
		}

		public void SaveCharacter(Character character)
		{
			var userId = character.Get<string>(CommonProperties.UserId);
			if (string.IsNullOrEmpty(userId))
				throw new ArgumentNullException("character['UserId']");

			if (string.IsNullOrEmpty(character.Id))
				character.Id = Guid.NewGuid().ToString();

			Save(Characters, character.Id, character);

			var userInfo = Load<UserCharacterInfo>(CharactersUsers, userId) ?? new UserCharacterInfo {UserId = userId};

			if (!userInfo.Chars.Contains(character.Id))
				userInfo.Chars.Add(character.Id);
			Save(CharactersUsers, userId, userInfo);
		}

		private static string GetPlaceFileName(Unit location)
		{
			return string.Format(PlaceFileNameFormat, location.X, location.Y, location.Z);
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

		#region Nested Classes
		private class UserCharacterInfo
		{
			public string UserId { get; set; }
			public List<string> Chars { get; set; }

			public UserCharacterInfo()
			{
				Chars = new List<string>();
			}
		}
		#endregion
	}
}
