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

		public static NotificationMessage Normal(string text)
		{
			return new NotificationMessage
			       	{
			       		Category = NotificationCategories.Normal,
						Text = text
			       	};
		}

		public static NotificationMessage Error(string text)
		{
			return new NotificationMessage
			{
				Category = NotificationCategories.Error,
				Text = text
			};
		}

		public static NotificationMessage Name(string text)
		{
			return new NotificationMessage
			{
				Category = NotificationCategories.Name,
				Text = text
			};
		}

		public static NotificationMessage Description(string text)
		{
			return new NotificationMessage
			{
				Category = NotificationCategories.Description,
				Text = text
			};
		}

		public static NotificationMessage Positive(string text)
		{
			return new NotificationMessage
			{
				Category = NotificationCategories.Positive,
				Text = text
			};
		}

		public static NotificationMessage Negative(string text)
		{
			return new NotificationMessage
			{
				Category = NotificationCategories.Negative,
				Text = text
			};
		}

		public static NotificationMessage Heading(string text)
		{
			return new NotificationMessage
			{
				Category = NotificationCategories.Heading,
				Text = text
			};
		}

		public static NotificationMessage Location(string text)
		{
			return new NotificationMessage
			{
				Category = NotificationCategories.Location,
				Text = text
			};
		}
	}
}