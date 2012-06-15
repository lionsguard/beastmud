
using System.Collections.Generic;
using Beast.Commands;
using Beast.Configuration;

namespace Beast.Data
{
	public interface IRepository
	{
		void Initialize();

		#region Command Definitions

		CommandDefinition GetCommandDefinition(string name);
		void SaveCommandDefinition(CommandDefinition definition);

		#endregion

		#region Zones
		IEnumerable<Zone> GetZones();
		Zone GetZone(string id);
		void SaveZone(Zone zone);
		#endregion

		RepositoryElement ToConfig();
		void FromConfig(RepositoryElement config);
	}
}
