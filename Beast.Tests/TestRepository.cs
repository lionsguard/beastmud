using System;
using Beast.Configuration;
using Beast.Data;
using Beast.Security;

namespace Beast.Tests
{
	public class TestRepository : IRepository
	{
		public void Initialize()
		{
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

		public User GetUser(Login login)
		{
			throw new NotImplementedException();
		}

		public void SaveUser(User user)
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