using System.Collections.Generic;

namespace Beast.Data
{
	public interface IPlaceRepository : IRepository
	{
		IEnumerable<Terrain> GetTerrain();
		void SaveTerrain(Terrain terrain);

		long GetPlaceCount();
		Place GetPlace(string id);
		void SavePlace(Place place);
	}
}