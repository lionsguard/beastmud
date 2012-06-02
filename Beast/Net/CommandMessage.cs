using System;
using Beast.Commands;

namespace Beast.Net
{
	public class CommandMessage : Message
	{
		public override MessageType Type
		{
			get { return MessageType.Command; }
		}

		public string Command { get; set; }
		public bool Success { get; set; }
		public string Error { get; set; }
		public object Data { get; set; }

		public CommandMessage(string command)
			: this(Guid.NewGuid().ToString(), command)
		{
		}

		public CommandMessage(string id, string command)
			: base(id)
		{
			Command = command;
			Success = true;
		}

		public CommandMessage(Command command)
			: this(command.Id, command.Name)
		{
			
		}

		public CommandMessage Invalidate(string error)
		{
			Success = false;
			Error = error;
			return this;
		}

		public CommandMessage Invalidate(string format, params object[] args)
		{
			return Invalidate(string.Format(format, args));
		}
	}
}