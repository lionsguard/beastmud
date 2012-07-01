
using System.Collections.Generic;
using Beast.Net;

namespace Beast.Mobiles
{
	public class Character : Mobile
	{
		#region Deltas
		private readonly Queue<IMessage> _messages = new Queue<IMessage>();

		public override void EnqueueMessages(params IMessage[] messages)
		{
			if (messages == null || messages.Length == 0)
				return;

			lock (_messages)
			{
				foreach (var message in messages)
				{
					_messages.Enqueue(message);
				}
			}
		}

		public override IEnumerable<IMessage> DequeueMessages()
		{
			lock (_messages)
			{
				while (_messages.Count > 0)
				{
					yield return _messages.Dequeue();
				}
			}
		}
		#endregion
	}
}