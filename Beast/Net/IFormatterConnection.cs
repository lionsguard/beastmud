namespace Beast.Net
{
	public interface IFormatterConnection<T> : IConnection
	{
		IMessageFormatter<T> Formatter { get; set; }
	}
}