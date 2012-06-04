using Beast.Net;

namespace Beast.Text
{
	public class BasicMessageParser : IMessageParser
	{
		public string Parse(IMessage message)
		{
			var msg = message as Message;
			if (msg == null)
				return string.Empty;

			switch (msg.Type)
			{
				case MessageType.Command:
					break;
				case MessageType.Property:
					break;
				case MessageType.Event:
					break;
				case MessageType.Notification:
					return FormatNotification((NotificationMessage) msg);
			}
			return string.Empty;
		}

		protected virtual string FormatNotification(NotificationMessage msg)
		{
			var txt = new AnsiText(msg.Text);
			switch (msg.Category)
			{
				case 1: // error
					txt.Color(AnsiColor.Red).Bold();
					break;
				case 2: // name
					txt.Color(AnsiColor.Yellow).Bold();
					break;
				case 3: // description
					txt.Color(AnsiColor.Cyan);
					break;
				case 4: // positive
					txt.Color(AnsiColor.Green);
					break;
				case 5: // negative
					txt.Color(AnsiColor.Red);
					break;
				case 6: // heading
					txt.Color(AnsiColor.Yellow);
					break;
			}
			return txt.ToString();
		}
	}
}
