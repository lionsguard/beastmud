using Beast.Net;

namespace Beast.Text
{
	public interface IMessageParser
	{
		string Parse(IMessage message);
	}
}