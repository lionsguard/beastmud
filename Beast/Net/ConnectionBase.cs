using System;
using System.Collections.Generic;
using Beast.Commands;
using Beast.Mobiles;

namespace Beast.Net
{
	/// <summary>
	/// An abstract base class for creating IConnection instances.
	/// </summary>
	public abstract class ConnectionBase : IConnection
	{
		#region Implementation of IConnection

		public string Id { get; set; }
		public DateTime LastActivity { get; set; }
		public object User { get; set; }
		public Character Character { get; set; }

		protected bool IsClosed { get; set; }

		private readonly Queue<Command> _commands = new Queue<Command>();
		private readonly Queue<IMessage> _messages = new Queue<IMessage>();
		private readonly Queue<Command> _history = new Queue<Command>(100);

		protected ConnectionBase()
		{
			Id = Guid.NewGuid().ToString();
			LastActivity = DateTime.UtcNow;
		}

		public void EnqueueCommand(Command command)
		{
			if (IsClosed)
				return;

			lock (_commands)
			{
				_commands.Enqueue(command);

				_history.Enqueue(command);
				if (_history.Count > 100)
					_history.Dequeue();
			}
		}

		public IEnumerable<Command> DequeueCommands()
		{
			lock (_commands)
			{
				while (_commands.Count > 0)
				{
					yield return _commands.Dequeue();
				}
			}
		}

		public void Write(params IMessage[] messages)
		{
			if (IsClosed)
				return;

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

		public void Flush()
		{
			if (IsClosed)
				return;

			var list = new List<IMessage>();
			lock (_messages)
			{
				while (_messages.Count > 0)
				{
					list.Add(_messages.Dequeue());
				}
			}
			FlushOverride(list);
		}

		/// <summary>
		/// Executes a custom routine for sending data to the connection's output stream.
		/// </summary>
		/// <param name="messages">The list of messages to send.</param>
		protected abstract void FlushOverride(IEnumerable<IMessage> messages);

		public void Close()
		{
			IsClosed = true;
			lock (_commands)
			{
				_commands.Clear();
			}
			lock (_messages)
			{
				_messages.Clear();
			}
			CloseOverride();
		}

		/// <summary>
		/// Executes custom code required when a connection is closed.
		/// </summary>
		protected virtual void CloseOverride()
		{
		}

		#endregion
	}
}