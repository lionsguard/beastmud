
using Beast.Configuration;
using Beast.Security;

namespace Beast.Data
{
	public interface IRepository
	{
		void Initialize();

		IGameObject GetTemplate(string templateName);
		void SaveTemplate(IGameObject obj);

		long GetUserCount();
		User GetUser(Login login);
		void SaveUser(User user);

		RepositoryElement ToConfig();
		void FromConfig(RepositoryElement config);
	}
}
