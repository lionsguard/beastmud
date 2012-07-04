
using System.Collections.Generic;

namespace Beast
{
	/// <summary>
	/// Represents active world data including places, objects and mobiles.
	/// </summary>
	public static class World
	{
		private static List<Terrain> _terrain;
		/// <summary>
		/// Gets a list of the current terrain loaded into the active world.
		/// </summary>
		public static IEnumerable<Terrain> Terrain
		{
			get { return _terrain ?? (_terrain = new List<Terrain>(Game.Current.Repository.GetTerrain())); }
		}

		/// <summary>
		/// Gets a collection of current loaded places.
		/// </summary>
		public static PlaceCollection Places { get; private set; }

		/// <summary>
		/// Initializes the game world.
		/// </summary>
		internal static void Initialize()
		{
			Places = new PlaceCollection();
		}

		#region Place Methods
		public static Place GetPlace(string id)
		{
			if (!Places.ContainsKey(id))
			{
				var place = Game.Current.Repository.GetPlace(id);
				if (place != null)
					Places[place.Id] = place;
			}
			return Places[id];
		}
		public static void SavePlace(Place place)
		{
			Game.Current.Repository.SavePlace(place);
			Places[place.Id] = place;
		}
		#endregion
	}
}
