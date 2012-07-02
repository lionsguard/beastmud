
using System.Collections.Generic;
using System.Web;

namespace Beast.Net
{
	public class HttpConnection : ConnectionBase, IFormatterConnection<string>
	{
		#region Factory
		private static readonly ConnectionFactory<HttpConnection, string> _sFactory = new ConnectionFactory<HttpConnection, string>(new JsonMessageFormatter());
		public static ConnectionFactory<HttpConnection,string> Factory
		{
			get { return _sFactory; }
		}
		#endregion

		public IMessageFormatter<string> Formatter { get; set; }

		private readonly Queue<IMessage> _outputMessages = new Queue<IMessage>();

		public void ProcessInput(IInput input, HttpResponse response)
		{
			Game.Current.Commands.Execute(input, this);
			Flush();

			ProcessOutput(response);
		}

		public void ProcessOutput(HttpResponse response)
		{
			lock (_outputMessages)
			{
				while (_outputMessages.Count > 0)
				{
					response.Write(Formatter.FormatMessage(_outputMessages.Dequeue()));
				}
			}
		}

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
	}
}
