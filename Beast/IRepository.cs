
using Beast.Commands;

namespace Beast
{
	public interface IRepository
	{
		#region Command Definitions

		CommandDefinition GetCommandDefinition(string name);
		void SaveCommandDefinition(CommandDefinition definition);

		#endregion
	}
}
