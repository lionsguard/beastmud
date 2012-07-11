using System.Collections.Generic;
using Beast.Mobiles;

namespace Beast.Data
{
	public interface ICharacterRepository : IRepository
	{
		long GetCharacterCount();
		IEnumerable<Character> GetCharacters(string userId);
		Character GetCharacter(string id);
		Character GetCharacterByName(string name);
		void SaveCharacter(Character character);
	}
}