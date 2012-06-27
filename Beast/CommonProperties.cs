namespace Beast
{
	public class CommonProperties
	{
		public static readonly Property Id = new Property("Id", typeof(string), null);
		public static readonly Property Name = new Property("Name",typeof(string), null);
		public static readonly Property Description = new Property("Description", typeof(string), null);
		public static readonly Property Body = new Property("Body", typeof(BoundProperty<int>), new BoundProperty<int>());
		public static readonly Property Mind = new Property("Mind", typeof(BoundProperty<int>), new BoundProperty<int>());
		public static readonly Property IsAtRest = new Property("IsAtRest", typeof(bool), true);
	}
}