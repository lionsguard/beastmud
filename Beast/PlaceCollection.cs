using System.Collections.Generic;

namespace Beast
{
	public class PlaceCollection : Dictionary<Unit, Place>
	{
		public void Add(Place place)
		{
			Add(place.Location, place);
		}
	}
}