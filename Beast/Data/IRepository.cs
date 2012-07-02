using System.Collections.Generic;
using Beast.Configuration;
using Beast.Mobiles;
using Beast.Security;

namespace Beast.Data
{
	public interface IRepository
	{
		void Initialize();

		IEnumerable<Terrain> GetTerrain();
		void SaveTerrain(Terrain terrain);

		IGameObject GetTemplate(string templateName);
		void SaveTemplate(IGameObject obj);

		long GetUserCount();
		User GetUser(string username);
		User GetUserById(string id);
		void SaveUser(User user);

		Place GetPlace(Unit location);
		void SavePlace(Place place);

		long GetCharacterCount();
		IEnumerable<Character> GetCharacters(string userId);
		Character GetCharacter(string id);
		void SaveCharacter(Character character);

		RepositoryElement ToConfig();
		void FromConfig(RepositoryElement config);
	}
}
