using System;

namespace Beast.Net
{
	public abstract class Message : IMessage
	{
		public string Id { get; set; }
		public abstract MessageType Type { get; }

		protected Message()
		{
			Id = Guid.NewGuid().ToString();
		}
		protected Message(string id)
		{
			Id = id;
		}
	}
}
