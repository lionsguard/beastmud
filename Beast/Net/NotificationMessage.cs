namespace Beast.Net
{
	public class NotificationMessage : Message
	{
		public override MessageType Type
		{
			get { return MessageType.Notification; }
		}

		public int Category { get; set; }
		public string Text { get; set; }
	}
}