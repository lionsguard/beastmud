using System.Collections.Generic;
using System.ComponentModel.Composition;
using Beast.Mobiles;
using Beast.Security;

namespace Beast.Data
{
	public class RepositoryManager : IUserRepository, ITemplateRepository, IPlaceRepository, ICharacterRepository, IWorldRepository, IRepository
	{
		[Import(typeof(IUserRepository))]
		public IUserRepository Users { get; set; }

		[Import(typeof(ITemplateRepository))]
		public ITemplateRepository Templates { get; set; }

		[Import(typeof(IPlaceRepository))]
		public IPlaceRepository Places { get; set; }

		[Import(typeof(ICharacterRepository))]
		public ICharacterRepository Characters { get; set; }

		[Import(typeof(IWorldRepository))]
		public IWorldRepository Worlds { get; set; }

		internal RepositoryManager()
		{
		}

		#region Implementation of IRepository

		public void Initialize()
		{
			Users.Initialize();
			Templates.Initialize();
			Places.Initialize();
			Characters.Initialize();
		}

		public void Shutdown()
		{
			Users.Shutdown();
			Templates.Shutdown();
			Places.Shutdown();
			Characters.Shutdown();
		}

		#endregion

		#region Implementation of IUserRepository

		public long GetUserCount()
		{
			return Users.GetUserCount();
		}

		public User GetUser(string username)
		{
			return Users.GetUser(username);
		}

		public User GetUserById(string id)
		{
			return Users.GetUserById(id);
		}

		public void SaveUser(User user)
		{
			Users.SaveUser(user);
		}

		#endregion

		#region Implementation of ITemplateRepository

		public long GetTemplateCount()
		{
			return Templates.GetTemplateCount();
		}

		public IGameObject GetTemplate(string templateName)
		{
			return Templates.GetTemplate(templateName);
		}

		public void SaveTemplate(IGameObject obj)
		{
			Templates.SaveTemplate(obj);
		}

		#endregion

		#region Implementation of IPlaceRepository

		public long GetPlaceCount()
		{
			return Places.GetPlaceCount();
		}

		public Place GetPlace(string id)
		{
			return Places.GetPlace(id);
		}

		public void SavePlace(Place place)
		{
			Places.SavePlace(place);
		}

		#endregion

		#region Implementation of ICharacterRepository

		public long GetCharacterCount()
		{
			return Characters.GetCharacterCount();
		}

		public IEnumerable<Character> GetCharacters(string userId)
		{
			return Characters.GetCharacters(userId);
		}

		public Character GetCharacter(string id)
		{
			return Characters.GetCharacter(id);
		}

		public Character GetCharacterByName(string name)
		{
			return Characters.GetCharacterByName(name);
		}

		public void SaveCharacter(Character character)
		{
			Characters.SaveCharacter(character);
		}

		#endregion

		public IWorld GetWorld()
		{
			return Worlds.GetWorld();
		}

		public void SaveWorld<T>(T world) where T : class, IWorld
		{
			Worlds.SaveWorld(world);
		}
	}
}
