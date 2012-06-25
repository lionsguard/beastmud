
using Beast.Configuration;

namespace Beast.Data
{
	public interface IRepository
	{
		void Initialize();

		RepositoryElement ToConfig();
		void FromConfig(RepositoryElement config);
	}
}
