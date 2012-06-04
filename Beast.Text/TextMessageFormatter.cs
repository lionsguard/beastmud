using System.Text;
using Beast.Commands;
using Beast.Net;

namespace Beast.Text
{
	public class TextMessageFormatter : IMessageFormatter<byte[]>
	{
		public ITextParser TextParser { get; private set; }
		public IMessageParser MessageParser { get; private set; }

		public TextMessageFormatter()
			: this(new BasicTextParser(), new BasicMessageParser())
		{			
		}

		public TextMessageFormatter(ITextParser textParser, IMessageParser messageParser)
		{
			TextParser = textParser;
			MessageParser = messageParser;
		}

		#region Implementation of IMessageFormatter<byte[]>

		public byte[] FormatMessage(IMessage message)
		{
			return Encoding.ASCII.GetBytes(MessageParser.Parse(message));
		}

		public Command FormatCommand(byte[] data)
		{
			var text = Encoding.ASCII.GetString(data);
			return TextParser.Parse(text);
		}

		#endregion
	}
}
