using System;
using System.Collections.Generic;
using Beast.Configuration;
using Beast.Data;
using Beast.Mobiles;
using Beast.Security;

namespace Beast.Tests
{
	public class TestRepository : IRepository
	{
		public void Initialize()
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

		public Place GetPlace(Unit location)
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

		public RepositoryElement ToConfig()
		{
			return new RepositoryElement
			       	{
			       		Type = GetType().AssemblyQualifiedName
			       	};
		}

		public void FromConfig(RepositoryElement config)
		{
		}
	}
}