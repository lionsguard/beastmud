
using System.Collections;
using System.Collections.Generic;

namespace Beast
{
	/// <summary>
	/// Represents in game world data including zones, places, objects and mobiles.
	/// </summary>
	public class World : IEnumerable<Place>
	{
		private readonly Dictionary<Unit, Place> _places = new Dictionary<Unit, Place>();

		public Place this[Unit location]
		{
			get 
			{ 
				Place place;
				_places.TryGetValue(location, out place);
				return place;
			}
		}

		public Place this[int x, int y, int z]
		{
			get { return this[new Unit(x, y, z)]; }
		}

		/// <summary>
		/// Initializes a new instance of the World class.
		/// </summary>
		internal World()
		{
			
		}

		#region Implementation of IEnumerable

		public IEnumerator<Place> GetEnumerator()
		{
			return _places.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
