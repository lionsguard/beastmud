using Beast.Net;

namespace Beast.Mobiles
{
	public abstract class Character : Mobile
	{
		public IConnection Connection { get; set; }
	}
}