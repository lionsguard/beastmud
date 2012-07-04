
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
		/// Initializes the game world.
		/// </summary>
		internal static void Initialize()
		{
		}
	}
}
