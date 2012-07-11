
using System.Collections.Generic;

namespace Beast
{
	public interface IWorld : IDataObject
	{
		TerrainCollection Terrain { get; }
		PlaceCollection Places { get; }

		void Initialize();

		Place GetPlace(string id);
		void SavePlace(Place place);
		IEnumerable<Place> GetPlaceMap(Place center, int radius);
	}
}
