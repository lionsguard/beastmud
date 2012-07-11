
namespace Beast
{
	public class TerrainCollection : OwnedPropertyCollection<Terrain>
	{
		public const string TerrainPrefix = "Terrain";

		public TerrainCollection(IDataObject owner) : base(owner, TerrainPrefix)
		{
		}

		public void Add(Terrain terrain)
		{
			Set(terrain.Id.ToString(), terrain);
		}

		public bool Contains(Terrain terrain)
		{
			return ContainsKey(terrain.Id.ToString());
		}
	}
}