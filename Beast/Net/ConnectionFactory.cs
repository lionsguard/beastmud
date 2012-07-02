namespace Beast.Net
{
	public class ConnectionFactory<TConnection, TMessageFormat> : IConnectionFactory 
		where TConnection : IFormatterConnection<TMessageFormat>, new()
	{
		public IMessageFormatter<TMessageFormat> Formatter { get; private set; }

		public ConnectionFactory(IMessageFormatter<TMessageFormat> formatter)
		{
			Formatter = formatter;
		}

		public IConnection CreateConnection()
		{
			return new TConnection
			       	{
			       		Formatter = Formatter
			       	};
		}
	}
}