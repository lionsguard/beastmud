
namespace Beast.Net
{
	public class SocketConnectionFactory : IConnectionFactory
	{
		private readonly IMessageFormatter<byte[]> _formatter;

		public SocketConnectionFactory(IMessageFormatter<byte[]> formatter)
		{
			_formatter = formatter;
		}

		#region Implementation of IConnectionFactory

		public IConnection CreateConnection()
		{
			return new SocketConnection(_formatter);
		}

		#endregion
	}
}