using Beast.Security;

namespace Beast.Data
{
	public interface IUserRepository : IRepository
	{
		long GetUserCount();
		User GetUser(string username);
		User GetUserById(string id);
		void SaveUser(User user);
	}
}