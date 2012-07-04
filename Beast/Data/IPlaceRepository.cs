using System.Collections.Generic;

namespace Beast.Data
{
	public interface IPlaceRepository : IRepository
	{
		IEnumerable<Terrain> GetTerrain();
		void SaveTerrain(Terrain terrain);

		long GetPlaceCount();
		Place GetPlace(Unit location);
		void SavePlace(Place place);
	}
}