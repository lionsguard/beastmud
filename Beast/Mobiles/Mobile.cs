using System.Collections.Generic;
using Beast.Net;

namespace Beast.Mobiles
{
	public abstract class Mobile : GameObject
	{
		public Unit Position { get; set; }

		public virtual void EnqueueMessages(params IMessage[] messages)
		{
		}

		public virtual IEnumerable<IMessage> DequeueMessages()
		{
			return new IMessage[0];
		}
	}
}
