namespace Beast
{
	public class Terrain
	{
		public static readonly Terrain Empty = new Terrain { Id = 0, Color = "000000", Name = "None", WalkType = WalkTypes.None };

		public int Id { get; set; }
		public string Name { get; set; }
		public string Color { get; set; }
		public WalkTypes WalkType { get; set; }
	}
}
