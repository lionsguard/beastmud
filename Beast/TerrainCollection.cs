using System.Collections.Generic;

namespace Beast
{
	public class TerrainCollection : Dictionary<int, Terrain>
	{
		public void Add(Terrain terrain)
		{
			Add(terrain.Id, terrain);
		}

		public bool Contains(Terrain terrain)
		{
			return ContainsKey(terrain.Id);
		}
	}
}