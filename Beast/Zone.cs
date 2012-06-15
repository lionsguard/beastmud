
namespace Beast
{
	public class Zone
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public PlaceCollection Places { get; set; }

		public Zone()
		{
			Places = new PlaceCollection();
		}
	}
}
