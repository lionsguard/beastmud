namespace Beast.Net
{
	public class HttpConnectionFactory<T> : IConnectionFactory where T : HttpConnection, new()
	{
		public IMessageFormatter<string> Formatter { get; private set; }

		public HttpConnectionFactory(IMessageFormatter<string> formatter)
		{
			Formatter = formatter;
		}

		public IConnection CreateConnection()
		{
			return new T
			       	{
			       		Formatter = Formatter
			       	};
		}
	}
}