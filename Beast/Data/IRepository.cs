
using Beast.Configuration;

namespace Beast.Data
{
	public interface IRepository
	{
		void Initialize();

		string GetNextObjectId(IGameObject obj);

		IGameObject GetTemplate(string templateName);
		void SaveTemplate(IGameObject obj);

		RepositoryElement ToConfig();
		void FromConfig(RepositoryElement config);
	}
}
