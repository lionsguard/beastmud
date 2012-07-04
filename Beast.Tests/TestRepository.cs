using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Beast.Data;
using Beast.Mobiles;
using Beast.Security;

namespace Beast.Tests
{
	[Export(typeof(IUserRepository))]
	[Export(typeof(ITemplateRepository))]
	[Export(typeof(IPlaceRepository))]
	[Export(typeof(ICharacterRepository))]
	public class TestRepository : IUserRepository, ITemplateRepository, IPlaceRepository, ICharacterRepository
	{
		public void Initialize()
		{
		}

		public void Shutdown()
		{
		}

		public IEnumerable<Terrain> GetTerrain()
		{
			throw new NotImplementedException();
		}

		public void SaveTerrain(Terrain terrain)
		{
			throw new NotImplementedException();
		}

		public long GetTemplateCount()
		{
			throw new NotImplementedException();
		}

		public IGameObject GetTemplate(string templateName)
		{
			throw new NotImplementedException();
		}

		public void SaveTemplate(IGameObject obj)
		{
			throw new NotImplementedException();
		}

		public long GetUserCount()
		{
			throw new NotImplementedException();
		}

		public User GetUser(string username)
		{
			throw new NotImplementedException();
		}

		public User GetUserById(string id)
		{
			throw new NotImplementedException();
		}

		public void SaveUser(User user)
		{
			throw new NotImplementedException();
		}

		public long GetPlaceCount()
		{
			throw new NotImplementedException();
		}

		public Place GetPlace(string id)
		{
			throw new NotImplementedException();
		}

		public void SavePlace(Place place)
		{
			throw new NotImplementedException();
		}

		public long GetCharacterCount()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Character> GetCharacters(string userId)
		{
			throw new NotImplementedException();
		}

		public Character GetCharacter(string id)
		{
			throw new NotImplementedException();
		}

		public void SaveCharacter(Character character)
		{
			throw new NotImplementedException();
		}
	}
}