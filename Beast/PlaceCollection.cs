using System;
using System.Collections.Generic;

namespace Beast
{
	public class PlaceCollection : Dictionary<string, Place>
	{
		public PlaceCollection()
			: base(StringComparer.InvariantCultureIgnoreCase)
		{
			
		}

		public void Add(Place place)
		{
			Add(place.Id, place);
		}
	}
}