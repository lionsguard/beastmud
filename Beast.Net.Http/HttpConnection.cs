
using System.Collections.Generic;

namespace Beast.Net
{
	public abstract class HttpConnection : ConnectionBase
	{
		public IMessageFormatter<string> Formatter { get; set; }
	}

	public class StandardHttpConnection : HttpConnection
	{
		private readonly Queue<IMessage> _outputMessages = new Queue<IMessage>();

		protected override void FlushOverride(IEnumerable<IMessage> messages)
		{
			lock (_outputMessages)
			{
				foreach (var message in messages)
				{
					_outputMessages.Enqueue(message);
				}
			}
		}

		public IEnumerable<string> DequeueMessages()
		{
			lock (_outputMessages)
			{
				while (_outputMessages.Count > 0)
				{
					yield return Formatter.FormatMessage(_outputMessages.Dequeue());
				}
			}
		}
	}
}
