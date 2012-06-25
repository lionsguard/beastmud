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
					return FormatCommand((ResponseMessage) msg);
				case MessageType.Property:
					return FormatProperty((PropertyDeltaMessage) msg);
				case MessageType.Event:
					return FormatEvent((EventDeltaMessage) msg);
				case MessageType.Notification:
					return FormatNotification((NotificationMessage) msg);
			}
			return string.Empty;
		}

		protected virtual string FormatCommand(ResponseMessage msg)
		{
			return string.Empty;
		}

		protected virtual string FormatProperty(PropertyDeltaMessage msg)
		{
			return string.Empty;
		}

		protected virtual string FormatEvent(EventDeltaMessage msg)
		{
			return string.Empty;
		}

		protected virtual string FormatNotification(NotificationMessage msg)
		{
			var txt = new AnsiText(msg.Text);
			switch (msg.Category)
			{
				case NotificationCategories.Error:
					txt.Color(AnsiColor.Red).Bold();
					break;
				case NotificationCategories.Name:
					txt.Color(AnsiColor.Yellow).Bold();
					break;
				case NotificationCategories.Description:
					txt.Color(AnsiColor.Cyan);
					break;
				case NotificationCategories.Positive:
					txt.Color(AnsiColor.Green);
					break;
				case NotificationCategories.Negative:
					txt.Color(AnsiColor.Red);
					break;
				case NotificationCategories.Heading:
					txt.Color(AnsiColor.Yellow);
					break;
				case NotificationCategories.Location:
					txt.Color(AnsiColor.Green);
					break;
			}
			return txt.ToString();
		}
	}
}
